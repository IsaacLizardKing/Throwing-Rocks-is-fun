using UnityEngine;
using Unity.Collections;
using UnityEngine.Rendering;
using Unity.Mathematics;
using static Unity.Mathematics.math;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainChild : MonoBehaviour
{
    [SerializeField] int chunkSize = 64;
    [SerializeField] int LODStepSize = 128;
    public Vector3 cornerGoalz;
    public TerrainData terra;
    private int LOD = 0;
    int LODscale;
    Camera mCamera;

    void OnEnable() {
        if(terra != null && GetComponent<MeshFilter>().mesh != null && cornerGoalz != null && Mathf.Pow(2, LOD) < chunkSize) {
            var mesh = new Mesh {
                name = "TerrainShaderMesh"
            };
            
            mesh.vertices = getTerrainVertices();
            mesh.triangles = polygonizeVertices();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.uv = getTerrainUvs();

            GetComponent<MeshFilter>().mesh = mesh;
        }
    }

    void Resample() {
        if(terra != null && cornerGoalz != null && Mathf.Pow(2, LOD) < chunkSize) {
            var mesh = new Mesh {
                name = "TerrainShaderMesh"
            };
            mesh.vertices = getTerrainVertices();
            mesh.triangles = polygonizeVertices();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.uv = getTerrainUvs();

            GetComponent<MeshFilter>().mesh = mesh;
        }
    }

    void OnDisable() {
		var mesh = new Mesh {
			name = "TerrainShaderMesh"
		};
        GetComponent<MeshFilter>().mesh = mesh;
    }

    Vector3[] getTerrainVertices() {
        int LODscale = (int) Mathf.Pow(2, LOD);
        var Xbound = (int) chunkSize / LODscale;
        var Zbound = (int) chunkSize / LODscale;

        float xDiff = cornerGoalz.x - transform.localPosition.x;
        float zDiff = cornerGoalz.z - transform.localPosition.z;

        Vector3[] verts = new Vector3[Xbound * Zbound];
        var x = 0;
        while(x < Xbound) {
            var z = 0;
            while(z < Zbound) {
                float xPos = (x / ((float) (Xbound - 1))) * xDiff;
                float zPos = (z / ((float) (Zbound - 1))) * zDiff;

                verts[x * Xbound + z] = new Vector3(xPos, interpHeight(xPos, zPos), zPos);
                z += 1;
            } 
            x += 1;
        }

        return verts;
    }
    
    float interpHeight(float xPos, float zPos) {
        float a = terra.GetHeight((int) ((transform.localPosition.x + xPos) / 2.34f), (int) ((transform.localPosition.z + zPos) / 2.34f));
        float x = terra.GetHeight((int) ((transform.localPosition.x + xPos) / 2.34f + 1), (int) ((transform.localPosition.z + zPos) / 2.34f));
        float z = terra.GetHeight((int) ((transform.localPosition.x + xPos) / 2.34f), (int) ((transform.localPosition.z + zPos) / 2.34f + 1));
        float xz = terra.GetHeight((int) ((transform.localPosition.x + xPos) / 2.34f + 1), (int) ((transform.localPosition.z + zPos) / 2.34f + 1));
        float aw = Mathf.Sqrt(Mathf.Pow(xPos - ((int) xPos), 2) + Mathf.Pow(zPos - ((int) zPos), 2));
        float xw = Mathf.Sqrt(Mathf.Pow(((int) (xPos + 1)) - xPos, 2) + Mathf.Pow(zPos - ((int) zPos), 2));
        float zw = Mathf.Sqrt(Mathf.Pow(xPos - ((int) xPos), 2) + Mathf.Pow(((int) (zPos + 1)) - zPos, 2));
        float ww = Mathf.Sqrt(Mathf.Pow(((int) (xPos + 1)) - xPos, 2) + Mathf.Pow(((int) (zPos + 1)) - zPos, 2));
        float wn = 1 / (aw + xw + zw + ww);
        
        return (a * aw + x * xw + z * zw + xz * ww) * wn;
    }

    int[] polygonizeVertices () {
        int LODscale = (int) Mathf.Pow(2, LOD);
        var Xbound = (int) chunkSize / LODscale;
        var Zbound = (int) chunkSize / LODscale;
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
                tris[counter] = p3;
                counter += 1;
                tris[counter] = p2;
                counter += 1;
                tris[counter] = p4;
                counter += 1;
                z += 1;
            } 
            x += 1;
        }
        return tris;
    }

    Vector2[] getTerrainUvs() {
        int LODscale = (int) Mathf.Pow(2, LOD);
        var Xbound = (int) chunkSize / LODscale;
        var Zbound = (int) chunkSize / LODscale;

        float xDiff = cornerGoalz.x - transform.localPosition.x;
        float zDiff = cornerGoalz.z - transform.localPosition.z;

        Vector2[] uvs = new Vector2[Xbound * Zbound];
        var x = 0;
        while(x < Xbound) {
            var z = 0;
            while(z < Zbound) {
                float xPos = (x / ((float) (Xbound - 1))) * xDiff;
                float zPos = (z / ((float) (Zbound - 1))) * zDiff;

                uvs[x * Xbound + z] = new Vector2((transform.localPosition.x + xPos) / 1200, (transform.localPosition.z + zPos) / 1200);
                z += 1;
            } 
            x += 1;
        }

        return uvs;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        mCamera = Camera.main;
    }

    void updateLOD() {
        var tileCenter = transform.position + new Vector3(chunkSize / 2, 0, chunkSize / 2);
        var camPos = mCamera.transform.position;
        var relativePos = tileCenter - camPos;

        
        LOD = (int) Mathf.Sqrt(relativePos.x * relativePos.x + relativePos.y * relativePos.y + relativePos.z * relativePos.z) / LODStepSize;
        if(Mathf.Pow(2, LOD) >= chunkSize) {
            LOD = (int) (Mathf.Log(chunkSize, 2) - 2);
        }
        if(mCamera.WorldToScreenPoint(tileCenter).z < -1) LOD -= 1;
        if(LOD < 0) LOD = 0;
    }

    // Update is called once per frame
    void Update() {
        if(gameObject.activeSelf) {
            var currentLOD = LOD;
            updateLOD();
            if (currentLOD != LOD) Resample();
        }
    }
}
