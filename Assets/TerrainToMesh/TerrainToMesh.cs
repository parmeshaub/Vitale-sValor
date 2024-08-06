using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class TerrainToMesh : MonoBehaviour
{
    public Terrain terrain;
    public int resolution = 512; // Adjust as needed

    private string savePath; // Path to save the mesh data
    private bool isGeneratingMesh = false; // Prevents multiple mesh generation at the same time

    void Start()
    {
        savePath = Application.persistentDataPath + "/terrain_mesh_data.json";

        
        // Start the delayed mesh generation coroutine
        StartCoroutine(DelayedGenerateMesh());
    }

    void OnDestroy()
    {
        Undo.undoRedoPerformed -= OnUndoRedoPerformed;
    }

    void OnUndoRedoPerformed()
    {
        if (!isGeneratingMesh)
        {
            StartCoroutine(DelayedGenerateMesh());
        }
    }

    IEnumerator DelayedGenerateMesh()
    {
        isGeneratingMesh = true;
        yield return new WaitForSeconds(1f); // Adjust the delay as needed
        GenerateMesh();
        isGeneratingMesh = false;
    }

    void GenerateMesh()
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain reference not set.");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;
        int terrainWidth = (int)terrainData.size.x;
        int terrainHeight = (int)terrainData.size.z;

        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        Vector2[] uv = new Vector2[resolution * resolution];

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float normalizedX = (float)x / (resolution - 1);
                float normalizedZ = (float)z / (resolution - 1);

                float height = terrainData.GetHeight(
                    Mathf.RoundToInt(normalizedX * heightmapWidth),
                    Mathf.RoundToInt(normalizedZ * heightmapHeight)
                );

                vertices[z * resolution + x] = new Vector3(
                    normalizedX * terrainWidth,
                    height,
                    normalizedZ * terrainHeight
                );

                uv[z * resolution + x] = new Vector2(normalizedX, normalizedZ);
            }
        }   

        int triangleIndex = 0;
        for (int z = 0; z < resolution - 1; z++)
        {
            for (int x = 0; x < resolution - 1; x++)
            {
                int topLeft = z * resolution + x;
                int topRight = topLeft + 1;
                int bottomLeft = (z + 1) * resolution + x;
                int bottomRight = bottomLeft + 1;

                triangles[triangleIndex++] = topLeft;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = topRight;

                triangles[triangleIndex++] = topRight;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = bottomRight;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
            
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        SaveMeshData(mesh.vertices, mesh.triangles, mesh.uv);

        // Send message to TerrainMeshLoader to reload mesh data
        GameObject terrainMeshLoaderObject = GameObject.Find("TerrainMeshLoader");
        if (terrainMeshLoaderObject != null)
        {
            Debug.Log("uiu");
            terrainMeshLoaderObject.SendMessage("ReloadMeshData", SendMessageOptions.DontRequireReceiver);
        }
    }

    void SaveMeshData(Vector3[] vertices, int[] triangles, Vector2[] uv)
    {
        MeshData meshData = new MeshData(vertices, triangles, uv);
        string json = JsonUtility.ToJson(meshData);
        File.WriteAllText(savePath, json);
    }
}

[System.Serializable]
public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uv;

    public MeshData(Vector3[] vertices, int[] triangles, Vector2[] uv)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uv = uv;
    }
}
