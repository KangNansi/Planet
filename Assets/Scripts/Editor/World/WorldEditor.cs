using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(World))]
public class WorldEditor : Editor {


    public override void OnInspectorGUI()
    {
        World world = target as World;
        DrawPropertiesExcluding(serializedObject, "noises");

        //Noises editor
        List<World.Noise> noises = world.noises;
        if (GUILayout.Button("Add Noise"))
            noises.Add(new World.Noise());
        for(int i=0; i < noises.Count; i++)
        {
            noises[i].xOff = EditorGUILayout.FloatField("X", noises[i].xOff);
            noises[i].yOff = EditorGUILayout.FloatField("Y", noises[i].yOff);
            noises[i].frequency = EditorGUILayout.FloatField("Frequency", noises[i].frequency);
            noises[i].strength = EditorGUILayout.Slider("Strength", noises[i].strength, 0f, 1f);
            noises[i].offset = EditorGUILayout.Slider("Offset", noises[i].offset, -1f, 1f);
            noises[i].factor = EditorGUILayout.Slider("Factor", noises[i].factor, 0f, 20f);
            if (GUILayout.Button("Remove"))
            {
                noises.RemoveAt(i);
                i--;
            }
        }

        if (GUILayout.Button("Generate"))
        {
            world.Generate();
        }

        serializedObject.ApplyModifiedProperties();

    }


}
