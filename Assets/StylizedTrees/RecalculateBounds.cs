using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecalculateBounds : MonoBehaviour
{
    private void Start()
    {
        foreach (Transform treeChild in this.transform)
        {
            if (treeChild.name == "bush")
            {
                Mesh mesh = treeChild.GetComponent<MeshFilter>().mesh;
                mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10000);
            }
        }   
    }

}
