using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

public class Interactor : MonoBehaviour
{
    public GameObject controller; // The sphere controller in charge of lerping textures
    public GameObject lightingManager;


    public Transform influencingObject; // Assign Object A in the inspector
    public Renderer objectRenderer; // Renderer of Object B
    private Material[] materialInstances;

    /// <summary>
    /// Variables below used for texture blending and size increment
    /// </summary>
    public Vector3 targetScale = new Vector3(2f, 2f, 2f); // The scale you want to reach
    public float duration = 2f; // The duration over which the scale change will occur

    private Vector3 initialScale;
    private float timeElapsed;

    /// <summary>
    /// Textures for nodes in shader graph to control environmental blending
    /// </summary>
    public Texture2D[] floraTexture;
    public Texture2D[] flurryTexture;
    public Texture2D[] fyreTexture;

    private Texture currentTex;
    private Texture targetTex;

    public Color flurryBoundColor;
    public Color fyreBoundColor;
    public Color floraBoundColor;

    public float normalDecreaseRate = 1.0f; // Rate at which the float value decreases
    private TargetInteractor myScript;

    public void Start()
    {
        // Acknowledges materials assigned for texture blending
        Renderer renderer = this.GetComponent<Renderer>();
        Material[] materials = renderer.materials;

        materialInstances = new Material[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            // Access each material instance
            Material material = materials[i];

            // Dk why all models have this mat even when it is not there
            if (material.name != "Lit (Instance)")
            {
                // Store the material instance in the materialInstances array
                materialInstances[i] = material;
            }         
        }

        // Used for growing
        initialScale = transform.localScale; // Store the initial scale


        myScript = controller.GetComponent<TargetInteractor>();
    }

    /// <summary>
    /// Making controller grow
    /// </summary>
    public IEnumerator GrowController(string goingWhere)
    {

        NodeTextureSwap(goingWhere, "set"); // Grows into what dimension

        timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            controller.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the target scale is set at the end of the duration
        controller.transform.localScale = targetScale;

        // After grows finish, swap the texture nodes for future swaps
        NodeTextureSwap(goingWhere, "done");
    }

