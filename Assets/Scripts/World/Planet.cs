using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Planet : MonoBehaviour {
    public Material material;
    public Material water;

    public GameObject tree;

    public float drawDistance = 1f;
    public int resolution;
    public float radius;
    public int division;
    public int child;

    public int noisePrecision = 10;
    public NoiseBox noise;

    public Color flatc;
    public Color slopec;

    public bool colored;

    ProceduralTexture flat = null;
    ProceduralTexture slope = null;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Simplify(Camera.main.transform.position);
	}

    public void Generate()
    {
        CreateTerrain(Vector3.down, Vector3.forward, Vector3.right);
        CreateTerrain(Vector3.down, Vector3.right, Vector3.back);
        CreateTerrain(Vector3.down, Vector3.back, Vector3.left);
        CreateTerrain(Vector3.down, Vector3.left, Vector3.forward);

        CreateTerrain(Vector3.up, Vector3.right, Vector3.forward);
        CreateTerrain(Vector3.up, Vector3.forward, Vector3.left);
        CreateTerrain(Vector3.up, Vector3.left, Vector3.back);
        CreateTerrain(Vector3.up, Vector3.back, Vector3.right);


        GameObject w = new GameObject();
        MeshFilter filter = w.AddComponent<MeshFilter>();
        MeshRenderer rend = w.AddComponent<MeshRenderer>();
        MeshBuilder mb = new MeshBuilder();
        mb.CreaterSphere(35, radius);
        filter.sharedMesh = mb.ToMesh();
        rend.sharedMaterial = water;
        w.transform.position = transform.position;
        w.transform.parent = transform;


    }

    public void Init()
    {
        //flat = new ProceduralTexture();
        //slope = new ProceduralTexture();
        //flat.Randomize();
        //slope.Randomize();
        //transform.localScale = Vector3.one;
        for(int i=transform.childCount-1; i>=0; i--)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public void Simplify(Vector3 position)
    {
        foreach (Transform t in transform)
        {
            TriangleTerrain terrain = t.GetComponent<TriangleTerrain>();
            if(terrain)
                terrain.Simplify(position);
        }
    }

    public void CreateTerrain(Vector3 a, Vector3 b, Vector3 c)
    {
        GameObject g = new GameObject();
        g.transform.parent = transform;
        g.transform.localPosition = Vector3.zero;
        TriangleTerrain t = g.AddComponent<TriangleTerrain>();
        t.GetComponent<MeshRenderer>().sharedMaterial = material;
        t.planet = this;
        t.Generate(a, b, c, resolution, radius, division, child);
    }
}
