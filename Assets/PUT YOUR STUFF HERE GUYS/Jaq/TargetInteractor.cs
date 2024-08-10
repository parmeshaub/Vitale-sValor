using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class TargetInteractor : MonoBehaviour
{
    public Color treeNatureColor;
    public Color treeFireColor;
    public Color treeSnowColor;
    public float treeSwaySpeed;

    public float treeDuration = 5.0f; // Duration of the lerp
    public float triplanarDuration = 2.0f; //Duration of triplanar lerp


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

        // This is for all assets. Dungeon exteriors, village props, rocks and etc.
        // Checking the object's layer name
        if (other.gameObject.tag == "Changable")
        {
            StartCoroutine(Triplanar(other.gameObject.GetComponent<Renderer>(), goingWhere));
        }
    }


    public IEnumerator Triplanar(Renderer renderer, string goingWhere)
    {
        Material[] currentMaterials = renderer.materials; // Getting all materials attached
        float gainingTime = 0.0f; // Resets time to gain for lerp



        for (int i = 0; i < currentMaterials.Length; i++)
        {
            // Access each material instance
            var mat = currentMaterials[i];
            Debug.Log(mat.name);


            if (goingWhere == "flurry")
            {
                // Holder values with default initializations
                float targetSlider = 0f;
                float targetLevel = 0f;
                Vector3 targetDirection = Vector3.zero;
                float targetSlide = 0f;

                /*if (mat.name == "triplanarbricks (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = 0.54f;
                    targetLevel = 2.2f;
                    targetDirection = new Vector3(0f, -5.82f, 0f);
                    targetSlide = 1.2f;
                }*/

                if (mat.name == "triplanarPillar (Instance)")
                {
                    // Target snowy properties of triplanar material

                    Debug.Log("GOTTEN pillar");
                    targetSlider = -0.11f;
                    targetLevel = 0.66f;
                    targetDirection = new Vector3(0.12f, -4f, 0f);
                    targetSlide = 1.16f;
                    
                }

                if (mat.name == "triplanarRoof (Instance)")
                {
                    Debug.Log("GOTTEN roof");
                    // Target snowy properties of triplanar material
                    targetSlider = 1.2f;
                    targetLevel = 2.2f;
                    targetDirection = new Vector3(4.38f, -71.1f, 21.6f);
                    targetSlide = 2.9f;

                }

                // Current triplanar properties
                float currentSlider = mat.GetFloat("_Slider");
                float currentTargetLevel = mat.GetFloat("_Level");
                Vector3 currentTargetDirection = mat.GetVector("_Direction");
                float currentSlide = mat.GetFloat("_Slide");

                if (currentSlider != 0f)
                {
                    while (gainingTime < triplanarDuration)
                    {
                        // Increment elapsed time
                        gainingTime += Time.deltaTime;
                        float lerpValue = gainingTime / triplanarDuration;
                        yield return new WaitForSeconds(0.000000001f);

                        // Lerping values
                        float currentValue1 = Mathf.Lerp(currentSlider, targetSlider, lerpValue);

                        Debug.Log(currentValue1);
                        Debug.Log(targetSlider);

                        float currentValue2 = Mathf.Lerp(currentSlide, targetSlide, lerpValue);
                        float currentValue3 = Mathf.Lerp(currentTargetLevel, targetLevel, lerpValue);
                        Vector3 currentValue4 = Vector3.Lerp(currentTargetDirection, targetDirection, lerpValue);

                        // Applying lerping values to current material renderer
                        mat.SetFloat("_Slider", currentValue1);
                        mat.SetFloat("_Slide", currentValue2);
                        mat.SetFloat("_Level", currentValue3);
                        mat.SetVector("_Direction", currentValue4);
                    }
                }             
            }
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
