using UnityEngine;
using NaughtyAttributes;
using Zenta.Core.Runtime.Managers;

namespace Zenta.Core.Runtime
{
    [CreateAssetMenu(menuName = "Zenta/Level", fileName = "Level", order = 0)]
    public class Level : ScriptableObject
    {
        [HideInInspector] public int level;
        public GameObject prefab;
        [Scene] public int scene;
        public Sprite preview;

        private void OnValidate()
        {
            if (prefab != null)
            {
                if (prefab.GetComponent<LevelManager>() == null)
                {
                    prefab = null;
                }
            }
        }
    }
}