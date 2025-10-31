//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine;


//[CustomEditor(typeof(ItemDataBase))]
//public class ItemContainerEditor : Editor
//{
//    private ReorderableList list;

//    private void OnEnable()
//    {
//        var container = (ItemDataBase)target;

//        list = new ReorderableList(serializedObject,
//            serializedObject.FindProperty("datas"),
//            true, true, true, true);

//        list.drawHeaderCallback = (Rect rect) =>
//        {
//            EditorGUI.LabelField(rect, "Items");
//        };

//        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
//        {
//            var element = list.serializedProperty.GetArrayElementAtIndex(index);
//            EditorGUI.PropertyField(rect, element, true);
//        };

//        list.onAddCallback = (ReorderableList l) =>
//        {
//            var category = container.category;
//            Item newItem = null;

//            switch (category)
//            {
//                case CATEGORY.Spells:
//                    newItem = new Spell();
//                    break;
//                case CATEGORY.Modifiers:
//                    newItem = new Modifiers();
//                    break;
//                default:
//                    newItem = new Item();
//                    break;
//            }

//            Undo.RecordObject(container, "Add Item");
//            container.datas.Add(newItem);
//            EditorUtility.SetDirty(container);
//        };
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        EditorGUILayout.PropertyField(serializedObject.FindProperty("category"));
//        list.DoLayoutList();

//        serializedObject.ApplyModifiedProperties();
//    }
//}
//#endif
