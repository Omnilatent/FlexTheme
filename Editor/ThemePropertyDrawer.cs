using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Omnilatent.FlexTheme.Editor
{
    /*[CustomPropertyDrawer(typeof(ThemePropertyBase))]
    public class ThemePropertyDrawer : PropertyDrawer
    {
        ThemePropertyBase m_Instance;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //EditorGUI.PropertyField(position, property, label, true);

            if (GUILayout.Button("Copy current"))
            {
                if (m_Instance == null)
                {
                    m_Instance = (ThemePropertyBase)GetPropertyInstance(property);
                }
                m_Instance.CopyCurrentValue();
            }
        }

        public System.Object GetPropertyInstance(SerializedProperty property)
        {

            string path = property.propertyPath;

            System.Object obj = property.serializedObject.targetObject;
            var type = obj.GetType();

            var fieldNames = path.Split('.');
            for (int i = 0; i < fieldNames.Length; i++)
            {
                var info = type.GetField(fieldNames[i]);
                if (info == null)
                    break;

                // Recurse down to the next nested object.
                obj = info.GetValue(obj);
                type = info.FieldType;
            }

            return obj;
        }
    }

    [CustomPropertyDrawer(typeof(ImageThemeProperty))]
    public class ImageThemePropertyDrawer : ThemePropertyDrawer
    {
    }*/
}