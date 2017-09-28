using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class TriangleTerrain : MonoBehaviour {
    public int resolution = 0;

    [SerializeField]
    Vector3 _a;
    [SerializeField]
    Vector3 _b;
    [SerializeField]
    Vector3 _c;

    [SerializeField]
    float _radius;
    [SerializeField]
    public int _size;
    [SerializeField]
    public int _counter;

    [SerializeField]
    private int _s2;


    public Planet planet;


    public void Simplify(Vector3 target)
    {
        Vector3 center = GetTriangleCenter();
        Vector3 normalizedTarget = (target - planet.transform.position).normalized;
        float distance = (target - planet.transform.position).magnitude;
        Vector3 v = _b - _a;
        float magnitude = v.magnitude;
        if ((normalizedTarget*(distance/_radius) - center).magnitude > (magnitude * planet.drawDistance))
            foreach (Transform t in transform)
            {
                GetComponent<MeshRenderer>().enabled = true;
                t.gameObject.SetActive(false);
            }
        else
        {
            if (transform.childCount == 0 && _counter>0)
                Divide();
            foreach (Transform t in transform)
            {
                TriangleTerrain tt = t.GetComponent<TriangleTerrain>();
                if (tt)
                {
                    t.gameObject.SetActive(true);
                    GetComponent<MeshRenderer>().enabled = false;
                    tt.Simplify(target);
                }
            }
        }
    }

    Vector3 GetTriangleCenter()
    {
        return (_a + _b + _c)/3f;
    }

    public void Generate()
    {
        foreach(Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
        MeshCollider mc = GetComponent<MeshCollider>();
        mc.sharedMesh = null;
        mc.enabled = false;
        Generate(_a, _b, _c, resolution, _radius, _size);
    }

    public void Init()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<TriangleTerrain>().Init();
            DestroyImmediate(child.gameObject);
        }
    }

    public void Generate(Vector3 a, Vector3 b, Vector3 c, int counter = 0, float radius = 1f, int size = 2, int s2 = 2)
    {
        _a = a;
        _b = b;
        _c = c;
        _radius = radius;
        _size = size;
        _counter = counter;
        _s2 = s2;
        if(counter == 0)
        {
            size *= 2;
        }
        MeshRenderer rend = GetComponent<MeshRenderer>();
        MeshFilter filter = GetComponent<MeshFilter>();
        MeshCollider collider = GetComponent<MeshCollider>();
        MeshBuilder mb = new MeshBuilder();
        mb.AddTriangleMesh(a, c, b, size);
        mb.Normalize();

        Vector3 toCenter = GetTriangleCenter() * radius;

        float[] r = new float[mb.vertices.Count];
        float[] s = new float[mb.vertices.Count];

        float stepAngle = (mb.vertices[0]-mb.vertices[1]).magnitude;
        for (int i = 0; i < mb.vertices.Count; i++) {
            
            Vector3 v = mb.vertices[i];

            

            Vector2 textureCoordinates;
            Vector3 vft = (v.z < 0) ? new Vector3(v.x, v.y, -v.z) : v;
            textureCoordinates.x = 0.5f + (Mathf.Atan2(vft.x, vft.z) / (2f * Mathf.PI));
            textureCoordinates.y = 0.5f - Mathf.Asin(vft.y) / Mathf.PI;
            //textureCoordinates*_radius/((counter+3)*10f));
            
            
            r[i] = planet.noise.Get(v.x, v.y, v.z);

            Quaternion baseRotation = Quaternion.LookRotation(Vector3.forward, v);
            Vector3 ta = Quaternion.LookRotation(_b - _a, v) * Vector3.right;
            Vector3 tb = Quaternion.LookRotation(_c - _b, v) * Vector3.right;
            Vector3 tc = Quaternion.LookRotation(_a - _c, v) * Vector3.right;
            Vector3[] adjacent = new Vector3[6];
            Vector3 vf = v + r[i] / 4f * v;
            vf *= 1000;
            for (int j = 0; j < 6; j++)
            {
                float sa = (j % 2 == 0) ? stepAngle : -stepAngle;
                Vector3 t = (j < 2) ? ta : (j < 4) ? tb : tc;
                adjacent[j] = Quaternion.AngleAxis(sa, t) * v;
                adjacent[j] += planet.noise.Get(adjacent[j]) * adjacent[j] / 4f;
                adjacent[j] *= 1000;
            }
            Vector3 normal = new Vector3();
            for (int j = 0; j < 6; j++)
            {
                normal += Vector3.Cross(adjacent[j] - vf, adjacent[(j + 4) % 6] - vf).normalized;
            }

            Vector3 pdist = transform.parent.position - planet.transform.position;
            mb.vertices[i] = (v + r[i]/4f*v) * radius;

            mb.vertices[i] -= toCenter;
            s[i] = Vector3.Angle(v, normal.normalized);
            //Calculating normals;
            mb.normals.Add(normal.normalized);
            Vector3 color = (v + Vector3.one)/2f;
            mb.uvs.Add(textureCoordinates);// new Vector2(1+r[i], 0));
            mb.colors.Add(new Color(color.x, color.y, color.z, s[i]));
        }

        transform.position = planet.transform.position+toCenter;
        if (counter == 0)
        {
            //placeTrees(mb, s);
            placeDetails(mb, s);
        }
        Mesh m = mb.ToMesh();

        filter.sharedMesh = m;
        if(counter == 0)
        {
            collider.sharedMesh = m;
        }
    }

    private void placeTrees(MeshBuilder mb, float[] s)
    {
        return;
        for(int i = 0; i < mb.vertices.Count; i++)
        {
            if(s[i] < 5)
            {
                GameObject a = GameObject.Instantiate(planet.tree);
                a.transform.parent = transform;
                a.transform.position = transform.position+mb.vertices[i];
                a.transform.rotation = Quaternion.FromToRotation(Vector3.up, a.transform.position - planet.transform.position);
            }
        }
    }

    void placeDetails(MeshBuilder mb, float[] s)
    {
        MeshBuilder details = new MeshBuilder();
        for (int i = 0; i < mb.triangles.Count; i+=3)
        {
            Vector3 v1 = mb.vertices[mb.triangles[i]];
            Vector3 v2 = mb.vertices[mb.triangles[i + 1]];
            Vector3 v3 = mb.vertices[mb.triangles[i + 2]];
            Vector3 center = (v1 + v2 + v3) / 3f;
            Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1);
            float slope = Vector3.Angle(center.normalized, normal);
            details.AddCube(center, Vector3.one);
            details.CreateUVPlane(center, Vector3.zero, Vector3.up, Vector3.up + Vector3.right, Vector3.right);

        }
        GameObject d = new GameObject();
        d.transform.position = transform.position;
        d.transform.parent = transform;
        MeshRenderer rend = d.AddComponent<MeshRenderer>();
        MeshFilter filter = d.AddComponent<MeshFilter>();
        rend.material = planet.material;
        d.transform.localScale = Vector3.one * 5000;
        filter.sharedMesh = details.ToMesh();
    }

    void makeChild(Vector3 a, Vector3 b, Vector3 c)
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        GameObject g = new GameObject();
        TriangleTerrain t = g.AddComponent<TriangleTerrain>();
        g.transform.parent = transform;
        g.transform.localPosition = Vector3.zero;
        g.GetComponent<MeshRenderer>().sharedMaterial = rend.sharedMaterial;
        t.planet = planet;
        g.isStatic = true;
        t.Generate(a, c, b, _counter - 1, _radius, _size, _s2);
        rend.enabled = false;
    }

    void Divide()
    {
        Debug.Log("Dividing");
        MeshBuilder falseMesh = new MeshBuilder();
        falseMesh.AddTriangleMesh(_a, _c, _b, _s2);
        falseMesh.Normalize();
        for (int i = 0; i < falseMesh.triangles.Count; i += 3)
        {
            makeChild(falseMesh.vertices[falseMesh.triangles[i]], falseMesh.vertices[falseMesh.triangles[i + 1]], falseMesh.vertices[falseMesh.triangles[i + 2]]);
        }
    }

    

}
