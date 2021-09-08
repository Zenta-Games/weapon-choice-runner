using UnityEngine;
using Zenta.Core.Runtime.Managers;

namespace Zenta.Core.Runtime
{
    public class Initializer
    {
        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            GameManager manager = new GameObject("GameManager").AddComponent<GameManager>();

            Debug.Log("GameManager Initialized");

            manager.OnStateChanged += OnGameStateChanged;
        }

        private static void OnGameStateChanged(GameState from, GameState to)
        {
            Debug.Log($"GameState changed from {from} to {to}");
        }
    }
}