using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightingManager : MonoBehaviour
{
    public Light directionalLight; // Assign your directional light in the inspector
    public Material skyBox;


    public float skyboxTransition = 5f;




    [Header("Fyre Skybox")]
    /// <summary>
    /// Fyre lighting settings
    /// </summary>
    public Color fyreFogColor;
    public float fyreFogIntensity;
    // Skybox settings below
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;

    public Color cloudsColor;

    public float horizontolWidth;
    public float cloudsScale;
    private float sunDefaultRadius = 0.356f;
    private float sunGoneRadius = 0f;

    public Color color5;
    public Color color6;
    public Color color7;
    public Color color8;

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
        StartCoroutine(LerpColor(floraColor, flurryColor));
    }

    public void FloraToFyre()
    {
        StartCoroutine(ChangeSkyBox("fyre"));
    }


    private IEnumerator ChangeSkyBox(string goingWhere)
    {
        if (goingWhere == "fyre")
        {
            // All colors
            Color currentColor1 = skyBox.GetColor("_daySkyColorTop");
            Color currentColor2 = skyBox.GetColor("_daySkyColorBottom");
            Color currentColor3 = skyBox.GetColor("_nightSkyColorTop");
            Color currentColor4 = skyBox.GetColor("nightSkyColorBottom");

            Color currentColor5 = skyBox.GetColor("_dayCloudsEdge");
            Color currentColor6 = skyBox.GetColor("_dayCloudsMain");
            Color currentColor7 = skyBox.GetColor("_nightCloudsMain");
            Color currentColor8 = skyBox.GetColor("_nightCloudsEdge");

            // Getting cloud values         
            float currentBaseNoiseScale = skyBox.GetFloat("_baseNoiseScale");
            float currentCloudsPrimaryScale = skyBox.GetFloat("_cloudsPrimaryScale");
            float currentCloudsSecondaryScale = skyBox.GetFloat("_cloudsSecondaryScale");

            // Getting current horizontal values
            float currentHorizontalRadius = skyBox.GetFloat("_HorizontalWidth");
            Color currentHorizontalColor = skyBox.GetColor("_horizonColor");

            // Getting sun radius
            float currentSunRadius = skyBox.GetFloat("_sunRadius");


            float timeElapsedd = 0f;

            while (timeElapsedd < skyboxTransition)
            {
                yield return new WaitForSeconds(0.0000000000001f);
                timeElapsedd += Time.deltaTime / skyboxTransition;
                float t = Mathf.Clamp01(timeElapsedd / skyboxTransition);

         


                // Lerping colors
                Color newColor1 = Color.Lerp(currentColor1, color1, t);
                Color newColor2 = Color.Lerp(currentColor2, color2, t);
                Color newColor3 = Color.Lerp(currentColor3, color3, t);
                Color newColor4 = Color.Lerp(currentColor4, color4, t);

                Color newColor5 = Color.Lerp(currentColor5, color5, t);
                Color newColor6 = Color.Lerp(currentColor6, color6, t);
                Color newColor7 = Color.Lerp(currentColor7, color7, t);
                Color newColor8 = Color.Lerp(currentColor8, color8, t);

                // Lerping sun
                float sunRadius = Mathf.Lerp(sunDefaultRadius, sunGoneRadius, t);

                // Lerping clouds
                float currentCloudScale = Mathf.Lerp(currentBaseNoiseScale, 0, t);
                float currentCloudPrimary = Mathf.Lerp(currentCloudsPrimaryScale, 0, t);
                float currentCloudSecondary = Mathf.Lerp(currentCloudsSecondaryScale, 0, t);




                // Setting colors
                skyBox.SetColor("_daySkyColorTop", newColor1);
                skyBox.SetColor("_daySkyColorBottom", newColor2);
                skyBox.SetColor("_nightSkyColorTop", newColor3);
                skyBox.SetColor("nightSkyColorBottom", newColor4);

                skyBox.SetColor("_dayCloudsEdge", newColor5);
                skyBox.SetColor("_dayCloudsMain", newColor6);
                skyBox.SetColor("_nightCloudsMain", newColor7);
                skyBox.SetColor("_nightCloudsEdge", newColor8);

                // Setting Sun
                skyBox.SetFloat("_sunRadius", sunRadius);

                // Setting clouds
                skyBox.SetFloat("_baseNoiseScale", currentBaseNoiseScale);
                skyBox.SetFloat("_cloudsPrimaryScale", currentCloudsPrimaryScale);
                skyBox.SetFloat("_cloudsSecondaryScale", currentCloudsSecondaryScale);
            }
        }
        yield return null;
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


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            //FloraToFyre();
            StartCoroutine(ChangeSkyBox("fyre"));
        }
    }
}
