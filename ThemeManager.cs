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
        public const string themeAssetPath = "Themes";
        ThemeAssetCollection currentThemeAsset;
        public static ThemeAssetCollection DefaultTheme { get => Instance.defaultTheme; }
        public static ThemeAssetCollection CurrentTheme { get => Instance.currentThemeAsset; }
        Dictionary<string, Object> loadedObjectsCache = new Dictionary<string, Object>();

        static ThemeManager instance;

        public static ThemeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("ThemeManager");
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
            currentThemeAsset = defaultTheme;
        }

        public static void SetTheme(string themeId = "default", bool updateExistingThemedObject = false)
        {
            Instance.currentThemeAsset = Resources.Load<ThemeAssetCollection>(Path.Combine(themeAssetPath, themeId));
            if (updateExistingThemedObject)
            {
                var themedItems = MonoBehaviour.FindObjectsOfType<ThemedItemBase>();
                foreach (var item in themedItems)
                {
                    item.OnThemeChanged();
                }
            }
        }

        static void SetTheme(ThemeAssetCollection theme)
        {
            Instance.currentThemeAsset = theme;
        }

        public Object LoadObject(string path)
        {
            if (loadedObjectsCache.TryGetValue(path, out var cachedObj))
            {
                return cachedObj;
            }
            var loadObj = Resources.Load<Object>(path);
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