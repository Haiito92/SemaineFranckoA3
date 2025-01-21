using Localization.Runtime;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(LanguageManager))]
public class LanguageManagerEditor : Editor
{
    LanguageManager Source => (LanguageManager)target;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        EditorUtility.SetDirty(target);

        Source.LANGUAGEProperty = (LANGUAGES_STATE)EditorGUILayout.EnumPopup(new GUIContent("Language : ", ""), Source.LANGUAGEProperty);

        Source._transalationAsset = (TextAsset)EditorGUILayout.ObjectField(new GUIContent("CSV File :",""), Source._transalationAsset, typeof(TextAsset), true);

        ShowTextListPanel();

        if (GUILayout.Button("Fetch texts from scene"))
            Source.AddTextToList();

        if (GUILayout.Button("Clear list"))
            Source.ClearTextList();            
    }

    private void ShowTextListPanel()
    {
        GUIStyle styleWindow = new("window")
        {
            margin = new RectOffset(),
            padding = new RectOffset(),
        };


        EditorGUILayout.BeginVertical(styleWindow);

        DrawHeaderTextList();

        for (int i = 0; i < Source.TextList.Count; i++)
        {
            DrawContentTextList(i);
        }

        EditorGUILayout.EndVertical();

        void DrawHeaderTextList()
        {

            GUIStyle titleStyle = new GUIStyle("MiniBoldLabel");
            titleStyle.fontSize = 15;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.LabelField("Text List", titleStyle);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scene name", EditorStyles.centeredGreyMiniLabel, GUILayout.MinWidth(styleWindow.fixedWidth / 3));
            EditorGUILayout.LabelField("Component", EditorStyles.centeredGreyMiniLabel, GUILayout.MinWidth(styleWindow.fixedWidth / 3));
            EditorGUILayout.LabelField("CSV Keys", EditorStyles.centeredGreyMiniLabel, GUILayout.MinWidth(styleWindow.fixedWidth / 3));
            EditorGUILayout.EndHorizontal();
        }

        void DrawContentTextList(int itemIndex)
        {
            var ld = Source.TextList[itemIndex];

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            EditorGUILayout.TextField("", ld.TextComponent.name, GUILayout.MinWidth(styleWindow.fixedWidth / 3));
            GUI.enabled = true;
            ld.TextComponent = (MaskableGraphic)EditorGUILayout.ObjectField(new GUIContent("", ""), ld.TextComponent, typeof(MaskableGraphic), false, GUILayout.MinWidth(styleWindow.fixedWidth / 3));
            ld.Key = EditorGUILayout.TextField("", Source.TextList[itemIndex].Key, GUILayout.MinWidth(styleWindow.fixedWidth / 3));
            EditorGUILayout.EndHorizontal();

            Source.TextList[itemIndex] = ld;
            EditorGUILayout.Space();
        }
    }
}
