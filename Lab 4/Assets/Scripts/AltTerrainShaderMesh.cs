using UnityEngine;
using Unity.Collections;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AltTerrainShaderMesh : MonoBehaviour
{
    [SerializeField] TerrainData terra;
    
    void OnEnable () {
        int vertexAttributeCount = 4;

		Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
		Mesh.MeshData meshData = meshDataArray[0];
        var mesh = new Mesh {
			name = "TerrainShaderMesh"
		};
        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);

        GetComponent<MeshFilter>().mesh = mesh;
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
