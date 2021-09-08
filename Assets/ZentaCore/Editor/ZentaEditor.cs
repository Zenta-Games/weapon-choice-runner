using UnityEditor;
using UnityEngine;

namespace Zenta.Core.Editor
{
    public class ZentaEditor : EditorWindow
    {
        private static bool facebook;
        private static bool elephant;
        private static bool ga;
        private static int level;
        private static string bundleIdentifier;
        private static string companyGame;
        private static string productName;

        [MenuItem("Zenta/Editor")]
        public static void ZentaEditorWindow()
        {
            Initialize();

            EditorWindow window = GetWindow<ZentaEditor>("Zenta Editor");

            window.Focus();
        }

        public static void Initialize()
        {
            facebook = ScriptingDefineUtility.Contains("Facebook");
            elephant = ScriptingDefineUtility.Contains("Elephant");
            ga = ScriptingDefineUtility.Contains("GameAnalytics");
            level = PlayerPrefs.GetInt("level", 0);
            bundleIdentifier = PlayerSettings.GetApplicationIdentifier(EditorUserBuildSettings.selectedBuildTargetGroup);
            companyGame = PlayerSettings.companyName;
            productName = PlayerSettings.productName;
        }

        private void OnGUI()
        {
            DrawLevelEditor();
            DrawAnalytics();
            DrawPlayerSettings();
        }

        private void DrawLevelEditor()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.LabelField("Level Editor", EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Level : ", level.ToString());

            EditorGUILayout.BeginHorizontal(GUI.skin.box);

            if (GUILayout.Button("-"))
            {
                if (level > 0)
                {
                    PlayerPrefs.SetInt("level", level - 1);
                    level = PlayerPrefs.GetInt("level", 0);
                }
            }

            if (GUILayout.Button("+"))
            {
                PlayerPrefs.SetInt("level", level + 1);
                level = PlayerPrefs.GetInt("level", 0);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        private void DrawAnalytics()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.LabelField("Analytics tool settings", EditorStyles.boldLabel);
            
            facebook = EditorGUILayout.Toggle("Facebook: ", facebook);
            elephant = EditorGUILayout.Toggle("Elephant: ", elephant);
            ga = EditorGUILayout.Toggle("GameAnalytics: ", ga);

            if (GUILayout.Button("Update Analytics Settings"))
            {
                UpdateAnalytics();
            }

            EditorGUILayout.EndVertical();
        }

        private void UpdateAnalytics()
        {
            if (facebook)
            {
                ScriptingDefineUtility.Add("Facebook");
            }
            else
            {
                ScriptingDefineUtility.Remove("Facebook");
            }

            if (elephant)
            {
                ScriptingDefineUtility.Add("Elephant");
            }
            else
            {
                ScriptingDefineUtility.Remove("Elephant");
            }

            if (ga)
            {
                ScriptingDefineUtility.Add("GameAnalytics");
            }
            else
            {
                ScriptingDefineUtility.Remove("GameAnalytics");
            }
        }

        private void DrawPlayerSettings()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.LabelField("Player Settings", EditorStyles.boldLabel);

            bundleIdentifier = EditorGUILayout.TextField("Bundle Identifier : ", bundleIdentifier);
            companyGame = EditorGUILayout.TextField("Company Name : ", companyGame);
            productName = EditorGUILayout.TextField("Product Name : ", productName);

            if (GUILayout.Button("Update"))
            {
                PlayerSettings.SetApplicationIdentifier(EditorUserBuildSettings.selectedBuildTargetGroup, bundleIdentifier);
                PlayerSettings.companyName = companyGame;
                PlayerSettings.productName = productName;

                EditorUtility.DisplayDialog("Updated", "Player settings updated", "OK");
            }

            EditorGUILayout.EndVertical();
        }
    }
}