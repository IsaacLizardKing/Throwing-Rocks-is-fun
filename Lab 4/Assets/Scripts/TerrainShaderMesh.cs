using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainShaderMesh : MonoBehaviour
{
    [SerializeField] TerrainData terra;
    
    void OnEnable () {
		var mesh = new Mesh {
			name = "TerrainShaderMesh"
		};
        
        mesh.vertices = getTerrainVertices();
        mesh.triangles = polygonizeVertices();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        GetComponent<MeshFilter>().mesh = mesh;
	}
    
    
    Vector3[] getTerrainVertices() {
        var tSize = terra.size;
        var Xbound = (int)tSize[0];
        var Yscale = tSize[1];
        var Zbound = (int)tSize[2];

        Vector3[] verts = new Vector3[Xbound * Zbound];
        var x = 0;
        while(x < Xbound){
            var z = 0;
            while(z < Zbound) {
                //terra.GetHeight(x, z)
                verts[x * Xbound + z] = new Vector3(x, z, z);
                z += 1;
            } 
            x += 1;
        }
        return verts;
    }
    
    int[] polygonizeVertices () {
        var tSize = terra.size;
        var Xbound = (int)tSize[0];
        var Zbound = (int)tSize[2];
        var numTriNums = (Xbound - 1) * (Zbound - 1) * 6;
        int[] tris = new int[numTriNums];
        var x = 0;
        var counter = 0;
        while(x < Xbound - 1){
            var z = 0;
            while(z < Zbound - 1) {
                int p1 = x * Xbound + z;
                int p2 = x * Xbound + z + 1;
                int p3 = (x + 1) * Xbound + z;
                int p4 = (x + 1) * Xbound + z + 1;
                tris[counter] = p1;
                counter += 1;
                tris[counter] = p2;
                counter += 1;
                tris[counter] = p3;
                counter += 1;
                tris[counter] = p2;
                counter += 1;
                tris[counter] = p3;
                counter += 1;
                tris[counter] = p4;
                counter += 1;
                z += 1;
            } 
            x += 1;
        }
        return tris;
    }
    
    Vector3[] getTerrainNormals(Mesh mesh) {
        return new Vector3[] {
			Vector3.back, Vector3.back, Vector3.back
		};
    }   

    Vector4[] getTerrainTangents(Mesh mesh) {
        return new Vector4[] {
			new Vector4(1f, 0f, 0f, -1f),
			new Vector4(1f, 0f, 0f, -1f),
			new Vector4(1f, 0f, 0f, -1f)
		};
    }

    Vector2[] getUvs() {
        return new Vector2[] {
			Vector2.zero, Vector2.right, Vector2.up
		};
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
