#if Elephant
using ElephantSDK;
#endif
#if Facebook
using Facebook.Unity;
#endif
#if GameAnalytics
using GameAnalyticsSDK;
#endif

using UnityEngine;

namespace Zenta.Core.Runtime.Analytics
{
    public static class Analytics
    {
        public static void Initialize()
        {
#if Facebook
            if (!FB.IsInitialized)
            {
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                FB.ActivateApp();
            }
#endif

#if GameAnalytics
            GameAnalytics.Initialize();
#endif
        }

#if Facebook
        private static void InitCallback()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private static void OnHideUnity(bool isGameShown)
        {

        }
#endif

        public static void FailEvent(int level)
        {
            Debug.Log($"Analytics sends fail level at : {level + 1}");

#if GameAnalytics
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, (level + 1).ToString());
#endif
#if Elephant
            Elephant.LevelFailed(level + 1);
#endif
        }

        public static void CompleteEvent(int level)
        {
            Debug.Log($"Analytics sends complete level at : {level + 1}");

#if GameAnalytics
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, (level + 1).ToString());
#endif
#if Elephant
            Elephant.LevelCompleted(level + 1);
#endif
        }

        public static void StartEvent(int level)
        {
            Debug.Log($"Analytics sends start level at : {level + 1}");

#if GameAnalytics
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, (level + 1).ToString());
#endif
#if Elephant
            Elephant.LevelStarted(level + 1);
#endif
        }

        public static void CustomEvent(string eventName, int level)
        {
            Debug.Log($"Analytics sends {eventName} level at : {level + 1}");

#if GameAnalytics
            GameAnalytics.NewDesignEvent(eventName, (level + 1));
#endif
#if Elephant
            Elephant.Event(eventName, (level + 1));
#endif
        }
    }
}
