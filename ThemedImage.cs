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
    public class ImageThemeProperty
    {
        [SerializeField] ThemeAssetCollection theme;
        [SerializeField] Vector2 anchoredPosition;

        public ThemeAssetCollection Theme { get => theme; set => theme = value; }
        public Vector2 AnchoredPosition { get => anchoredPosition; set => anchoredPosition = value; }

        public ImageThemeChangeEvent imageThemeChangeEvent;
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
        //[SerializeField] ThemeAssetCollection debugCurrentTheme; //for debug

        RectTransform GetRect() => transform as RectTransform;

        // Start is called before the first frame update
        void Start()
        {
            OnThemeChanged();
        }

        public override void OnThemeChanged()
        {
            base.OnThemeChanged();
            string imageFileName = m_Image.sprite.name;

            //debugCurrentTheme = ThemeManager.CurrentTheme;
            var spr = ThemeManager.CurrentTheme.GetImage(imageFileName);
            if (spr != null)
            {
                m_Image.sprite = spr;
                if (setNativeSizeOnChange)
                {
                    m_Image.SetNativeSize();
                }
            }

            var customProperty = customImageProperties.Find(x => x.Theme == ThemeManager.CurrentTheme);
            if (customProperty != null)
            {
                GetRect().anchoredPosition = customProperty.AnchoredPosition;
                customProperty.InvokeEvent();
            }
        }

        // Update is called once per frame
        void Reset()
        {
            m_Image = GetComponent<Image>();
        }
    }
}