using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoiseBox))]
public class NoiseBoxEditor : Editor {
    int precision = 5;
    public override void OnInspectorGUI()
    {
        NoiseBox t = target as NoiseBox;

        if (GUILayout.Button("Add"))
        {
            t.noises.Add(new NoiseBox.Noise(0,0));
        }

        t.overallStrength = EditorGUILayout.FloatField(t.overallStrength);
        EditorGUILayout.BeginHorizontal();


        precision = EditorGUILayout.IntField(precision);
        if (GUILayout.Button("Randomize"))
        {
            t.noises = new List<NoiseBox.Noise>();
            t.Randomize(precision);
        }

        EditorGUILayout.EndHorizontal();
        for (int i = t.noises.Count - 1; i >= 0 ; i--)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete"))
            {
                t.noises.RemoveAt(i);
                continue;
            }
            DrawNoise(t.noises[i]);
            EditorGUILayout.EndHorizontal();
        }

    }

    private void DrawNoise(NoiseBox.Noise n)
    {
        EditorGUILayout.BeginVertical();
        n.frequency = EditorGUILayout.FloatField("Frequency:",n.frequency);
        n.strength = EditorGUILayout.Slider("strength:", n.strength, 0, 1);
        EditorGUILayout.EndVertical();
    }
}
