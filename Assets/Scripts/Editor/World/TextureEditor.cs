using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureEditor : EditorWindow
{
    Texture2D tex = null;
    ProceduralTexture pt = new ProceduralTexture();

    string name = "";


    [MenuItem("Window/Texture Editor")]
    static void init()
    {
        TextureEditor window = (TextureEditor)EditorWindow.GetWindow(typeof(TextureEditor));
        window.Show();
      //window.linemat = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Line.mat");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Test"))
        {
            tex = pt.GetTexture();
        }
        name = EditorGUILayout.TextField(name);
        if (GUILayout.Button("Save"))
        {
            byte[] data = tex.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/Graphics/Textures/" + name + ".png", data);
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add")) pt.colors.Add(new ProcColor());
        pt.frequency = EditorGUILayout.Slider(pt.frequency, 0, 10);
        GUILayout.EndHorizontal();
        foreach (ProcColor p in pt.colors)
            DrawColor(p);

        GUI.DrawTexture(new Rect(Vector2.zero-Vector2.down*400, Vector2.one*128), tex);
        
    }

    private void DrawColor(ProcColor p)
    {
        GUILayout.BeginHorizontal();
        p.strength = EditorGUILayout.Slider(p.strength, 0, 1);
        p.color = EditorGUILayout.ColorField(p.color);
        p.complement = EditorGUILayout.ColorField(p.complement);
        if (GUILayout.Button("Generate"))
            p.noise.Randomize();
        if (GUILayout.Button("Delete"))
            pt.colors.Remove(p);
        GUILayout.EndHorizontal();
        p.noise.grain = GUILayout.HorizontalSlider(p.noise.grain, 0, 0.3f);
    }
}
