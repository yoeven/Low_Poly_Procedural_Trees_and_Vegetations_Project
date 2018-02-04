/* 
 * The following code is based of this thread: https://answers.unity.com/questions/798510/flat-shading.html
 *
 * This function flat shades any mesh.
 * 
 */


using UnityEngine;

public static class FlatShader
{
    public static void flatShade(this Mesh mesh)
    {
        Vector3[] oldVerts = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] vertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            vertices[i] = oldVerts[triangles[i]];
            triangles[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
