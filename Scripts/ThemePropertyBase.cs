using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Omnilatent.FlexTheme
{
    public class UIThemeProperty : ThemePropertyBase
    {
        [SerializeField] protected Vector2 anchoredPosition;
        public Vector2 AnchoredPosition { get => anchoredPosition; set => anchoredPosition = value; }
    }

    public class ThemePropertyBase
    {
        [SerializeField] protected ThemeAssetCollection theme;
        public ThemeAssetCollection Theme { get => theme; set => theme = value; }
    }
}