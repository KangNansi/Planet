using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder {

    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Color> colors = new List<Color>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<Vector3> normals = new List<Vector3>();
	
    public void AddTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        int offset = vertices.Count;
        vertices.Add(p1);
        vertices.Add(p2);
        vertices.Add(p3);
        triangles.Add(offset + 0);
        triangles.Add(offset + 1);
        triangles.Add(offset + 2);
    }

    public void AddEquiTriangle(Vector3 position, float radius, Vector3 target)
    {
        Vector3 up = Vector3.up * radius;
        Quaternion r = Quaternion.AngleAxis(120, Vector3.forward);
        Quaternion r2 = Quaternion.AngleAxis(240, Vector3.forward);
        Vector3 a = position + up;
        Vector3 b = position + r * up;
        Vector3 c = position + r2 * up;
        AddTriangle(a, b, c);
    }

    public void AddPlane(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        AddTriangle(p1, p2, p3);
        AddTriangle(p1, p3, p4);
    }

    public void AddInvertedPlane(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        AddTriangle(p1, p3, p2);
        AddTriangle(p1, p4, p3);
    }

    public void AddPlane(Vector3 position, Vector2 scale, Quaternion rotation)
    {
        Vector3 p1 = rotation * (new Vector3(0, 0, 0) - position) + position;
        Vector3 p2 = rotation * (new Vector3(0, scale.y, 0) - position) + position;
        Vector3 p3 = rotation * (new Vector3(scale.x, scale.y, 0) - position) + position;
        Vector3 p4 = rotation * (new Vector3(scale.x, 0, 0) - position) + position;
        AddPlane(p1, p2, p3, p4);
    }

    public void AddInvertedPlane(Vector3 position, Vector2 scale, Quaternion rotation)
    {
        Vector3 p1 = rotation * (new Vector3(0, 0, 0) - position) + position;
        Vector3 p2 = rotation * (new Vector3(scale.x, 0, 0) - position) + position;
        Vector3 p3 = rotation * (new Vector3(scale.x, scale.y, 0) - position) + position;
        Vector3 p4 = rotation * (new Vector3(0, scale.y, 0) - position) + position;
        AddPlane(p1, p2, p3, p4);
    }

    public void AddCube(Vector3 position, Vector3 scale)
    {
        AddPlane(
            new Vector3(position.x - scale.x / 2f, position.y - scale.y / 2f, position.z + scale.z / 2f),
            new Vector3(position.x + scale.x / 2f, position.y - scale.y / 2f, position.z + scale.z / 2f),
            new Vector3(position.x + scale.x / 2f, position.y + scale.y / 2f, position.z + scale.z / 2f),
            new Vector3(position.x - scale.x / 2f, position.y + scale.y / 2f, position.z + scale.z / 2f));
        AddPlane(
            new Vector3(position.x + scale.x / 2f, position.y - scale.y / 2f, position.z - scale.z / 2f),
            new Vector3(position.x - scale.x / 2f, position.y - scale.y / 2f, position.z - scale.z / 2f),
            new Vector3(position.x - scale.x / 2f, position.y + scale.y / 2f, position.z - scale.z / 2f),
            new Vector3(position.x + scale.x / 2f, position.y + scale.y / 2f, position.z - scale.z / 2f));
        AddPlane(
            new Vector3(position.x + scale.x / 2f, position.y - scale.y / 2f, position.z + scale.z / 2f),
            new Vector3(position.x + scale.x / 2f, position.y - scale.y / 2f, position.z - scale.z / 2f),
            new Vector3(position.x + scale.x / 2f, position.y + scale.y / 2f, position.z - scale.z / 2f),
            new Vector3(position.x + scale.x / 2f, position.y + scale.y / 2f, position.z + scale.z / 2f));
        AddPlane(
            new Vector3(position.x - scale.x / 2f, position.y - scale.y / 2f, position.z - scale.z / 2f),
            new Vector3(position.x - scale.x / 2f, position.y - scale.y / 2f, position.z + scale.z / 2f),
            new Vector3(position.x - scale.x / 2f, position.y + scale.y / 2f, position.z + scale.z / 2f),
            new Vector3(position.x - scale.x / 2f, position.y + scale.y / 2f, position.z - scale.z / 2f));
        AddPlane(
            new Vector3(position.x + scale.x / 2f, position.y - scale.y / 2f, position.z - scale.z / 2f),
            new Vector3(position.x + scale.x / 2f, position.y - scale.y / 2f, position.z + scale.z / 2f),
            new Vector3(position.x - scale.x / 2f, position.y - scale.y / 2f, position.z + scale.z / 2f),
            new Vector3(position.x - scale.x / 2f, position.y - scale.y / 2f, position.z - scale.z / 2f));
        AddPlane(
            new Vector3(position.x + scale.x / 2f, position.y + scale.y / 2f, position.z + scale.z / 2f),
            new Vector3(position.x + scale.x / 2f, position.y + scale.y / 2f, position.z - scale.z / 2f),
            new Vector3(position.x - scale.x / 2f, position.y + scale.y / 2f, position.z - scale.z / 2f),
            new Vector3(position.x - scale.x / 2f, position.y + scale.y / 2f, position.z + scale.z / 2f));
    }

    public void AddWall(Vector3 position, Vector3 size, Quaternion rotation)
    {

        AddPlane(rotation * position,
            rotation * (position + Vector3.right * size.x),
            rotation * (position + Vector3.right * size.x + Vector3.up * size.y),
            rotation * (position + Vector3.up * size.y));
        AddInvertedPlane(rotation * position + Vector3.forward*size.z,
            rotation * (position + Vector3.forward * size.z + Vector3.right * size.x),
            rotation * (position + Vector3.forward * size.z + Vector3.right * size.x + Vector3.up * size.y),
            rotation * (position + Vector3.forward * size.z + Vector3.up * size.y));
    }

    public void AddCylinder(Vector3 position, Vector2 size, int division, float frequency, float factor, float offset)
    {

        for(int i=0; i < division; i++)
        {
            for(int j=0; j < division; j++)
            {
                
                float x = i / (float)division;
                float y = j / (float)division;
                float x2 = (i + 1) / (float)division;
                float y2 = (j + 1) / (float)division;
                Vector3 a = position + GetSphericalVector(x, y);
                /*Vector3 b = position + GetSphericalVector(x2, y);
                Vector3 c = position + GetSphericalVector(x2, y2);
                Vector3 d = position + GetSphericalVector(x, y2);
                Debug.Log(a);
                AddInvertedPlane(a, b, c, d);*/
                vertices.Add(a);
                Quaternion rot = Quaternion.FromToRotation(Vector3.zero, position-a);
                float p = Mathf.PerlinNoise(rot.x*frequency, rot.y* frequency);
                float p2 = Mathf.PerlinNoise(rot.w * frequency, rot.z * frequency);
                colors.Add(new Color(Mathf.Clamp((p-offset)*factor+offset, 0, 1), 0, 0, 1));
            }
        }

        for(int i=0; i < division; i++)
            for(int j=0; j < division; j++)
            {
                triangles.Add(i * division + j);
                triangles.Add(((i + 1)%division) * division + j);
                triangles.Add(((i + 1) % division) * division + ((j + 1) % division));
                triangles.Add(i * division + j);
                triangles.Add(((i + 1) % division) * division + ((j + 1) % division));
                triangles.Add(i * division + ((j + 1) % division));
            }
    }

    public void AddTriangleMesh(Vector3 a, Vector3 b, Vector3 c, int subdivisions)
    {

        int d = vertices.Count;
        int u = CreateVertexLine(a, b, subdivisions);

        for(int i=1; i < subdivisions; i++)
        {
            float progress = (float)i / subdivisions;
            CreateVertexLine(Vector3.Lerp(a, c, progress), Vector3.Lerp(b, c, progress), subdivisions-i);
            CreateStrip(subdivisions-i, u, d);
            d = u;
            u = vertices.Count;
        }
        vertices.Add(c);
        triangles.Add(d);
        triangles.Add(d + 1);
        triangles.Add(u);
        

        /*
        for(int i = 0; i < resolution; i++)
        {
            float progress = i / (float)resolution;
            Vector3 from, to;
            vertices.Add(to = Vector3.Lerp(a, b, progress));
            from = Vector3.Lerp(a, c, progress);
            CreateStrip(i, vertices.Count-1, vBottom);
            CreateVertexLine(from, to, i);
            vBottom = vertices.Count - 1 - i;
        }*/
    }

    public void CreaterSphere(int div, float radius)
    {
        AddTriangleMesh(Vector3.down, Vector3.right, Vector3.forward, div);
        AddTriangleMesh(Vector3.down, Vector3.back, Vector3.right, div);
        AddTriangleMesh(Vector3.down, Vector3.left, Vector3.back, div);
        AddTriangleMesh(Vector3.down, Vector3.forward, Vector3.left, div);

        AddTriangleMesh(Vector3.up, Vector3.forward, Vector3.right, div);
        AddTriangleMesh(Vector3.up, Vector3.left, Vector3.forward, div);
        AddTriangleMesh(Vector3.up, Vector3.back, Vector3.left, div);
        AddTriangleMesh(Vector3.up, Vector3.right, Vector3.back, div);

        Normalize(radius);
    }

    public void Normalize(float factor = 1)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 v = vertices[i];
            v = Vector3.Normalize(v)*factor;
            vertices[i] = v;
        }
    }

    private int CreateVertexLine(Vector3 from, Vector3 to, int steps)
    {
        for(int i=0; i <= steps; i++)
        {

            vertices.Add(Vector3.Lerp(from, to, (float)i / steps));
        }
        return vertices.Count;
    }

    private void CreateStrip(int steps, int u, int d)
    {
        for (int i = 0; i <= steps + (steps-1) + 1; i++)
        {
            if (i % 2 == 0)
            {
                triangles.Add(d);
                d++;
                triangles.Add(d);
                triangles.Add(u);

            }
            else
            {
                triangles.Add(d);
                triangles.Add(u + 1);
                triangles.Add(u);
                u++;
            }
        }
    }

    public void CreatePlane(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        AddTriangle(a, b, c);
        AddTriangle(a, c, d);
    }

    public void CreateUVPlane(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        CreatePlane(a, b, c, d);
        uvs.Add(Vector3.zero);
        uvs.Add(Vector3.up);
        uvs.Add(Vector3.up + Vector3.right);
        uvs.Add(Vector3.zero);
        uvs.Add(Vector3.up + Vector3.right);
        uvs.Add(Vector3.right);
    }

    public void CreateUVPlane(Vector3 pos, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        CreateUVPlane(pos + a, pos + b, pos + c, pos + d);
    }

    public Vector3 GetSphericalVector(float x, float y)
    {
        return Quaternion.AngleAxis(y*360, Vector3.up) * new Vector3(Mathf.Cos(x*2*Mathf.PI), Mathf.Sin(x*2*Mathf.PI));
    }

    public float[] GetSteepness()
    {
        float[] r = new float[vertices.Count];
        int[] count = new int[vertices.Count];
        for(int i = 0; i < triangles.Count; i += 3)
        {
            float am = vertices[triangles[i]].magnitude;
            float bm = vertices[triangles[i + 1]].magnitude;
            float cm = vertices[triangles[i + 2]].magnitude;
            r[triangles[i]] += (Mathf.Abs(am-bm) + Mathf.Abs(am-cm))/am;
            r[triangles[i + 1]] += (Mathf.Abs(bm - am) + Mathf.Abs(bm - cm))/bm;
            r[triangles[i + 2]] += (Mathf.Abs(cm - am) + Mathf.Abs(cm - bm))/cm;
        }
        return r;
    }


    public Mesh ToMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.normals = normals.ToArray();
        mesh.RecalculateBounds();
        return mesh;
    }

}
