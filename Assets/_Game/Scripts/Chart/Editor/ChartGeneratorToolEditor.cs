#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChartGeneratorTool))]
public class ChartGeneratorToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChartGeneratorTool tool = (ChartGeneratorTool)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Generate And Save Chart"))
        {
            tool.GenerateAndSave();
        }
        if (GUILayout.Button("Load Preview Chart"))
        {
            tool.LoadAndPreview();
        }
    }
}
#endif