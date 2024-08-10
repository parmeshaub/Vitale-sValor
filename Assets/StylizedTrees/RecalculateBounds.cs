using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecalculateBounds : MonoBehaviour
{
    public Color[] greenColors;
    private Material leaveMat;


    private void Start()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10000);

        Renderer renderer = GetComponent<Renderer>();
        Material[] materials = renderer.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            if ((materials[i].name == "leave_1 (Instance)") || (materials[i].name == "leave_1_2 (Instance)"))
            {
                leaveMat = renderer.materials[i];
                ColorRandomizer();
            }
        }
    }




    public void ColorRandomizer()
    {
        int randomIndex = Random.Range(0, greenColors.Length);
        leaveMat.SetColor("_LeafColor", greenColors[randomIndex]);
    }

}
