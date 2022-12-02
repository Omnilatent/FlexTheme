using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Omnilatent.FlexTheme
{

    public class ThemeManager : MonoBehaviour
    {
        [SerializeField] List<string> themeAssetIds;
        [SerializeField] ThemeAssetCollection defaultTheme;

        [System.Flags]
        public enum AppEnvironment
        {
            None = 0,
            EditorOnly = 1,
            PlayerOnly = 2,
            Both = EditorOnly | PlayerOnly
        }
        [Tooltip("When Themed Item is initialized, add listener to theme change event.")]
        [SerializeField] private AppEnvironment themedItemsListenEventEnvironment = AppEnvironment.EditorOnly;

        public const string themeAssetPath = "Themes";
        ThemeAssetCollection currentThemeAsset = null;
        public static ThemeAssetCollection DefaultTheme { get => Instance.defaultTheme; }
        public static ThemeAssetCollection CurrentTheme { get => Instance.currentThemeAsset; }
        Dictionary<string, UnityEngine.Object> loadedObjectsCache = new Dictionary<string, UnityEngine.Object>();

        public static string prefKeyCurrentTheme = "FlexTheme_CurrentTheme";

        public bool ThemedItemsListenToThemeChange
        {
            get
            {
                if (themedItemsListenEventEnvironment == AppEnvironment.None) { return false; }
                else if (themedItemsListenEventEnvironment == AppEnvironment.Both) { return true; }
                if (themedItemsListenEventEnvironment.HasFlag(AppEnvironment.EditorOnly) && Application.isEditor)
                {
                    return true;
                }
                if (themedItemsListenEventEnvironment == AppEnvironment.PlayerOnly)
                {
                    return !Application.isEditor;
                }
                return true;
            }
        }
        public Action<ThemeAssetCollection> OnThemeChange;

        static ThemeManager instance;

        public static ThemeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("ThemeManager");
                    if (!Application.isPlaying)
                    {
                        Debug.Log("Getting instance in edit mode will return the prefab.");
                        return prefab.GetComponent<ThemeManager>();
                    }
                    instance = Instantiate(prefab).GetComponent<ThemeManager>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            string preferredTheme = LoadPrefCurrentThemeName();
            if (!string.IsNullOrEmpty(preferredTheme))
            {
                SetTheme(preferredTheme);
            }
            if (currentThemeAsset == null)
            {
                currentThemeAsset = defaultTheme;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="themeId">If null, theme will be set to default theme</param>
        /// <param name="updateExistingThemedObject"></param>
        public static void SetTheme(string themeId, bool updateExistingThemedObject = false)
        {
            if (string.IsNullOrEmpty(themeId))
            {
                themeId = DefaultTheme.name;
            }
            if (Application.isPlaying)
            {
                Instance.currentThemeAsset = GetTheme(themeId);
            }
            if (updateExistingThemedObject)
            {
                var themedItems = MonoBehaviour.FindObjectsOfType<ThemedItemBase>();
                foreach (var item in themedItems)
                {
                    item.OnThemeChanged();
                }
                Instance.OnThemeChange?.Invoke(Instance.currentThemeAsset);
            }
        }

        static void SetTheme(ThemeAssetCollection theme)
        {
            Instance.currentThemeAsset = theme;
        }

        public static ThemeAssetCollection GetTheme(string themeId)
        {
            var theme = Resources.Load<ThemeAssetCollection>(Path.Combine(themeAssetPath, themeId));
            if (theme == null) { Debug.LogError($"No theme exists at path {Path.Combine(themeAssetPath, themeId)}"); }
            return theme;
        }

        public static void SavePreference()
        {
            PlayerPrefs.SetString(prefKeyCurrentTheme, CurrentTheme.name);
            PlayerPrefs.Save();
        }

        public static string LoadPrefCurrentThemeName()
        {
            return PlayerPrefs.GetString(prefKeyCurrentTheme, null);
        }

        public UnityEngine.Object LoadObject(string path)
        {
            if (loadedObjectsCache.TryGetValue(path, out var cachedObj))
            {
                return cachedObj;
            }
            var loadObj = Resources.Load<UnityEngine.Object>(path);
            loadedObjectsCache.Add(path, loadObj);
            Debug.Log($"Loaded {path}: {loadObj}");
            return loadObj;
        }

        public void UnloadCachedObjects()
        {
            foreach (var item in loadedObjectsCache)
            {
                Destroy(item.Value);
            }
            loadedObjectsCache.Clear();
        }
    }

}