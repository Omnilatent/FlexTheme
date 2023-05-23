using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Omnilatent.FlexTheme.Editor
{
    [CustomEditor(typeof(ThemeAssetCollection))]
    public class ThemeAssetCollectionEditor : UnityEditor.Editor
    {
        private ThemeAssetCollection m_Target;

        void Awake()
        {
            m_Target = (ThemeAssetCollection)target;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Update sprites path"))
            {
                UpdateData();
            }

            base.OnInspectorGUI();
        }

        // [MenuItem("Tools/Update StageObjects Sprites Path")]
        private void UpdateData()
        {
            // Dictionary<string, string> data = new Dictionary<string, string>();
            List<ThemeAssetCollection.SpritePath> data = new List<ThemeAssetCollection.SpritePath>();
            string folderPath = AssetDatabase.GetAssetPath(m_Target.SpriteResourcesFolder);
            string[] GUIs = AssetDatabase.FindAssets("t:sprite", new[] { folderPath });
            for (int i = 0; i < GUIs.Length; i++)
            {
                string filePath = AssetDatabase.GUIDToAssetPath(GUIs[i]);
                string resPath = filePath.Substring(filePath.LastIndexOf("Resources/") + 10);
                int fileExtPos = resPath.LastIndexOf(".");
                if (fileExtPos >= 0)
                    resPath = resPath.Substring(0, fileExtPos);

                string fileName = resPath.Substring(resPath.LastIndexOf("/") + 1);
                data.Add(new ThemeAssetCollection.SpritePath(fileName, resPath));
            }

            m_Target.SpritePaths = data;
            CheckDataDuplicateFilename();
            EditorUtility.SetDirty(m_Target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Sprite Resources path has been updated.");
        }

        void CheckDataDuplicateFilename()
        {
            HashSet<string> filenamesHashSet = new HashSet<string>();
            for (int i = 0; i < m_Target.SpritePaths.Count; i++)
            {
                if (filenamesHashSet.Contains(m_Target.SpritePaths[i].spriteName))
                {
                    Debug.LogError($"There are multiple files with name <color=green>{m_Target.SpritePaths[i].spriteName}</color>");
                }
                else
                {
                    filenamesHashSet.Add(m_Target.SpritePaths[i].spriteName);
                }
            }
        }
    }
}