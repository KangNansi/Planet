using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BuildingBuilder : EditorWindow {

    public Mesh mesh;
    public Material mat;
    Camera cam = new Camera();
    //Material linemat;

    Vector3 rot = new Vector3();
    Quaternion rotation = Quaternion.identity;

    public Rect rendRect = new Rect(0,0,300,300);

    [MenuItem("Window/Building builder")]
    static void init()
    {
        BuildingBuilder window = (BuildingBuilder)EditorWindow.GetWindow(typeof(BuildingBuilder));
        window.Show();
        //window.linemat = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Line.mat");
    }

    private void OnGUI()
    {
        //GUI
        mesh = (Mesh)EditorGUILayout.ObjectField("Mesh", mesh, typeof(Mesh), false);
        mat = (Material)EditorGUILayout.ObjectField("Material", mat, typeof(Material), false);

        if (GUILayout.Button("Generate"))
        {
            Build();
        }
        if (GUILayout.Button("Save"))
        {
            string path = EditorUtility.SaveFilePanelInProject("Save", "mesh", "asset", "ok");
            AssetDatabase.CreateAsset(mesh, path);
        }
        if (GUILayout.Button("Reset Rotation"))
        {
            rot = Vector3.zero;
        }
        

        

        BeginWindows();
        rendRect = GUI.Window(1, rendRect, Renderer, "Render");
        EndWindows();
        
        //Handles.DrawCamera(Rect.zero, cam);
    }

    private void Build()
    {
        MeshBuilder b = new MeshBuilder();
        b.AddWall(Vector3.zero, new Vector3(1, 0.3f, 7), Quaternion.identity);
        mesh = b.ToMesh();
    }

    private void BuildWall(List<Vector3> vertices, List<int> triangles)
    {
        
    }

    void Renderer(int wid)
    {
        GUI.DragWindow();
        
        switch (Event.current.keyCode)
        {
            case KeyCode.Q:
                rot.y += 10;
                Repaint();
                break;
            case KeyCode.D:
                rot.y -= 10;
                Repaint();
                break;
            case KeyCode.S:
                rot.x += 10;
                Repaint();
                break;
            case KeyCode.Z:
                rot.x -= 10;
                Repaint();
                break;
            default:
                break;
        }

        //DRAWING
        if (Event.current.type == EventType.Repaint)
        {
            Repaint();
        }
    }

    new private void Repaint()
    {
        base.Repaint();
        if (!mesh || !mat) return;
        Debug.Log("Repainting");
        GL.PushMatrix();
        Rect vp = new Rect(rendRect.position.x, this.position.size.y - rendRect.position.y - rendRect.size.y, rendRect.size.x, rendRect.size.y);
        GL.Viewport(vp);
        GL.Clear(true, true, Color.black);
        GL.invertCulling = true;
        GL.LoadProjectionMatrix(Matrix4x4.Perspective(90, 1, 1, 1000));
        Vector3 pos = new Vector3(0, 0, -5);
        mat.SetPass(0);
        Graphics.DrawMeshNow(mesh, Matrix4x4.TRS(new Vector3(0,0,-5), Quaternion.Euler(rot), Vector3.one));
        GL.PushMatrix();
        //linemat.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(new Color(1,0,0));
        GL.Vertex(pos);
        GL.Vertex(pos + Vector3.left);
        GL.End();
        GL.PopMatrix();
        GL.PopMatrix();
    }
}
