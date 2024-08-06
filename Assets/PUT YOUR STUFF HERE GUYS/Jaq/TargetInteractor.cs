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


    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger zone has the specified tag
        if (other.gameObject.tag == "Tree")
        {
            Transform treeObj = other.gameObject.transform;

            foreach (Transform child in treeObj)
            {
                if (child.name == "bush")
                {
                    StartCoroutine(LerpTreeColor(child.gameObject.GetComponent<Renderer>(), "flurry"));
                }
            }
        }
    }
        public IEnumerator LerpTreeColor(Renderer renderer, string goingWhere)
        {

            float elapsedTime = 0.0f;

            Color originalColor = renderer.material.GetColor("_LeafColor");

            while (elapsedTime < treeDuration)
            {
                // Increment elapsed time
                elapsedTime += Time.deltaTime;

                // Calculate the lerp value
                float lerpValue = elapsedTime / treeDuration;

                // Lerp the color
                Color currentColor = Color.Lerp(originalColor, treeSnowColor, lerpValue);

                // Apply the current color to the material
                renderer.material.SetColor("_LeafColor", currentColor);

            // Yield until the next frame
            yield return null;
            }
        }
    
}
