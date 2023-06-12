using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatManager))]
public class StatManagerEditor : Editor {
    public override void OnInspectorGUI() {
        StatManager statManager = (StatManager)target;
        if (GUILayout.Button("ResetGemStats")) {
            statManager.ResetData();
        }
    }
}