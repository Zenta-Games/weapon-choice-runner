using UnityEngine;

namespace Zenta.Core.Runtime.RemoteConfig.Variables
{
    public abstract class BaseRemoteVariable<T> : ScriptableObject
    {
        public virtual T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = SetValue(value);
            }
        }

        public virtual T DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        public virtual string Key
        {
            get
            {
                return _key;
            }
        }

        public virtual System.Type Type { get { return typeof(T); } }

        [SerializeField]
        protected T _value = default(T);
        [SerializeField]
        protected T _defaultValue = default(T);
        [SerializeField]
        protected string _key = default(string);

        public virtual T SetValue(T newValue)
        {
            _value = newValue;

            return _value;
        }

        public void SetDefaultValue(T value)
        {
            _defaultValue = value;
        }

        public void SetKey(string key)
        {
            _key = key;
        }

        public static implicit operator T(BaseRemoteVariable<T> variable)
        {
            return variable.Value;
        }

        public override string ToString()
        {
            return _value == null ? "null" : _value.ToString();
        }

        public void OnValidate()
        {
            SetValue(Value);
        }
    }
}