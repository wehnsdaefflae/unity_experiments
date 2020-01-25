using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (UpdateTexture))]
public class EditorMapGenerator : Editor {

    public override void OnInspectorGUI() {
        UpdateTexture updateTexture = (UpdateTexture)target;

        updateTexture.MyAwake();

        if (DrawDefaultInspector() && updateTexture.autoUpdate) updateTexture.MyUpdate();

        if (GUILayout.Button("Generate")) updateTexture.MyUpdate();

    }
}
