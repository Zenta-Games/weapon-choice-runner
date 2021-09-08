using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zenta.Core.Runtime.UI.Utils
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _slider;

        [ProgressBar("Value", 1f, EColor.Green)]
        [SerializeField]
        private float _value;
        
        public float Value
        {
            get { return _value; }
        }

        public void UpdateValue(float value)
        {
            _value = value;
            _slider.fillAmount = _value;
        }
    }
}