using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Omnilatent.FlexTheme.Editor
{

    public class MenuUtils : EditorWindow
    {
        static ThemeAssetCollection theme;
        public const string menuPath = "Tools/Flex Theme/";
        const string menuPathShowWindow = menuPath + "Switch Theme (debug)";
        const string menuPathImportExtra = menuPath + "Import Extra Package";
        static bool savePreference;

        static void Init()
        {
            theme = ThemeManager.CurrentTheme;
        }

        static void Unload()
        {
            theme = null;
        }

        [MenuItem(menuPathShowWindow)]
        public static void ShowWindow()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            MenuUtils window = GetWindow();

            window.minSize = new Vector2(400, 200);
            window.maxSize = new Vector2(400, 200);

            window.ShowUtility();
            if (Application.isPlaying)
                Init();
        }

        static MenuUtils GetWindow() { return (MenuUtils)EditorWindow.GetWindow(typeof(MenuUtils), false, title: "Flex Theme Switcher"); }

        private static void OnPlayModeStateChanged(PlayModeStateChange mode)
        {
            switch (mode)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    Debug.Log($"entered play {GetWindow()}");
                    if (GetWindow() != null)
                    {
                        Init();
                    }
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    if (GetWindow() != null)
                    {
                        Unload();
                    }
                    break;
            }
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.LabelField("Enter Play Mode to use this feature.");
                return;
            }
            savePreference = EditorGUILayout.Toggle("Save Preference", savePreference);
            theme = EditorGUILayout.ObjectField(theme, typeof(ThemeAssetCollection), true) as ThemeAssetCollection;
            if (theme != null && ThemeManager.CurrentTheme != theme)
            {
                SetTheme();
            }
            if (GUILayout.Button("Set manually"))
            {
                SetTheme();
            }
        }

        void SetTheme()
        {
            Debug.Log("[Debug] Updating all Themed component with new theme.");
            ThemeManager.SetTheme(theme.name, true);
            if (savePreference) { ThemeManager.SavePreference(); }
        }

        [MenuItem(menuPathImportExtra, priority = 0)]
        public static void ImportExtra()
        {
            string path = GetPackagePath("Assets/Omnilatent/FlexTheme/FlexThemeExtra.unitypackage", "FlexThemeExtra");
            AssetDatabase.ImportPackage(path, true);
        }

        static string GetPackagePath(string path, string filename)
        {
            if (!File.Exists($"{Application.dataPath}/../{path}"))
            {
                Debug.Log($"{filename} not found at {path}, attempting to search whole project for {filename}");
                string[] guids = AssetDatabase.FindAssets($"{filename} l:package");
                if (guids.Length > 0)
                {
                    path = AssetDatabase.GUIDToAssetPath(guids[0]);
                }
                else
                {
                    Debug.LogError($"{filename} not found at {Application.dataPath}/../{path}");
                    return null;
                }
            }
            return path;
        }
    }
}
