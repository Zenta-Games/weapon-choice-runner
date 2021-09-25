using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenta.Core.Runtime.Config.Configs;
using Zenta.Core.Runtime.Interfaces;

#if Elephant
using ElephantSDK;
#endif



namespace Zenta.Core.Runtime.Managers
{
    public class GameManager : MonoBehaviour
    {
        public delegate void OnLevelLoadedDelegate(Level level);
        public OnLevelLoadedDelegate OnLevelLoaded;

        public delegate void OnStateChangedDelegate(GameState from, GameState to);
        public event OnStateChangedDelegate OnStateChanged;

        private static GameManager _instance;
        public static GameManager Instance
        {
            get { return _instance; }
        }

        private GameState _state;
        public GameState State
        {
            get { return _state; }
        }

        private GameConfig _config;
        public GameConfig Config
        {
            get { return _config; }
        }

        private SceneLoadType _sceneLoadType;
        public SceneLoadType SceneLoadType
        {
            get { return _sceneLoadType; }
        }

        private int _level;
        public int Level
        {
            get { return _level; }
            private set
            {
                _level = value;
                PlayerPrefs.SetInt("level", value);
            }
        }

        [SerializeField] [ReadOnly] private List<Level> builtInLevels;
        [SerializeField] private List<Level> levels;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            OnLevelLoaded += SpawnLevel;

            LoadConfig();
        }

        private void Start()
        {
            Analytics.Analytics.Initialize();

            _level = PlayerPrefs.GetInt("level", 0);

            LoadLevels();

#if Elephant
            ElephantCore.onRemoteConfigLoaded += Initialize;
#else
            Initialize();
#endif
        }

        private void LoadConfig()
        {
            _config = Resources.Load<GameConfig>("GameConfig");
        }

        private void Initialize()
        {
            RemoteConfig.RemoteConfig.Initialize();

            levels = new List<Level>();

            string levelsData = Config.LevelsVariable;

            string[] splitted = levelsData.Split(',');

            for (int i = 0; i < splitted.Length; i++)
            {
                if (!splitted[i].Contains("-"))
                {
                    var parsed = int.TryParse(splitted[i], out int levelIndex);

                    if (parsed)
                    {
                        Level level = builtInLevels.Find(a => a.level == levelIndex);

                        if (level != null)
                        {
                            levels.Add(level);
                        }
                    }
                }
            }

            Debug.Log($"{levels.Count} levels loaded from remote config.");

            if (levels.Count > 0)
            {
                LoadInitialLevel();
            }
            else
            {
                Debug.LogError("No level resource file found");
            }
        }

        public void LoadInitialLevel()
        {
            _sceneLoadType = SceneLoadType.Loaded;

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                LoadLoadingScene();
            }
            else
            {
                FindObjectOfType<LevelManager>().Initialize(null);
            }
            /*else
            {
                var initializables = FindObjectsOfType<MonoBehaviour>().OfType<IInitializable>().ToList();

                for (int i = 0; i < initializables.Count; i++)
                {
                    initializables[i].Initialize(null);
                }
            }*/
        }

        private void SpawnLevel(Level level)
        {
            LevelManager[] levels = FindObjectsOfType<LevelManager>();

            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].gameObject.SetActive(false);
                Destroy(levels[i].gameObject);
            }

            if (OnStateChanged != null)
            {
                foreach (var item in OnStateChanged.GetInvocationList())
                {
                    OnStateChanged -= (OnStateChangedDelegate)item;
                }
            }

            LevelManager manager = Instantiate(level.prefab).GetComponent<LevelManager>();

            manager.Initialize(level);

            for (int i = 0; i < Config.RequiredLevelObjects.Length; i++)
            {
                Instantiate(Config.RequiredLevelObjects[i]);
            }
        }

        public void DispatchInitialize(Level level)
        {
            var initializables = FindObjectsOfType<MonoBehaviour>().OfType<IInitializable>().ToList();

            for (int i = 0; i < initializables.Count; i++)
            {
                initializables[i].Initialize(level);
            }
        }

        public void LoadCurrentLevel()
        {
            _sceneLoadType = SceneLoadType.Reloaded;

            LoadLoadingScene();
        }

        public void LoadNextLevel()
        {
            _sceneLoadType = SceneLoadType.Loaded;

            LoadLoadingScene();
        }

        private void LoadLoadingScene()
        {
            SceneManager.LoadScene(Config.LoadingScene);
        }

        private void LoadLevels()
        {
            builtInLevels = Resources.LoadAll<Level>(Config.LevelsPath).ToList();

            builtInLevels = builtInLevels.OrderBy(a => int.Parse(a.name.Split('_')[1])).ToList();

            for (int i = 0; i < builtInLevels.Count; i++)
            {
                builtInLevels[i].level = (i + 1);
            }
        }

        private bool ChangeState(GameState state)
        {
            if (State == state)
            {
                return false;
            }

            if (Config.GameStateProtection)
            {
                if (!ShouldStateChange(State))
                {
                    return false;
                }
            }

            GameState temp = _state;

            _state = state;

            OnStateChanged?.Invoke(temp, state);

            var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IGameStateListener>().ToArray();

            Debug.Log($"Dispatching state change event to {listeners.Length} objects");

            for (int i = 0; i < listeners.Length; i++)
            {
                listeners[i].OnStateChanged(temp, state);
            }

            return true;
        }

        public void LoadInitialState()
        {
            if (ChangeState(GameState.Initial))
            {

            }
        }

        public void StartLevel()
        {
            if (ChangeState(GameState.Playing))
            {

            }
        }

        public void FailLevel()
        {
            if (ChangeState(GameState.Failed))
            {
                Analytics.Analytics.FailEvent(Level);
            }
        }

        public void CompleteLevel()
        {
            if (ChangeState(GameState.Completed))
            {
                Analytics.Analytics.CompleteEvent(Level);
                IncreaseUserLevel();
            }
        }

        private bool ShouldStateChange(GameState from)
        {
            if ((int)from <= (int)GameState.Playing)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void IncreaseUserLevel()
        {
            Level = _level + 1;
        }

        public void DecreaseUserLevel()
        {
            Level = _level - 1;
        }

        public Level GetLevel()
        {
            int totalLevelCount = levels.Count;

            return levels[(_level % totalLevelCount)];
        }
    }
}