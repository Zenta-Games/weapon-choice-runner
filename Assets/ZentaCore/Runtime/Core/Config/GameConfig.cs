using NaughtyAttributes;
using UnityEngine;
using Zenta.Core.Runtime.RemoteConfig.Variables;

namespace Zenta.Core.Runtime.Config.Configs
{
    [CreateAssetMenu(menuName = "Zenta/GameConfig", fileName = "GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        public string LevelsPath;
        public GameObject GameUI;
        [Scene] public int LoadingScene;
        public StringRemoteVariable LevelsVariable;
        public string LevelLabelPrefix;
        [Tooltip("If game fails or completed state should be changed ?")] public bool GameStateProtection;
        [Tooltip("Objects to spawn at each level spawn")] public GameObject[] RequiredLevelObjects;
    }
}