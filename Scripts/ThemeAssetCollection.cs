using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

namespace Omnilatent.FlexTheme
{
    /// <summary>
    /// Represent the theme as a scriptable object. Handle loading & references to assets related to that theme.
    /// </summary>
    [CreateAssetMenu(fileName = "Theme", menuName = "Theme Asset")]
    public class ThemeAssetCollection : ScriptableObject
    {
        [SerializeField] SpriteAtlas[] spriteAtlases;
        [SerializeField] string[] spriteAtlasPaths;
        public static readonly string spriteAtlasPath = "ThemedSpriteAtlases";

        public Sprite GetImage(string name)
        {
            for (int i = 0; i < spriteAtlasPaths.Length; i++)
            {
                var spr = GetSpriteAtlas(spriteAtlasPaths[i]).GetSprite(name);
                if (spr != null) return spr;
            }
            return null;
        }

        public SpriteAtlas GetSpriteAtlas(string atlasName)
        {
            return ThemeManager.Instance.LoadObject(Path.Combine(spriteAtlasPath, this.name, atlasName)) as SpriteAtlas;
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
