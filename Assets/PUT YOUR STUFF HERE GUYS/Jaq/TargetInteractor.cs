using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class TargetInteractor : MonoBehaviour
{
    public Texture2D snowTexture;
    public Texture2D fireTexture;

    public Animator LiquidAnimator;
    public GameObject WaterObject;

    public Color treeNatureColor;
    public Color treeFireColor;
    public Color treeSnowColor;
    public float treeSwaySpeed;

    public float treeDuration = 5.0f; // Duration of the lerp
    public float triplanarDuration = 2.0f; //Duration of triplanar lerp

    public string currentWhere;
    public string goingWhere;
    public string previouslyWhere;

    public void Start()
    {
        FindObjects();
    }


    public void FindObjects()
    {
        // Find animator for liquds
        GameObject liquidParent = GameObject.Find("LiquidAnimator");
        LiquidAnimator = liquidParent.GetComponent<Animator>();

        // Game obj for water body
        WaterObject = GameObject.Find("water").transform.gameObject;
        if (WaterObject != null )
        {
            Debug.Log("found");
        }
    }


    /// <summary>
    /// Identifies the invidual gameobject elements and what to do with them
    /// </summary>
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
                StartCoroutine(LerpTreeTransparency(treeMaterial, 0.3f, 2));
            }
            else if (goingWhere == "flora")
            {
                StartCoroutine(LerpTreeTransparency(treeMaterial, 2, 0.3f));
            }
        }

        if (other.gameObject.name == "waterDetector")
        {
            LiquidSwitcher(goingWhere);
        }

        // This is for all assets. Dungeon exteriors, village props, rocks and etc.
        // Checking the object's layer name
        if (other.gameObject.tag == "Changable")
        {
            StartCoroutine(Triplanar(other.gameObject.GetComponent<Renderer>(), goingWhere));
        }
    }

    /// <summary>
    /// Switching water bodies between water, ice and lava
    /// </summary>
    public void LiquidSwitcher(string goingWhere)
    {
        if (currentWhere == "flora")
        {
            if (goingWhere == "flurry")
            {
                LiquidAnimator.SetTrigger("waterToIce");
                WaterObject.GetComponent<MeshCollider>().enabled = true;
            }

            if (goingWhere == "fyre")
            {
                LiquidAnimator.SetTrigger("waterToLava");
                WaterObject.GetComponent<MeshCollider>().enabled = false;
            }
        }

        if (currentWhere == "flurry")
        {
            if (goingWhere == "fyre")
            {
                LiquidAnimator.SetTrigger("lavaToIce");
                WaterObject.GetComponent<MeshCollider>().enabled = false;
            }

            if (goingWhere == "flora")
            {
                LiquidAnimator.SetTrigger("iceToWater");
                WaterObject.GetComponent<MeshCollider>().enabled = false;
            }
        }

        if (currentWhere == "fyre")
        {
            if (goingWhere == "flora")
            {
                LiquidAnimator.SetTrigger("lavaToWater");
                WaterObject.GetComponent<MeshCollider>().enabled = false;
            }

            if (goingWhere == "flurry")
            {
                LiquidAnimator.SetTrigger("lavaToIce");
                WaterObject.GetComponent<MeshCollider>().enabled = true;
            }

        }      
    }

    
    public IEnumerator Triplanar(Renderer renderer, string goingWhere)
    {
        yield return new WaitForSeconds(0.000000000000000000001f);
        Material[] currentMaterials = renderer.materials; // Getting all materials attached

        for (int i = 0; i < currentMaterials.Length; i++)
        {

            if (goingWhere == "flurry")
            {
                currentMaterials[i].SetTexture("_SnowTex", snowTexture);
            }

            else if (goingWhere == "fyre")
            {
                currentMaterials[i].SetTexture("_SnowTex", fireTexture);
            }

            // Target snowy properties of triplanar material
            var targetSlider = 0f;
            var targetLevel = 0f;
            var targetDirection = new Vector3(0f, 0f, 0f);
            var targetSlide = 0f;

            // Access each material instance
            var mat = currentMaterials[i];      

            if ((goingWhere == "flurry") || (goingWhere == "fyre"))
            {

                if (mat.name == "triplanarbricks (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = -3.07f;
                    targetLevel = 1.61f;
                    targetDirection = new Vector3(0f, -5.43f, 0f);
                    targetSlide = 1.2f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));
                }

                if (mat.name == "triplanarPillar (Instance)")
                {
                    targetSlider = -1.81f;
                    targetLevel = -0.03f;
                    targetDirection = new Vector3(-0.76f, 60.49f, 0.41f);
                    targetSlide = 1.16f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));
                }

                if (mat.name == "triplanarRoof (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = 0.67f;
                    targetLevel = 2.27f;
                    targetDirection = new Vector3(0.18f, 2.48f, 1.78f);
                    targetSlide = 1.2f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));

                }

                if (mat.name == "triplanarRubble (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = -0.84f;
                    targetLevel = -0.04f;
                    targetDirection = new Vector3(47.03f, 95.3f, 12.35f);
                    targetSlide = 2f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));

                }

                if (mat.name == "triplanarFloor 2 (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = 1.2f;
                    targetLevel = 2.2f;
                    targetDirection = new Vector3(4.38f, -71.1f, 21.6f);
                    targetSlide = 2.9f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));
                }

                if (mat.name == "triplanarFloor3 (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = 11.88f;
                    targetLevel = 3.66f;
                    targetDirection = new Vector3(5f, 5f, 5f);
                    targetSlide = -2.1f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));
                }

                if (mat.name == "triplanarPlane (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = 1.63f;
                    targetLevel = 0.03f;
                    targetDirection = new Vector3(6.92f, 95.3f, 36.1f);
                    targetSlide = 2f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));
                }

                if (mat.name == "triplanarSR (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = -0.1f;
                    targetLevel = 2.15f;
                    targetDirection = new Vector3(0f, -5.82f, 0f);
                    targetSlide = 1.2f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));
                }

                if (mat.name == "triplanarCliff (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = 5.24f;
                    targetLevel = 39.59f;
                    targetDirection = new Vector3(5f, 5f, 5f);
                    targetSlide = -6.81f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));
                }

                if (mat.name == "triplanarRock (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = 5.24f;
                    targetLevel = 5.8f;
                    targetDirection = new Vector3(0.6f, 2.9f, 1f);
                    targetSlide = 0f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));
                }

                if (mat.name == "triplanarT4 (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = 1.51f;
                    targetLevel = 3.17f;
                    targetDirection = new Vector3(6.4f, 146.64f, 15.92f);
                    targetSlide = 0f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));
                }

                if (mat.name == "triplanarT6 (Instance)")
                {
                    // Target snowy properties of triplanar material
                    targetSlider = 1.51f;
                    targetLevel = 3.17f;
                    targetDirection = new Vector3(6.4f, 146.64f, 15.92f);
                    targetSlide = 0f;
                    // Current triplanar properties
                    float currentSlider = mat.GetFloat("_Slider");
                    float currentTargetLevel = mat.GetFloat("_Level");
                    Vector3 currentTargetDirection = mat.GetVector("_Direction");
                    float currentSlide = mat.GetFloat("_Slide");

                    // Passing it to a function to lerp the respective properties
                    StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));
                }
            }

            else if (goingWhere == "flora")
            {
                targetSlider = 0f;
                targetLevel = 0f;
                targetDirection = new Vector3(0f, 0f, 0f);
                targetSlide = 0f;
                // Current triplanar properties
                float currentSlider = mat.GetFloat("_Slider");
                float currentTargetLevel = mat.GetFloat("_Level");
                Vector3 currentTargetDirection = mat.GetVector("_Direction");
                float currentSlide = mat.GetFloat("_Slide");

                StartCoroutine(LerpTriplanar(mat, currentSlider, targetSlider, currentTargetLevel, targetLevel, currentSlide, targetSlide, currentTargetDirection, targetDirection));
            }
            


                                 
        }
    }


    /// <summary>
    /// Lerp unique triplanar values and textures for diff mats based on their names
    /// </summary>
    private IEnumerator LerpTriplanar(Material mat, float currentSlider, float targetSlider, float currentTargetLevel, float targetLevel, float currentSlide, float targetSlide, Vector3 currentTargetDirection, Vector3 targetDirection)
    {
        float gainingTime = 0f;

        while (gainingTime < triplanarDuration)
        {
            // Increment elapsed time
            gainingTime += Time.deltaTime;
            float lerpValue = gainingTime / triplanarDuration;
            yield return new WaitForSeconds(0.000000001f);

            // Lerping values
            float currentValue1 = Mathf.Lerp(currentSlider, targetSlider, lerpValue);
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

    public void GoingWhere(string goingWheree, string currentWheree)
    {
        goingWhere = goingWheree;
        currentWhere = currentWheree;
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

            yield return new WaitForSeconds(0.000001f);
            float currentValue = Mathf.Lerp(currentTarget, finalTarget, lerpValue);
            treeMat.SetFloat("_Clip", currentValue);
        }  
    }
    
    // Do not make any update test with keybinds here! Do it in the interactor script :o
}
