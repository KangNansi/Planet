using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcColor {

    public Color color = new Color();
    public Color complement = new Color();
    public NoiseBox noise;
    public Vector3 offset = new Vector3();

    public float strength = 1.0f;

    public void Randomize()
    {
        noise = ScriptableObject.CreateInstance<NoiseBox>();
        color = Random.ColorHSV();
        complement = Random.ColorHSV();
        noise.Randomize();
        offset = new Vector3(Random.Range(0, 100f), Random.Range(0, 100f), Random.Range(0, 100f));
    }

	public Color Get(Vector3 position)
    {
        float r = noise.Get(position);
        Color res = r * color + (1 - r) * complement;
        res.a = 1;
        return res*strength;
    }
}
