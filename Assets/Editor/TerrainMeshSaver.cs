using UnityEditor;
using UnityEngine;
using System.IO;

//*[InitializeOnLoad]*

 //To be placed in Assets/Editor folder
public class TerrainMeshSaverEditor
{
    /*static TerrainMeshSaverEditor()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            LoadMeshInEditor();
        }
        else if (state == PlayModeStateChange.ExitingEditMode)
        {
            SaveMeshInEditor();
        }
    }

    private static void LoadMeshInEditor()
    {
        string savePath = Application.persistentDataPath + "/terrain_mesh_data.json";

        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Terrain mesh data not found. Please generate and save the mesh data using TerrainToMesh.");
            return;
        }

        string json = File.ReadAllText(savePath);
        MeshData meshData = JsonUtility.FromJson<MeshData>(json);

        Mesh mesh = new Mesh();
        mesh.vertices = meshData.vertices;
        mesh.triangles = meshData.triangles;
        mesh.uv = meshData.uv;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GameObject terrainMeshObject = GameObject.Find("TerrainMesh");
        if (terrainMeshObject == null)
        {
            terrainMeshObject = new GameObject("TerrainMesh");
            terrainMeshObject.AddComponent<MeshFilter>();
            terrainMeshObject.AddComponent<MeshRenderer>();
        }

        MeshFilter meshFilter = terrainMeshObject.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // Optionally set the material if needed
        // MeshRenderer meshRenderer = terrainMeshObject.GetComponent<MeshRenderer>();
        // meshRenderer.material = <your material here>;
    }

    private static void SaveMeshInEditor()
    {
        string savePath = Application.persistentDataPath + "/terrain_mesh_data.json";
        GameObject terrainMeshObject = GameObject.Find("TerrainMesh");

        if (terrainMeshObject == null || !terrainMeshObject.GetComponent<MeshFilter>().sharedMesh)
        {
            Debug.LogWarning("Terrain mesh not found in the scene.");
            return;
        }

        Mesh mesh = terrainMeshObject.GetComponent<MeshFilter>().sharedMesh;
        MeshData meshData = new MeshData(mesh.vertices, mesh.triangles, mesh.uv);
        string json = JsonUtility.ToJson(meshData);
        File.WriteAllText(savePath, json);
    }*/
}
