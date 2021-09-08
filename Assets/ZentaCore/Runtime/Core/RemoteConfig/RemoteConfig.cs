using UnityEngine;
using Zenta.Core.Runtime.RemoteConfig.Variables;
#if Elephant
using ElephantSDK;
#endif

namespace Zenta.Core.Runtime.RemoteConfig
{
    public static class RemoteConfig
    {
        public static void Initialize()
        {
            StringRemoteVariable[] stringVariables = Resources.LoadAll<StringRemoteVariable>("RemoteConfig");
            IntRemoteVariable[] intVariables = Resources.LoadAll<IntRemoteVariable>("RemoteConfig");
            FloatRemoteVariable[] floatVariables = Resources.LoadAll<FloatRemoteVariable>("RemoteConfig");
            BoolRemoteVariable[] boolVariables = Resources.LoadAll<BoolRemoteVariable>("RemoteConfig");

            for (int i = 0; i < stringVariables.Length; i++)
            {
#if Elephant
                stringVariables[i].Value = ElephantSDK.RemoteConfig.GetInstance().Get(stringVariables[i].Key, stringVariables[i].DefaultValue);
#else
                stringVariables[i].Value = stringVariables[i].DefaultValue;
#endif
            }

            for (int i = 0; i < intVariables.Length; i++)
            {
#if Elephant
                intVariables[i].Value = ElephantSDK.RemoteConfig.GetInstance().GetInt(intVariables[i].Key, intVariables[i].DefaultValue);
#else
                intVariables[i].Value = intVariables[i].DefaultValue;
#endif
            }

            for (int i = 0; i < floatVariables.Length; i++)
            {
#if Elephant
                floatVariables[i].Value = ElephantSDK.RemoteConfig.GetInstance().GetFloat(floatVariables[i].Key, floatVariables[i].DefaultValue);
#else
                floatVariables[i].Value = floatVariables[i].DefaultValue;
#endif
            }

            for (int i = 0; i < boolVariables.Length; i++)
            {
#if Elephant
                boolVariables[i].Value = ElephantSDK.RemoteConfig.GetInstance().GetBool(boolVariables[i].Key, boolVariables[i].DefaultValue);
#else
                boolVariables[i].Value = boolVariables[i].DefaultValue;
#endif
            }
        }
    }
}