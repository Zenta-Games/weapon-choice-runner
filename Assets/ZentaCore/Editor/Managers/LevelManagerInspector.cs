using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using Zenta.Core.Runtime;
using Zenta.Core.Runtime.Config.Configs;
using Zenta.Core.Runtime.Managers;

namespace Zenta.Core.Editor.Managers
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerInspector : UnityEditor.Editor
    {
        private LevelManager instance;
        private bool isPrefab;
        private bool hasScene;
        private bool prefabScene;

        private void OnEnable()
        {
            Initialize();
        }

        private void OnValidate()
        {
            Initialize();
        }

        private void Initialize()
        {
            instance = target as LevelManager;

            hasScene = instance.gameObject.scene.IsValid();
            prefabScene = PrefabStageUtility.GetCurrentPrefabStage() != null;

            if (PrefabUtility.IsPrefabAssetMissing(instance.gameObject))
            {
                isPrefab = false;
            }
            else
            {
                isPrefab = PrefabUtility.IsAnyPrefabInstanceRoot(instance.gameObject);
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (hasScene && !prefabScene)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                EditorGUILayout.LabelField("Level Editor", EditorStyles.boldLabel);

                if (isPrefab)
                {
                    if (GUILayout.Button("Apply Changes"))
                    {
                        if (EditorUtility.DisplayDialog("Apply changes", "Do you want to apply changes ?", "Yes", "No"))
                        {
                            PrefabUtility.ApplyPrefabInstance(instance.gameObject, InteractionMode.AutomatedAction);
                        }
                    }
                }
                else
                {
                    if (GUILayout.Button("Create Prefab & Resources File"))
                    {
                        if (EditorUtility.DisplayDialog("Create prefab", "Do you want to create prefab and it's resources file ?", "Yes", "No"))
                        {
                            CreatePrefab();
                        }
                    }
                }

                EditorGUILayout.EndVertical();
            }
        }

        private void CreatePrefab()
        {
            Level level = Resources.Load<Level>(Resources.Load<GameConfig>("GameConfig").LevelsPath + "/" + instance.name);

            bool saveable;

            if (level != null)
            {
                saveable = EditorUtility.DisplayDialog("Level already exist", "Do you want to override?", "Yes", "No");
            }
            else
            {
                saveable = true;
            }

            if (saveable)
            {
                if (PrefabUtility.IsPrefabAssetMissing(instance.gameObject))
                {
                    PrefabUtility.UnpackPrefabInstance(instance.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

                    isPrefab = false;
                }

                string prefabsPath = "Assets/Prefabs/Levels";

                if (!AssetDatabase.IsValidFolder(prefabsPath))
                {
                    if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
                    {
                        AssetDatabase.CreateFolder("Assets", "Prefabs");
                    }

                    AssetDatabase.CreateFolder("Assets/Prefabs", "Levels");
                }

                string prefabPath = prefabsPath + "/" + instance.name + ".prefab";

                prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabPath);

                PrefabUtility.SaveAsPrefabAssetAndConnect(instance.gameObject, prefabPath, InteractionMode.AutomatedAction);

                string levelPath = "Assets/Resources/" + Resources.Load<GameConfig>("GameConfig").LevelsPath + "/" + instance.name + ".asset";

                if (level == null)
                {
                    level = ScriptableObject.CreateInstance<Level>();

                    if (!AssetDatabase.IsValidFolder("Assets/Resources/Levels"))
                    {
                        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                        {
                            AssetDatabase.CreateFolder("Assets", "Resources");
                        }

                        AssetDatabase.CreateFolder("Assets/Resources", "Levels");
                    }

                    levelPath = AssetDatabase.GenerateUniqueAssetPath(levelPath);

                    AssetDatabase.CreateAsset(level, levelPath);
                }

                level.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                level.scene = EditorSceneManager.GetActiveScene().buildIndex;

                AssetDatabase.ForceReserializeAssets(new List<string>() { levelPath });

                EditorUtility.SetDirty(level);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("Created", "Level prefab and resource file created!", "OK");

                Initialize();

                Repaint();
            }
        }
    }
}