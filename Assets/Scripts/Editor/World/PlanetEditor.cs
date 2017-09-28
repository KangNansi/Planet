using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
[CanEditMultipleObjects]
public class PlanetEditor : Editor {

    private void OnEnable()
    {
        //EditorApplication.update += Update;
    }

    private void OnDestroy()
    {
        EditorApplication.update -= Update;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Planet p = (target as Planet);
        if (GUILayout.Button("Generate"))
        {
            p.Init();
            p.Generate();
        }
        if (GUILayout.Button("Simplify"))
        {
            p.Simplify(SceneView.lastActiveSceneView.camera.transform.position);
        }
    }

    public void Update()
    {
        Planet p = (target as Planet);
        if(p)
            p.Simplify(SceneView.lastActiveSceneView.camera.transform.position);
    }
}
