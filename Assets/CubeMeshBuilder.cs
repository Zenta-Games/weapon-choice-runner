using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMeshBuilder : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] vertices;

    public GameObject dynamicCubeSample;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
           GameObject cube = Instantiate(dynamicCubeSample, transform.position + vertices[i],Quaternion.identity,this.transform);

           // cube.transform.LookAt(transform.position);
        }
    }
}
