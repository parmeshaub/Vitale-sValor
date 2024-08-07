using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public Light directionalLight; // Assign your directional light in the inspector

    /// <summary>
    /// For directional light color lerp
    /// </summary>
    public Color floraColor;
    public Color flurryColor;
    public float changeDuraiton;
    private float lerpTime = 0f;

    public void Start()
    {
        lightingRecognition();
        //StartCoroutine(LerpColor(floraColor, flurryColor));
    }

    private void lightingRecognition()
    {
        floraColor = directionalLight.color;
    }

    public void FloraToFlurry()
    {
        Debug.Log("called");
        StartCoroutine(LerpColor(floraColor, flurryColor));
    }




    private IEnumerator LerpColor(Color startColor, Color endColor)
    {
        float lerpTime = 0f;


        while (lerpTime < 1.0f)
        {
            lerpTime += Time.deltaTime / changeDuraiton;
            directionalLight.color = Color.Lerp(startColor, endColor, lerpTime);
            yield return null;
        }

        // Swap start and end colors
        Color temp = startColor;
        startColor = endColor;
        endColor = temp;

        // Reset lerpTime to loop the color lerp
        lerpTime = 0f;

    }
}
