using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    void Start(){
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

   
}
