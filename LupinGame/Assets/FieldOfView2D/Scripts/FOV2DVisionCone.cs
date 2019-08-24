using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

[RequireComponent(typeof(FOV2DEyes))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class FOV2DVisionCone : MonoBehaviour
{
    public enum Status
    {
        Idle,
        Suspicious,
        Alert
    }
    public enum Direction
    {
        Left,
        Right
    }
    public Status status;
    public Direction direction;
    public List<Material> materials;

    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;
    Mesh mesh;
    MeshRenderer meshRenderer;
    FOV2DEyes eyes;
    List<RaycastHit> hits;
    List<RaycastHit2D> hits2D; // mine
    int i;
    int v;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        eyes = gameObject.GetComponent<FOV2DEyes>();

        meshRenderer.material = materials[0];
    }

    void LateUpdate()
    {

        UpdateMesh2D();

        UpdateMeshMaterial();

        CheckParentDirection();

        switch (direction)
        {
            case Direction.Left:
                transform.rotation = Quaternion.Euler(new Vector3(0, -90, 90));
                break;

            case Direction.Right:
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, -90));
                break;
        }
    }


    void UpdateMesh2D()
    {

        hits2D = eyes.hits2D;

        if (hits2D == null || hits2D.Count == 0)
            return;

        if (mesh.vertices.Length != hits2D.Count + 1)
        {
            mesh.Clear();
            newVertices = new Vector3[hits2D.Count + 1];
            newTriangles = new int[(hits2D.Count - 1) * 3];

            i = 0;
            v = 1;
            while (i < newTriangles.Length)
            {
                if ((i % 3) == 0)
                {
                    newTriangles[i] = 0;
                    newTriangles[i + 1] = v;
                    newTriangles[i + 2] = v + 1;
                    v++;
                }
                i++;
            }
        }

        newVertices[0] = Vector3.zero;
        for (i = 1; i <= hits2D.Count; i++)
        {
            newVertices[i] = transform.InverseTransformPoint(hits2D[i - 1].point);
        }

        newUV = new Vector2[newVertices.Length];
        i = 0;
        while (i < newUV.Length)
        {
            newUV[i] = new Vector2(newVertices[i].x, newVertices[i].z);
            i++;
        }

        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.uv = newUV;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void UpdateMeshMaterial()
    {
        for (i = 0; i < materials.Count; i++)
        {
            if (i == (int)status && meshRenderer.material != materials[i])
            {
                meshRenderer.material = materials[i];
            }
        }
    }

    void CheckParentDirection()
    {
         var rotateValue = transform.parent.gameObject.transform.rotation.y;
         if (rotateValue == 0)
         {
             direction = Direction.Right;
         }
         else
         {
             direction = Direction.Left;
         }
    }
}
