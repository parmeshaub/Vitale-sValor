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

    private int hard = 0;
    public string goingWhere;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        // Check if the object entering the trigger zone has the specified tag
        if (other.gameObject.name == "bush")
        {
            Transform treeObj = other.gameObject.transform;

            if (hard <= 0)
            {

                StartCoroutine(LerpTreeColor(treeObj.gameObject.GetComponent<Renderer>()));
            }


            /*foreach (Transform child in treeObj)
            {
                if (child.name == "bush")
                {
                    StartCoroutine(LerpTreeColor(child.gameObject.GetComponent<Renderer>(), "flurry"));
                }
            }*/
        }

        if (other.gameObject.name == "water")
        {

        }
    }

    public void GoingWhere(string goingWheree)
    {
        goingWhere = goingWheree;
    }


    public IEnumerator LerpTreeColor(Renderer renderer)
    {

        float elapsedTime = 0.0f;

        Color originalColor = renderer.material.GetColor("_LeafColor");

        while (elapsedTime < treeDuration)
        {
            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the lerp value
            float lerpValue = elapsedTime / treeDuration;

            if (goingWhere == "flurry")
            {
                // Lerp the color
                Color currentColor = Color.Lerp(originalColor, treeSnowColor, lerpValue);

                // Apply the current color to the material
                renderer.material.SetColor("_LeafColor", currentColor);
                renderer.material.SetFloat("_SwaySpeed", 0.03f);
            }

            if (goingWhere == "flora")
            {
                // Lerp the color
                Color currentColor = Color.Lerp(originalColor, treeNatureColor, lerpValue);

                // Apply the current color to the material
                renderer.material.SetColor("_LeafColor", currentColor);
                renderer.material.SetFloat("_SwaySpeed", 2.4f);
            }


        // Yield until the next frame
        yield return null;
        }
    }
    
}
