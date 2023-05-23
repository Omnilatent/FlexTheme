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
        [Tooltip("Path to sprite atlas(es) for this theme. If assigned, will ignore spriteResourcesFolder")] [SerializeField]
        string[] spriteAtlasPaths;

        [Tooltip("Parent folder containing sprites of this theme in Resources folder")] [SerializeField]
        public Object SpriteResourcesFolder;

        [SerializeField] public List<SpritePath> SpritePaths;
        private Dictionary<string, string> spriteFilenameToPath; //converted from SpritePaths

        private Dictionary<string, string> SpriteFilenameToPath
        {
            get
            {
                if (spriteFilenameToPath == null && SpritePaths.Count > 0)
                {
                    spriteFilenameToPath = new Dictionary<string, string>();
                    for (int i = 0; i < SpritePaths.Count; i++)
                    {
                        if (spriteFilenameToPath.ContainsKey(SpritePaths[i].spriteName))
                        {
                            Debug.LogWarning($"There are files with duplicate names at {SpritePaths[i].path}, this will cause wrong sprite to be loaded.");
                            continue;
                        }

                        spriteFilenameToPath.Add(SpritePaths[i].spriteName, SpritePaths[i].path);
                    }
                }

                return spriteFilenameToPath;
            }
        }

        [System.Serializable]
        public class SpritePath
        {
            public string spriteName;
            public string path;

            public SpritePath(string spriteName, string path)
            {
                this.spriteName = spriteName;
                this.path = path;
            }
        }

        public static readonly string spriteAtlasPath = "ThemedSpriteAtlases";

        public Sprite GetImage(string name)
        {
            for (int i = 0; i < spriteAtlasPaths.Length; i++)
            {
                var spr = GetSpriteAtlas(spriteAtlasPaths[i]).GetSprite(name);
                if (spr != null) return spr;
            }

            if (SpriteFilenameToPath != null && SpriteFilenameToPath.TryGetValue(name, out var path))
            {
                return Resources.Load<Sprite>(path);
            }

            return null;
        }

        public SpriteAtlas GetSpriteAtlas(string atlasName)
        {
            return ThemeManager.Instance.LoadObject(Path.Combine(spriteAtlasPath, this.name, atlasName)) as SpriteAtlas;
        }

        public string GetSceneName(string defaultSceneName)
        {
            return $"{defaultSceneName}_{this.name}";
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