    /// <summary>
    /// Making controller shrink, reverse scales
    /// </summary>
    public IEnumerator ShrinkController()
    {
        timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            controller.transform.localScale = Vector3.Lerp(targetScale, initialScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the target scale is set at the end of the duration
        controller.transform.localScale = initialScale;
    }

    public IEnumerator ChangeNormalStrength(string state)
    {
        var minValue = 0f;

        for (int i = 0; i < materialInstances.Length; i++)
        {
            Material material = materialInstances[i];

            if (state == "decreasee")
            {
                yield return new WaitForSeconds(0.0000000000001f);
                //float currentValue = material.GetFloat("_NoiseStrength");
                //currentValue -= normalDecreaseRate * Time.deltaTime;
                //currentValue = Mathf.Max(currentValue, minValue);
                //material.SetFloat("_NoiseStrength", currentValue);


                float currentValue = material.GetFloat("_NoiseScale");
                currentValue -= normalDecreaseRate * Time.deltaTime;
                currentValue = Mathf.Max(currentValue, minValue);


            }

            else if (state == "reset")
            {
                material.SetFloat("_NoiseStrength", 0.53f);
            }  

            while (true)
            {
                float currentValue = material.GetFloat("_NoiseStrength");

            }
            
            
        }    
    }


    /// <summary>
    /// When done swapping texture
    /// </summary>
    public void NodeTextureSwap(string goingWhere, string process)
    {
        for (int i = 0; i < materialInstances.Length; i++)
        {
            Material material = materialInstances[i];
            
            if (goingWhere == "flurry")
            {
                TextureSorter(material, "flurry", process);
            }

            else if (goingWhere == "fyre")
            {
                TextureSorter(material, "fyre", process);
            }

            else if (goingWhere == "flora")
            {
                TextureSorter(material, "flora", process);
            }
        }

        if (process == "done")
        {
            // After completing texture swap
            StartCoroutine(ShrinkController());
            ControllerDetails("hide", "none");
        }      
    }

    /// <summary>
    /// Replaces the current texture with the targetted texutre through same naming convention
    /// </summary>
    public void TextureSorter(Material mat, string goingWhere, string process)
    {
        var currentName = mat.GetTexture("_currentTex").name; // gets current texture

        if (goingWhere == "flora")
        {
            foreach (Texture2D texture in floraTexture)
            {
                if (texture.name == currentName) // If similar texture name, replace
                {
                    if (process == "done") // if its done, set the texture
                    {
                        mat.SetTexture("_currentTex", texture); // Set the current to the changed 
                        //mat.SetTexture("_targetTex", null);
                    }
                    else if (process == "set") // if its not done, set the target texture
                    {
                        mat.SetTexture("_targetTex", texture);
                    }
                }                 
            }
        }

        if (goingWhere == "flurry")
        {
            foreach (Texture2D texture in flurryTexture)
            {
                if (texture.name == currentName) // If similar texture name, replace
                {
                    if (process == "done") // if its done, set the texture
                    {
                        mat.SetTexture("_currentTex", texture); // Set the current to the changed 
                        //mat.SetTexture("_targetTex", null);
                    }
                    else if (process == "set") // if its not done, set the target texture
                    {
                        mat.SetTexture("_targetTex", texture);
                    }
                }              
            }
        }

        if (goingWhere == "fyre")
        {
            foreach (Texture2D texture in fyreTexture)
            {
                if (texture.name == currentName) // If similar texture name, replace
                {
                    if (process == "done") // if its done, set the texture
                    {
                        mat.SetTexture("_currentTex", texture); // Set the current to the changed 
                        //mat.SetTexture("_targetTex", null);
                    }
                    else if (process == "set") // if its not done, set the target texture
                    {
                        mat.SetTexture("_targetTex", texture);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Hiding normal and displaying color
    /// </summary>
    /// <returns></returns>
    public void ControllerDetails(string state, string goingWhere)
    {     
        for (int i = 0; i < materialInstances.Length; i++)
        {
            Material material = materialInstances[i];
            if (state == "hide")
            {
                material.SetFloat("_RadiusNoiseCutoff", 0f); // Hides the noise effect
            }

            else if (state == "show")
            {
                material.SetFloat("_RadiusNoiseCutoff", 0.53f); // Show noise
            }


            // Sets color for boundaries
            if (goingWhere == "flurry")
            {
                material.SetColor("_EdgeColor", flurryBoundColor); 
            }
            else if (goingWhere == "flora")
            {
                material.SetColor("_EdgeColor", floraBoundColor);
            }
            else if (goingWhere == "fyre")
            {
                material.SetColor("_EdgeColor", fyreBoundColor);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        Material material = materialInstances[0];
        Debug.Log(material.GetTexture("_currentTex"));

        
        for (int i = 0; i < materialInstances.Length; i++)
        {
            Material currentMaterial = materialInstances[i];

            currentMaterial.SetVector("_InfluencingObjectPos", influencingObject.position);
            currentMaterial.SetVector("_InfluencingObjectScale", influencingObject.localScale);  // Update scale
        }
        


        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GrowController("flurry"));
            ControllerDetails("show", "flurry");
            StartCoroutine(ChangeNormalStrength("decrease"));
            myScript.GoingWhere("flurry");
            lightingManager.GetComponent<LightingManager>().FloraToFlurry();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(GrowController("flora"));
            ControllerDetails("show", "flora");
            StartCoroutine(ChangeNormalStrength("decrease"));
            myScript.GoingWhere("flora");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(GrowController("fyre"));
            ControllerDetails("show", "fyre");
            StartCoroutine(ChangeNormalStrength("decrease"));
            myScript.GoingWhere("fyre");
        }

    }
}