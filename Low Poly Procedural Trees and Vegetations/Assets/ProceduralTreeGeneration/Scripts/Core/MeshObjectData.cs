using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshObjectData
{
    public Vector3 position;
    public Vector3[] vertices;
    public Vector3[] normals;
    public Vector4[] tangents;
    public int[] triangles;
    public Vector2[] uv;
    public Vector2[] uv2;
    public Vector2[] uv3;
    public Vector2[] uv4;
    public Color[] colors;


    public Mesh ToMesh()
    {
        Mesh m = new Mesh();
        m.MarkDynamic();
        m.vertices = vertices;
        m.normals = normals;
        m.tangents = tangents;
        m.triangles = triangles;
        m.uv = uv;
        m.uv2 = uv2;
        m.uv3 = uv3;
        m.uv4 = uv4;
        m.colors = colors;
        return m;
    }
}