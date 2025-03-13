using UnityEngine;

public class TerrainParent : MonoBehaviour
{
    [SerializeField] int chunkSize = 64;
    [SerializeField] TerrainData terra;
    [SerializeField] GameObject tile;
    [SerializeField] int manhattanLoadDist = 128;
    [SerializeField] float screenLoadCushion = 0.1f;
    GameObject[] tiles;
    Camera mCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        mCamera = Camera.main;

        var tSize = terra.size;
        var Xbound = (int)(terra.size[0] / chunkSize);
        var Zbound = (int)(terra.size[2] / chunkSize);
        
        tiles = new GameObject[Xbound * Zbound];

        var x = 0;
        while (x < Xbound) {
            var z = 0;
            while (z < Zbound) {
                GameObject tileInstance = Instantiate(tile);
                tileInstance.name = $"tile{x * Xbound + z}";
                tileInstance.transform.SetParent(gameObject.transform);
                tileInstance.transform.localPosition = new Vector3(x * chunkSize, 0, z * chunkSize);
                TerrainChild a = tileInstance.GetComponent<TerrainChild>();
                a.terra = terra;
                a.cornerGoalz = new Vector3((x + 1) * chunkSize, 0, (z + 1) * chunkSize);
                tileInstance.SetActive(false);
                tileInstance.SetActive(toLoadOrNot(tileInstance));
                tiles[x * Xbound + z] = tileInstance;
                z += 1;
            }
            x += 1;
        }
    }

    // Update is called once per frame
    void Update() {
        foreach (GameObject tileInstance in tiles) {
            tileInstance.SetActive(toLoadOrNot(tileInstance));
        }
    }

    bool toLoadOrNot(GameObject tileInstance) {
        var tileCenter = tileInstance.transform.position + new Vector3(chunkSize / 2, 0, chunkSize / 2);
        var camPos = mCamera.transform.position;
        var relativePos = tileCenter - camPos;

        if (Mathf.Abs(relativePos.x) + Mathf.Abs(relativePos.z) < manhattanLoadDist) return true;
        var cameraSpace = mCamera.WorldToScreenPoint(tileCenter);
        var onScreen = cameraSpace.x / (float) mCamera.pixelWidth;
        if (onScreen > -screenLoadCushion && onScreen < 1 + screenLoadCushion && cameraSpace.z > 0) return true;
        if (cameraSpace.z < 0 || onScreen < -0.4 || onScreen > 1.4) return false;

        tileCenter = tileInstance.transform.position + new Vector3(chunkSize, 0, chunkSize);
        cameraSpace = mCamera.WorldToScreenPoint(tileCenter);
        onScreen = cameraSpace.x / (float) mCamera.pixelWidth;
        if (onScreen > -screenLoadCushion && onScreen < 1 + screenLoadCushion && cameraSpace.z > 0) return true;

        tileCenter = tileInstance.transform.position + new Vector3(0, 0, chunkSize);
        cameraSpace = mCamera.WorldToScreenPoint(tileCenter);
        onScreen = cameraSpace.x / (float) mCamera.pixelWidth;
        if (onScreen > -screenLoadCushion && onScreen < 1 + screenLoadCushion && cameraSpace.z > 0) return true;

        cameraSpace = mCamera.WorldToScreenPoint(tileInstance.transform.position);
        onScreen = cameraSpace.x / (float) mCamera.pixelWidth;
        if (onScreen > -screenLoadCushion && onScreen < 1 + screenLoadCushion && cameraSpace.z > 0) return true;

        tileCenter = tileInstance.transform.position + new Vector3(chunkSize, 0, 0);
        cameraSpace = mCamera.WorldToScreenPoint(tileCenter);
        onScreen = cameraSpace.x / (float) mCamera.pixelWidth;
        if (onScreen > -screenLoadCushion && onScreen < 1 + screenLoadCushion && cameraSpace.z > 0) return true;

        return(false);
    }
}
