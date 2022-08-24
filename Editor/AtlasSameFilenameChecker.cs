using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
namespace Omnilatent.FlexTheme.Editor
{

    public class AtlasSameFilenameChecker : EditorWindow
    {
        SpriteAtlas atlasToCheck;
        const string showWindowMenuName = MenuUtils.menuPath + "Check Sprite Atlas duplicate filenames";

        [MenuItem(showWindowMenuName)]
        public static void ShowWindow()
        {
            AtlasSameFilenameChecker window = (AtlasSameFilenameChecker)EditorWindow.GetWindow(typeof(AtlasSameFilenameChecker));

            window.minSize = new Vector2(400, 200);
            window.maxSize = new Vector2(400, 200);

            window.ShowUtility();
        }

        private void OnGUI()
        {
            atlasToCheck = EditorGUILayout.ObjectField(atlasToCheck, typeof(SpriteAtlas), true) as SpriteAtlas;
            if (GUILayout.Button("Check for duplicate filenames"))
            {
                Sprite[] sprites = new Sprite[atlasToCheck.spriteCount];
                atlasToCheck.GetSprites(sprites);
                var duplicates = CheckForRepeatedFilename(sprites);
                foreach (var item in duplicates)
                {
                    Debug.Log(item);
                }
            }
        }

        public IEnumerable<Sprite> CheckForRepeatedFilename(Sprite[] array)
        {
            IEnumerable<Sprite> duplicates = array
             .GroupBy(p => p.name)
             .Where(g => g.Count() > 1)
             .SelectMany(g => g);

            return duplicates;
        }
    }

}