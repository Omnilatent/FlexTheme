using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnilatent.FlexTheme
{
    [System.Serializable]
    public class GameObjectThemeMatcher
    {
        [SerializeField] ThemeAssetCollection theme;
        //[SerializeField] GameObject themeGameObject;
        [SerializeField] List<GameObject> themeGameObjects;

        public ThemeAssetCollection Theme { get => theme; set => theme = value; }
        //public GameObject ThemeGameObject { get => themeGameObject; set => themeGameObject = value; }
        public List<GameObject> ThemeGameObjects { get => themeGameObjects; set => themeGameObjects = value; }
    }

    [UnityEngine.Scripting.APIUpdating.MovedFrom(false, "Omnilatent.FlexTheme", null, "GameObjectThemeMatcher")]
    [System.Serializable]
    public class GameObjectThemeProperty
    {
        [SerializeField] ThemeAssetCollection theme;
        //[SerializeField] GameObject themeGameObject;
        [SerializeField] List<GameObject> themeGameObjects;

        public ThemeAssetCollection Theme { get => theme; set => theme = value; }
        //public GameObject ThemeGameObject { get => themeGameObject; set => themeGameObject = value; }
        public List<GameObject> ThemeGameObjects { get => themeGameObjects; set => themeGameObjects = value; }
    }

    public class ThemedObject : MonoBehaviour
    {
        [SerializeField] List<GameObjectThemeProperty> themeObjectsData;

        // Start is called before the first frame update
        void Start()
        {
            bool hasCurrentThemeData = false;
            GameObjectThemeProperty defaultThemeObjectData = null;
            //var defaultThemeObjectData = themeObjectsData.Find(x => x.Equals(ThemeManager.DefaultTheme));
            for (int i = 0; i < themeObjectsData.Count; i++)
            {
                if (themeObjectsData[i].Theme == ThemeManager.DefaultTheme)
                {
                    defaultThemeObjectData = themeObjectsData[i];
                    continue; //default theme object will be toggled after checking for current theme object
                }

                if (themeObjectsData[i].Theme != ThemeManager.CurrentTheme)
                {
                    ToggleThemeGameObject(themeObjectsData[i], false);
                }
                else
                {
                    ToggleThemeGameObject(themeObjectsData[i], true);
                    hasCurrentThemeData = true;
                }
            }

            //disable default object if this object has data for the current theme and vice versa
            if (defaultThemeObjectData != null)
                ToggleThemeGameObject(defaultThemeObjectData, !hasCurrentThemeData);
        }

        private void ToggleThemeGameObject(GameObjectThemeProperty gameObjectThemeMatcher, bool active)
        {
            foreach (var item in gameObjectThemeMatcher.ThemeGameObjects)
            {
                item.SetActive(active);
            }
        }

        /*private void Reset()
        {
            List<GameObjectThemeMatcher> themeObjectsData = new List<GameObjectThemeMatcher>()
            {
                new GameObjectThemeMatcher(){ Theme = ThemeManager.DefaultTheme, ThemeGameObjects = new List<GameObject>(){ gameObject } }
            };
        }*/
    }

}