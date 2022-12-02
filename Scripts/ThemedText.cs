using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Omnilatent.FlexTheme
{
    [System.Serializable]
    public class TextThemeProperty : UIThemeProperty
    {
        [SerializeField] private Material material;
        [SerializeField] private Color color = Color.black;

        public Material Material { get => material; set => material = value; }
        public Color Color { get => color; set => color = value; }
    }

    public class ThemedText : ThemedItemBase
    {
        [SerializeField] protected TMP_Text m_Text;
        [SerializeField] protected List<TextThemeProperty> customProperties;
        RectTransform GetRect() => transform as RectTransform;

        public override void OnThemeChanged()
        {
            base.OnThemeChanged();
            var customProperty = customProperties.Find(x => x.Theme == ThemeManager.CurrentTheme);
            if (customProperty != null)
            {
                GetRect().anchoredPosition = customProperty.AnchoredPosition;
                m_Text.color = customProperty.Color;
                if (customProperty.Material != null)
                {
                    m_Text.fontMaterial = customProperty.Material;
                }
            }
        }

        public override void CopyCurrentValue(ThemeAssetCollection targetTheme)
        {
            base.CopyCurrentValue(targetTheme);
            TextThemeProperty targetProperty;
            if (customProperties == null) { customProperties = new List<TextThemeProperty>(); }
            targetProperty = customProperties.Find(property => property.Theme == targetTheme);
            if (targetProperty == null)
            {
                targetProperty = new TextThemeProperty()
                {
                    Theme = targetTheme,
                };
                customProperties.Add(targetProperty);
            }
            targetProperty.AnchoredPosition = m_Text.rectTransform.anchoredPosition;
            targetProperty.Color = m_Text.color;
        }

        private void Reset()
        {
            m_Text = GetComponent<TMP_Text>();
            /*if (m_Text != null)
            {
                customProperties = new List<TextThemeProperty>();
                customProperties.Add(new TextThemeProperty()
                {
                    Theme = ThemeManager.DefaultTheme,
                    AnchoredPosition = m_Text.rectTransform.anchoredPosition,
                    Color = m_Text.color
                });
            }*/
        }
    }
}