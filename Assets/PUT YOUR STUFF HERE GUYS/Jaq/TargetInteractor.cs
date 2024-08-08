using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInteractor : MonoBehaviour
{
    public Color treeNatureColor;
    public Color treeFireColor;
    public Color treeSnowColor;
    public float treeSwaySpeed;

    public float treeDuration = 5.0f; // Duration of the lerp

    public string goingWhere;

    private void Start()
    {
        goingWhere = "flurry";
    }

    
    void OnTriggerEnter(Collider other)
    {

        // Check if the object entering the trigger zone has the specified tag
        if (other.gameObject.name == "Tree")
        {
            Renderer treeObj = other.gameObject.GetComponent<Renderer>();
            Material treeMaterial = treeObj.materials[1];

            Debug.Log(treeMaterial.name);

            if ((goingWhere == "flurry") || (goingWhere == "fyre"))
            {
                StartCoroutine(LerpTreeTransparency(treeMaterial, 1, 0));
            }
            else if (goingWhere == "flora")
            {
                StartCoroutine(LerpTreeTransparency(treeMaterial, 0, 1));
            }
        }

        if (other.gameObject.name == "water")
        {

        }
    }

    public void GoingWhere(string goingWheree)
    {
        goingWhere = goingWheree;
    }


    public IEnumerator LerpTreeTransparency(Material treeMat, float currentTarget ,float finalTarget)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < treeDuration)
        {
            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the lerp value
            float lerpValue = elapsedTime / treeDuration;


            float currentValue = Mathf.Lerp(currentTarget, finalTarget, lerpValue);
            treeMat.SetFloat("_Alpha", currentValue);
        }

        // Yield until the next frame
        yield return null;
        
    }
    
}
