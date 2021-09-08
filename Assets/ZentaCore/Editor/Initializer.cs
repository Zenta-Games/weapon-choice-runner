using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Zenta.Core.Runtime.Config.Configs;
using Zenta.Core.Runtime.Managers;
using System.Linq;
using Zenta.Core.Runtime.RemoteConfig.Variables;
using Zenta.Core.Runtime;

namespace Zenta.Core.Editor
{
    [InitializeOnLoad]
    public class Initializer
    {
        [MenuItem("Window/Zenta/Initialize")]
        public static void Initialize()
        {
            AddScenes();
            CheckSettings();
            AddLevelsRemote();
        }

        static Initializer()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;

            ZentaEditor.Initialize();
        }

        static void HierarchyWindowItemCallback(int pID, Rect pRect)
        {
            if (Event.current.type == EventType.DragExited)
            {
                DragAndDrop.AcceptDrag();

                if (DragAndDrop.objectReferences.Length != 1)
                {
                    return;
                }

                var objectRef = DragAndDrop.objectReferences[0];

                if (objectRef is MonoScript)
                {
                    var gameObject = new GameObject(objectRef.name);

                    System.Type mType = System.Type.GetType(objectRef.name + ",Assembly-CSharp");

                    gameObject.AddComponent(mType);
                }

                Event.current.Use();
            }
        }

        private static void AddScenes()
        {
            SceneAsset splash = Resources.Load<SceneAsset>("Scenes/Splash");
            SceneAsset loading = Resources.Load<SceneAsset>("Scenes/Loading");

            string path = "Assets/Scenes";

            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder("Assets", "Scenes");
            }

            SceneAsset existingSplash = (SceneAsset)AssetDatabase.LoadMainAssetAtPath(path + "/Splash.unity");
            SceneAsset existingLoading = (SceneAsset)AssetDatabase.LoadMainAssetAtPath(path + "/Loading.unity");

#pragma warning disable IDE0090 // Use 'new(...)'
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
#pragma warning restore IDE0090 // Use 'new(...)'

            scenes = (from scene in scenes where scene.enabled select scene).ToList();

            EditorBuildSettings.scenes = scenes.ToArray();

