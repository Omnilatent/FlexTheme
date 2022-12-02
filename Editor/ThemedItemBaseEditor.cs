using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Omnilatent.FlexTheme.Editor
{
    [CustomEditor(typeof(ThemedItemBase))]
    public class ThemedItemBaseEditor : UnityEditor.Editor
    {
        ThemeAssetCollection theme;
        ThemedItemBase m_Target;

        private void Awake()
        {
            m_Target = target as ThemedItemBase;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            theme = EditorGUILayout.ObjectField(new GUIContent("Target theme property", "Copy current object's attribute into the target theme's property"), theme, typeof(ThemeAssetCollection), true) as ThemeAssetCollection;
            if (GUILayout.Button("Copy current value"))
            {
                m_Target.CopyCurrentValue(theme);
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(m_Target);
            }
        }
    }

    [CustomEditor(typeof(ThemedImage))]
    public class ThemedImageEditor : ThemedItemBaseEditor { }

    [CustomEditor(typeof(ThemedText))]
    public class ThemedTextEditor : ThemedItemBaseEditor { }
}