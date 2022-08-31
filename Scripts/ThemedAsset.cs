using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnilatent.FlexTheme
{
    public class ThemePropertyBase
    {
        [SerializeField] protected ThemeAssetCollection theme;
        public ThemeAssetCollection Theme { get => theme; set => theme = value; }
    }

    [System.Serializable]
    public class ThemedAsset
    {
        [SerializeField] List<AssetThemeProperty> themeProperties;

        public Object GetCurrentThemeAsset()
        {
            var currentThemeProperty = themeProperties.Find(x => x.Equals(ThemeManager.CurrentTheme));
            return currentThemeProperty.Asset;
        }
    }

    [System.Serializable]
    public class AssetThemeProperty : ThemePropertyBase
    {
        [SerializeField] Object asset;

        public Object Asset { get => asset; set => asset = value; }
    }

    public class GenericThemeProperty<T> : ThemePropertyBase
    {
        T data;
        public T Data { get => data; set => data = value; }

        public GenericThemeProperty()
        {
        }

        public GenericThemeProperty(ThemeAssetCollection theme, T data)
        {
            this.Theme = theme;
            this.data = data;
        }
    }
}