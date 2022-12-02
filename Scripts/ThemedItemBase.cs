using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnilatent.FlexTheme
{
    public class ThemedItemBase : MonoBehaviour
    {
        protected virtual void Start() { Initialize(); }
        protected virtual void Initialize()
        {
            if (ThemeManager.Instance.ThemedItemsListenToThemeChange)
            {
                ThemeManager.Instance.OnThemeChange += OnThemeChanged;
            }
            OnThemeChanged();
        }
        protected void OnThemeChanged(ThemeAssetCollection theme) { OnThemeChanged(); }

        public virtual void OnThemeChanged() { }
        public virtual void CopyCurrentValue(ThemeAssetCollection targetTheme) { }

        protected virtual void OnDestroy()
        {
            if (ThemeManager.Instance.ThemedItemsListenToThemeChange)
            {
                ThemeManager.Instance.OnThemeChange -= OnThemeChanged;
            }
        }
    }

}