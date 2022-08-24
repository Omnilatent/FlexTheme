using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Omnilatent.FlexTheme.Editor
{

    public class MenuUtils : EditorWindow
    {
        static ThemeAssetCollection theme;
        public const string menuPath = "Tools/Flex Theme/";
        const string menuPathShowWindow = menuPath + "Switch Theme (debug)";

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

        static MenuUtils GetWindow() { return (MenuUtils)EditorWindow.GetWindow(typeof(MenuUtils)); }

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
            theme = EditorGUILayout.ObjectField(theme, typeof(ThemeAssetCollection), true) as ThemeAssetCollection;
            if (theme != null && ThemeManager.CurrentTheme != theme)
            {
                Debug.Log("[Debug] Updating all Themed component with new theme.");
                ThemeManager.SetTheme(theme.name, true);
            }
        }
    }
}
