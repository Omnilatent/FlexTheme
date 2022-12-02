using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Omnilatent.FlexTheme
{
    /// <summary>
    /// For use when there are sprite with same filename.
    /// </summary>
    [System.Serializable]
    public class ImageThemeProperty : UIThemeProperty
    {
        [Tooltip("If not empty, image will load using this filename instead of the filename in Image component.")]
        [SerializeField] protected string customImageFileName;
        public ImageThemeChangeEvent imageThemeChangeEvent;

        public string CustomImageFileName { get => customImageFileName; set => customImageFileName = value; }

        public void InvokeEvent()
        {
            imageThemeChangeEvent.Invoke(this);
        }
    }

    [System.Serializable] public class ImageThemeChangeEvent : UnityEvent<ImageThemeProperty> { }

    public class ThemedImage : ThemedItemBase
    {
        [SerializeField] bool setNativeSizeOnChange = true;
        [SerializeField] Image m_Image;
        [SerializeField] List<ImageThemeProperty> customImageProperties;
        protected string originalImageFileName;
        //[SerializeField] ThemeAssetCollection debugCurrentTheme; //for debug

        RectTransform GetRect() => transform as RectTransform;

        protected override void Initialize()
        {
            originalImageFileName = m_Image.sprite.name;
            base.Initialize();
        }

        public override void OnThemeChanged()
        {
            base.OnThemeChanged();
            //debugCurrentTheme = ThemeManager.CurrentTheme;

            string imageFileName = originalImageFileName;
            var customProperty = customImageProperties.Find(x => x.Theme == ThemeManager.CurrentTheme);
            if (customProperty != null)
            {
                if (!string.IsNullOrEmpty(customProperty.CustomImageFileName)) { imageFileName = customProperty.CustomImageFileName; }
                LoadSprite(imageFileName);
                GetRect().anchoredPosition = customProperty.AnchoredPosition;
                customProperty.InvokeEvent();
            }
            else
            {
                LoadSprite(imageFileName);
            }
        }

        protected void LoadSprite(string filename)
        {
            var spr = ThemeManager.CurrentTheme.GetImage(filename);
            if (spr != null)
            {
                m_Image.sprite = spr;
                if (setNativeSizeOnChange)
                {
                    m_Image.SetNativeSize();
                }
            }
        }

        public override void CopyCurrentValue(ThemeAssetCollection targetTheme)
        {
            base.CopyCurrentValue(targetTheme);

            ImageThemeProperty targetProperty;
            if (customImageProperties == null) { customImageProperties = new List<ImageThemeProperty>(); }
            targetProperty = customImageProperties.Find(property => property.Theme == targetTheme);
            if (targetProperty == null)
            {
                targetProperty = new ImageThemeProperty()
                {
                    Theme = targetTheme,
                };
                customImageProperties.Add(targetProperty);
            }
            targetProperty.AnchoredPosition = m_Image.rectTransform.anchoredPosition;
        }

        // Update is called once per frame
        void Reset()
        {
            m_Image = GetComponent<Image>();
            /*if (m_Image != null)
            {
                customImageProperties = new List<ImageThemeProperty>();
                customImageProperties.Add(new ImageThemeProperty()
                {
                    Theme = ThemeManager.DefaultTheme,
                    AnchoredPosition = m_Image.rectTransform.anchoredPosition,
                });
            }*/
        }
    }
}