using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Mesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MeshFilter meshFilter = (MeshFilter)transform.GetComponent( typeof( MeshFilter ) );
        Mesh mesh = meshFilter.mesh;

        Vector3[] vertices = new Vector3[3];
        //三角形顶点ID数组
        int[] triangles = new int[3];

        //三角形三个定点坐标，为了显示清楚忽略Z轴
        vertices[0] = new Vector3( 0, 0, 0 );
        vertices[1] = new Vector3( 0, 1, 0 );
        vertices[2] = new Vector3( 1, 0, 0 );

        //三角形绘制顶点的数组
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        //注释1
        mesh.vertices = vertices;

        //mesh.triangles = triangles;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
