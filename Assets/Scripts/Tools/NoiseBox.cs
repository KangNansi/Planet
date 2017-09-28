using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Noise", menuName = "Noise Box", order = 42)]
public class NoiseBox : ScriptableObject {

    [System.Serializable]
    public class Noise
    {
        public float frequency;
        public float strength;

        public Noise(float f, float s)
        {
            frequency = f;
            strength = s;
        }
    };

    [SerializeField]
    public List<Noise> noises = new List<Noise>();
    public float grain = 0.0f;

    public float overallStrength = 1.0f;

	public void AddNoise(float frequency, float strength)
    {
        noises.Add(new Noise(frequency, strength));
    }

    public float Get(float a, float b)
    {
        float result = 0;
        float totalStrength = 0;

        foreach(Noise n in noises)
        {
            result += n.strength*Mathf.PerlinNoise(a * n.frequency, a * n.frequency);
            totalStrength += n.strength;
        }

        return result / totalStrength;
    }

    public float Get(float a, float b, float c)
    {
        float result = 0;
        float totalStrength = 0;

        foreach (Noise n in noises)
        {
            result += n.strength * Perlin.Noise(a * n.frequency, b * n.frequency, c * n.frequency);
            totalStrength += n.strength;
        }

        return (result / totalStrength)*overallStrength;// + Random.Range(-grain, +grain);
    }

    public float Get(Vector3 p)
    {
        return Get(p.x, p.y, p.z);
    }

    public void Randomize(int precision = 5)
    {
        float start = 1f;
        for (int i = 0; i < precision; i++)
        {
            AddNoise(start, (precision - i) / (float)precision);
            start += Random.Range(2, 6);
        }
        grain = Random.Range(0,0.4f);
    }
}
