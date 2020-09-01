using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CatLike
{
    public static class EditorList
    {

        public static void Show(SerializedProperty list, EditorListOption options = EditorListOption.Default)
        {
            bool showListLabel = (options & EditorListOption.ListLabel) != 0;
            bool showListSize = (options & EditorListOption.ListSize) != 0;

            if (showListLabel)
            {
                EditorGUILayout.PropertyField(list);

                EditorGUI.indentLevel += 1;
            }

            if (list.isExpanded)
            {
                if (showListSize)
                {
                    EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
                }

                ShowElements(list, options);
            }

            if (showListLabel)
            {
                EditorGUI.indentLevel -= 1;
            }

        }

        private static void ShowElements(SerializedProperty list, EditorListOption options)
        {
            bool showElementLabels = (options & EditorListOption.ElementLabels) != 0;

            for (int i = 0; i < list.arraySize; i++)
            {
                if (showElementLabels)
                {
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
                }
                else
                {
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
                }
            }
        }
    }

}