using UnityEngine;
using System.IO;

/// <summary>
/// Attach this script to an empty game object in your scene and assign a prefab to it
/// </summary>  
public class TerrainMeshLoader : MonoBehaviour
{
    public GameObject terrainMeshPrefab; // Prefab to instantiate the terrain mesh

    private string savePath; // Path to save the mesh data

    void Start()
    {
        savePath = Application.persistentDataPath + "/terrain_mesh_data.json";

        // Check if terrain data exists
        if (File.Exists(savePath))
        {
            // Delete existing terrain data
            File.Delete(savePath);
        }

        // Check for terrain mesh data when the script is started
        CheckForMeshData();
    }

    void CheckForMeshData()
    {
        // Check if terrain mesh data exists
        if (File.Exists(savePath))
        {
            LoadMesh();
        }
        else
        {
            Debug.LogWarning("Terrain mesh data not found. Please generate and save the mesh data using TerrainToMesh.");
        }
    }

    void LoadMesh()
    {
        // Load terrain mesh data from file
        string json = File.ReadAllText(savePath);
        MeshData meshData = JsonUtility.FromJson<MeshData>(json);

        if (meshData == null || meshData.vertices == null || meshData.vertices.Length == 0)
        {
            Debug.LogWarning("Invalid terrain mesh data. Please ensure the mesh data is valid and complete.");
            return;
        }

        // Create mesh from loaded data
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.vertices;
        mesh.triangles = meshData.triangles;
        mesh.uv = meshData.uv;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Instantiate terrain mesh prefab and assign loaded mesh
        GameObject terrainMeshObject = Instantiate(terrainMeshPrefab, Vector3.zero, Quaternion.identity);
        MeshFilter meshFilter = terrainMeshObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = terrainMeshObject.AddComponent<MeshFilter>();
        }
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = terrainMeshObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = terrainMeshObject.AddComponent<MeshRenderer>();
        }
        // Optionally set the material
        // meshRenderer.material = terrainToMesh.terrain.materialTemplate;
    }

    // Method to reload mesh data externally (e.g., called from TerrainToMesh script)
    public void ReloadMeshData()
    {
        Debug.Log("checked again");
        CheckForMeshData();
    }
}