            if (existingSplash == null && existingLoading == null)
            {
                AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(splash), path + "/Splash.unity");
                AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(loading), path + "/Loading.unity");

                AddScenesToBuildSettings(new string[] { path + "/Loading.unity", path + "/Splash.unity" });

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private static void AddScenesToBuildSettings(string[] paths)
        {
#pragma warning disable IDE0090 // Use 'new(...)'
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
#pragma warning restore IDE0090 // Use 'new(...)'

            foreach (EditorBuildSettingsScene scene in scenes)
            {
                if (IsSceneContains(paths, scene.path))
                    return;
            }

            for (int i = 0; i < paths.Length; i++)
            {
#pragma warning disable IDE0090 // Use 'new(...)'
                EditorBuildSettingsScene newScene = new EditorBuildSettingsScene
#pragma warning restore IDE0090 // Use 'new(...)'
                {
                    path = paths[i],
                    enabled = true
                };

                scenes = scenes.Prepend(newScene).ToList();
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static bool IsSceneContains(string[] scenes, string scene)
        {
            for (int i = 0; i < scenes.Length; i++)
            {
                if (scenes[i] == scene)
                {
                    return true;
                }
            }

            return false;
        }

#pragma warning disable IDE0051 // Remove unused private members
        private static bool IsBuildSettingsContainsScene(string name)
#pragma warning restore IDE0051 // Remove unused private members
        {
#pragma warning disable IDE0090 // Use 'new(...)'
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
#pragma warning restore IDE0090 // Use 'new(...)'

            foreach (var scene in scenes)
            {
                if (GetSceneName(scene.path) == name)
                {
                    return true;
                }
            }

            return false;
        }

        private static string GetSceneName(string path)
        {
            string[] splitted = path.Split('/');

            return splitted[splitted.Length - 1].Split('.')[0];
        }

        private static void AddLevelsRemote()
        {
            /*var levels = AssetDatabase.LoadAssetAtPath<StringRemoteVariable>("Assets/Resources/RemoteConfig/Levels.asset");

            if (levels == null)
            {
                levels = ScriptableObject.CreateInstance<StringRemoteVariable>();

                levels.SetKey("levels");
                levels.SetDefaultValue("1,2,3,4,5,6,7,8,9,10");

                string path = "Assets/Resources/RemoteConfig";

                if (!AssetDatabase.IsValidFolder(path))
                {
                    AssetDatabase.CreateFolder("Assets/Resources", "RemoteConfig");
                }

                string levelsPath = AssetDatabase.GenerateUniqueAssetPath(path + "/Levels.asset");

                if (!string.IsNullOrEmpty(levelsPath))
                {
                    AssetDatabase.CreateAsset(levels, levelsPath);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    Debug.Log("Level remote config created");
                }
            }

            var config = Resources.Load<GameConfig>("GameConfig");

            if (config != null)
            {
                config.LevelsVariable = levels;
            }*/
        }

        private static void CheckSettings()
        {
            string sourcePath = "Assets/ZentaCore/Runtime/Resources/GameUI.prefab";

            if (!AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath))
            {
                return;
            }

            var config = Resources.Load<GameConfig>("GameConfig");

            if (config != null)
            {
                string uiPath = "Assets/Resources/GameUI.prefab";

                GameObject ui = AssetDatabase.LoadAssetAtPath<GameObject>(uiPath);

                if (ui == null)
                {
                    if (AssetDatabase.CopyAsset(sourcePath, uiPath))
                    {
                        AssetDatabase.ForceReserializeAssets(new List<string>() { uiPath });

                        if (config)
                        {
                            EditorUtility.SetDirty(config);
                            config.GameUI = AssetDatabase.LoadAssetAtPath<GameObject>(uiPath);
                        }

                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }

                if (!config.LevelsVariable)
                {
                    var levels = Resources.Load<StringRemoteVariable>("RemoteConfig/Levels");

                    if (levels != null)
                    {
                        config.LevelsVariable = levels;

                        AssetDatabase.ForceReserializeAssets(new List<string>() { uiPath });

                        EditorUtility.SetDirty(config);

                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        levels = ScriptableObject.CreateInstance<StringRemoteVariable>();
                        levels.SetKey("levels");
                        levels.SetDefaultValue("1,2,3,4,5,6,7,8,9,10");

                        string path = "Assets/Resources/RemoteConfig";

                        if (!AssetDatabase.IsValidFolder(path))
                        {
                            AssetDatabase.CreateFolder("Assets/Resources", "RemoteConfig");
                        }

                        string levelsPath = AssetDatabase.GenerateUniqueAssetPath(path + "/Levels.asset");

                        AssetDatabase.CreateAsset(levels, levelsPath);

                        AssetDatabase.ForceReserializeAssets(new List<string>() { uiPath });

                        EditorUtility.SetDirty(config);

                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
            }

            /*
            var config = AssetDatabase.LoadAssetAtPath<GameConfig>("Assets/Resources/GameConfig.asset");

            if (config == null)
            {
                config = ScriptableObject.CreateInstance<GameConfig>();

                config.LevelsPath = "Levels";
                config.LoadingScene = 1;
                config.LevelLabelPrefix = "Level ";

                string path = "Assets/Resources";

                if (!AssetDatabase.IsValidFolder(path))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }

                string configPath = AssetDatabase.GenerateUniqueAssetPath(path + "/GameConfig.asset");
                string uiPath = AssetDatabase.GenerateUniqueAssetPath(path + "/GameUI.prefab");

                if (!string.IsNullOrEmpty(uiPath))
                {
                    AssetDatabase.CopyAsset("Packages/games.zenta.core/Runtime/Resources/GameUI.prefab", uiPath);

                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/GameUI.prefab");

                    config.GameUI = prefab;

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    Debug.Log("GameUI created");
                }

                if (!string.IsNullOrEmpty(configPath))
                {
                    AssetDatabase.CreateAsset(config, configPath);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    Debug.Log("GameConfig created");
                }
            }*/
        }

        [MenuItem("GameObject/Create Zenta Level", priority = 0)]
        public static void CreateLevelMenuItem(MenuCommand menuCommand)
        {
            string name = "Level_";

            Level[] levels = Resources.LoadAll<Level>(Resources.Load<GameConfig>("GameConfig").LevelsPath);

            name += (levels.Length + 1).ToString();

#pragma warning disable IDE0090 // Use 'new(...)'
            GameObject go = new GameObject(name);
#pragma warning restore IDE0090 // Use 'new(...)'
            go.AddComponent<LevelManager>();

            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

            Selection.activeObject = go;
        }
    }
}