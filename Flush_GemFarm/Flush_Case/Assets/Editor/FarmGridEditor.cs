using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(FarmGrid))]
public class FarmGridEditor : Editor {

    public override void OnInspectorGUI() {
        FarmGrid farmGrid = (FarmGrid)target;
        if(DrawDefaultInspector()) {
            if(farmGrid.autoUpdate) {
                farmGrid.ResetGrid();
            }
        }
        if(GUILayout.Button("Generate")) {
            farmGrid.ResetGrid();
        }
    }




}