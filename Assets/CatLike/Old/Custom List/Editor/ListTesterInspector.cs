using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CatLike
{
    //[CustomEditor(typeof(ListTester)), CanEditMultipleObjects]
    public class ListTesterInspector : Editor
    {
        //private void OnEnable()
        //{
        //    EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
        //}

        //private void OnDisable()
        //{
        //    EditorApplication.contextualPropertyMenu -= OnPropertyContextMenu;
        //}

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorList.Show(serializedObject.FindProperty("intergers"));
            EditorList.Show(serializedObject.FindProperty("Vector3s"));
            EditorList.Show(serializedObject.FindProperty("colorPoints"), EditorListOption.NoElementLabels);
            EditorList.Show(serializedObject.FindProperty("objects"));


            serializedObject.ApplyModifiedProperties();
        }


        //private void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
        //{
        //    menu.AddItem(new GUIContent("我的右键菜单选项"), false, () => { Debug.Log("我的右键菜单选项 Done."); });
        //}

        //[MenuItem("CONTEXT/ListTester/我的菜单栏扩展")]
        //static void DoubleMass(MenuCommand command)
        //{
        //    ListTester listTester = (ListTester)command.context;

        //    Debug.Log("listTester == " + listTester == null ? "null" : "no null");

        //    System.Reflection.MemberInfo memberInfo = typeof(ListTesterInspector);

        //    var attributes = memberInfo.GetCustomAttributes(typeof(MenuItem), false);

        //    foreach (var item in attributes)
        //    {
        //        Debug.Log(item);
        //    }

        //}
    }

}