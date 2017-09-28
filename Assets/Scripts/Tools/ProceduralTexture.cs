using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProceduralTexture : ScriptableObject{

    public List<ProcColor> colors = new List<ProcColor>();
    public Vector3 grain = new Vector3();
    public float frequency = 1;
    public void Randomize()
    {
        int n = Random.Range(2, 3);
        for (int i = 0; i < 1; i++)
            colors.Add(new ProcColor());
        foreach (ProcColor c in colors)
            c.Randomize();
        grain.x = Random.Range(0.2f, 0.4f);
        grain.y = Random.Range(0.2f, 0.4f);
        grain.z = Random.Range(0.2f, 0.4f);
    }

    public Color GetPixel(Vector3 position)
    {
        Color r = new Color();
        foreach (ProcColor c in colors)
            r += c.Get(position);
        return r/colors.Count;
    }

    public Texture2D GetTexture()
    {
        Texture2D t = new Texture2D(128, 128);
        for(int i = 0; i < 128; i++)
        {
            for (int j = 0; j < 128; j++)
            {
                t.SetPixel(i, j, GetPixel(new Vector3((i/ 128f)*frequency, (j/ 128f)*frequency)));
            }
        }
        t.Apply();
        return t;
    }
}
