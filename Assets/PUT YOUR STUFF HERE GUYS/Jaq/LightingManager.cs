using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightingManager : MonoBehaviour
{
    public Light directionalLight; // Assign your directional light in the inspector
    public Material skyBox;


    public float skyboxTransition = 5f;

    private float sunDefaultRadius = 0.356f;
    private float sunGoneRadius = 0f;


    [Header("Fyre Skybox")]
    /// <summary>
    /// Fyre lighting settings
    /// </summary>
    public Color fyreFogColor;
    public float fyreFogIntensity;
    // Skybox settings below
    public Color fyreColor1;
    public Color fyreColor2;
    public Color fyreColor3;
    public Color fyreColor4;

    public float fyreHorizontolWidth;
    public float fyreHorizontolBrightness;
    public Color fyreHorizontolColor;
    public float fyreCloudsScale;

    public Color fyreColor5;
    public Color fyreColor6;
    public Color fyreColor7;
    public Color fyreColor8;


    [Header("Flora Skybox")]
    /// <summary>
    /// Flora Lighting Settings
    /// </summary>
    public Color floraFogColor;
    public float floraFogIntensity;
    // Skybox settings below
    public Color floraColor1;
    public Color floraColor2;
    public Color floraColor3;
    public Color floraColor4;

    public float floraHorizontolWidth;
    public float floraHorizontolBrightness;
    public Color floraHorizontolColor;
    public float floraCloudsScale;

    public Color floraColor5;
    public Color floraColor6;
    public Color floraColor7;
    public Color floraColor8;

    [Header("Flurry Skybox")]
    /// <summary>
    /// Flora Lighting Settings
    /// </summary>
    public Color flurryFogColor;
    public float flurryFogIntensity;
    // Skybox settings below
    public Color flurryColor1;
    public Color flurryColor2;
    public Color flurryColor3;
    public Color flurryColor4;

    public float flurryHorizontolWidth;
    public float flurryHorizontolBrightness;
    public Color flurryHorizontolColor;
    public float flurryCloudsScale;

    public Color flurryColor5;
    public Color flurryColor6;
    public Color flurryColor7;
    public Color flurryColor8;


    /// <summary>
    /// For directional light color lerp
    /// </summary>
    public Color directionalFloraLighting;
    public float directionalFloralLightingIntensity;
    public Color directionalFlurryLighting;
    public float directionalFlurryLightingIntensity;
    public Color directionalFyreLighting;
    public float directionalFyreLightingIntensity;

    public float changeDuraiton;


    public void Start()
    {
        lightingRecognition();
    }
    
    /// <summary>
    /// To store info
    /// </summary>
    private void lightingRecognition()
    {
        directionalFloraLighting = directionalLight.color;
    }

    /// <summary>
    /// Changing directional lighting influence, fog and skybox
    /// </summary>
    public void FloraToFlurry()
    {
        StartCoroutine(LerpDirectionalColor(directionalFloraLighting, directionalFlurryLighting, directionalFloralLightingIntensity, directionalFlurryLightingIntensity));
        StartCoroutine(LerpFogColor(floraFogColor, flurryFogColor, floraFogIntensity, flurryFogIntensity));
        StartCoroutine(ChangeSkyBox("flurry"));
    }

    /// <summary>
    /// Changing directional lighting influence, fog and skybox
    /// </summary>
    public void FloraToFyre()
    {
        StartCoroutine(LerpDirectionalColor(directionalFloraLighting, directionalFyreLighting, directionalFloralLightingIntensity,directionalFyreLightingIntensity));
        StartCoroutine(LerpFogColor(floraFogColor, fyreFogColor, floraFogIntensity, fyreFogIntensity));
        StartCoroutine(ChangeSkyBox("fyre"));
    }

    /// <summary>
    /// Getting current skybox information and making it lerp to desired dimension
    /// </summary>
    public IEnumerator ChangeSkyBox(string goingWhere)
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
        float gottenHorizontalWidth = skyBox.GetFloat("_HorizontalWidth");
        float gottenHorizontalBrightness = skyBox.GetFloat("_horizonBrightness");
        Color gottenHorizontalColor = skyBox.GetColor("_horizonColor");

        // Getting sun radius
        float currentSunRadius = skyBox.GetFloat("_sunRadius");


        float timeElapsedd = 0f;

        while (timeElapsedd < skyboxTransition)
        {
            yield return new WaitForSeconds(0.0000000000001f);
            timeElapsedd += Time.deltaTime / skyboxTransition;
            float t = Mathf.Clamp01(timeElapsedd / skyboxTransition);

            // Creating variables to store lerp colors            
            Color newColor1 = Color.white;
            Color newColor2 = Color.white;
            Color newColor3 = Color.white;
            Color newColor4 = Color.white;
            Color newColor5 = Color.white;
            Color newColor6 = Color.white;
            Color newColor7 = Color.white;
            Color newColor8 = Color.white;

            float sunRadius = 0f;

            Color currentHorizonColor = Color.white;
            float currentHorizonWidth = 0f;
            float currentHorizonBrightness = 0f;

            float currentCloudScale = 0f;
            float currentCloudPrimary = 0f;
            float currentCloudSecondary = 0f;

            if (goingWhere == "fyre")
            {
                // Lerping colors
                newColor1 = Color.Lerp(currentColor1, fyreColor1, t);
                newColor2 = Color.Lerp(currentColor2, fyreColor2, t);
                newColor3 = Color.Lerp(currentColor3, fyreColor3, t);
                newColor4 = Color.Lerp(currentColor4, fyreColor4, t);

                newColor5 = Color.Lerp(currentColor5, fyreColor5, t);
                newColor6 = Color.Lerp(currentColor6, fyreColor6, t);
                newColor7 = Color.Lerp(currentColor7, fyreColor7, t);
                newColor8 = Color.Lerp(currentColor8, fyreColor8, t);

                // Lerping sun
                sunRadius = Mathf.Lerp(sunDefaultRadius, sunGoneRadius, t);

                // Lerping Horizon
                currentHorizonColor = Color.Lerp(gottenHorizontalColor, fyreHorizontolColor, t);
                currentHorizonWidth = Mathf.Lerp(gottenHorizontalWidth, fyreHorizontolWidth, t);
                currentHorizonBrightness = Mathf.Lerp(gottenHorizontalBrightness, fyreHorizontolBrightness, t);

                // Lerping clouds
                currentCloudScale = Mathf.Lerp(currentBaseNoiseScale, 0, t);
                currentCloudPrimary = Mathf.Lerp(currentCloudsPrimaryScale, 0, t);
                currentCloudSecondary = Mathf.Lerp(currentCloudsSecondaryScale, 0, t);
            }


            if (goingWhere == "flurry")
            {
                // Lerping colors
                newColor1 = Color.Lerp(currentColor1, flurryColor1, t);
                newColor2 = Color.Lerp(currentColor2, flurryColor2, t);
                newColor3 = Color.Lerp(currentColor3, flurryColor3, t);
                newColor4 = Color.Lerp(currentColor4, flurryColor4, t);

                newColor5 = Color.Lerp(currentColor5, flurryColor5, t);
                newColor6 = Color.Lerp(currentColor6, flurryColor6, t);
                newColor7 = Color.Lerp(currentColor7, flurryColor7, t);
                newColor8 = Color.Lerp(currentColor8, flurryColor8, t);

                // Lerping sun
                sunRadius = Mathf.Lerp(sunDefaultRadius, sunGoneRadius, t);

                // Lerping Horizon
                currentHorizonColor = Color.Lerp(gottenHorizontalColor, flurryHorizontolColor, t);
                currentHorizonWidth = Mathf.Lerp(gottenHorizontalWidth, flurryHorizontolWidth, t);
                currentHorizonBrightness = Mathf.Lerp(gottenHorizontalBrightness, flurryHorizontolBrightness, t);

                // Lerping clouds
                currentCloudScale = Mathf.Lerp(currentBaseNoiseScale, 0.02f, t);
                currentCloudPrimary = Mathf.Lerp(currentCloudsPrimaryScale, 0.02f, t);
                currentCloudSecondary = Mathf.Lerp(currentCloudsSecondaryScale, 0.02f, t);
            }

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

            // Setting Horizon
            skyBox.SetColor("_horizonColor", currentHorizonColor);
            skyBox.SetFloat("_HorizontalWidth", currentHorizonWidth);
            skyBox.SetFloat("_horizonBrightness", currentHorizonBrightness);

            // Setting clouds
            skyBox.SetFloat("_baseNoiseScale", currentCloudScale);
            skyBox.SetFloat("_cloudsPrimaryScale", currentCloudPrimary);
            skyBox.SetFloat("_cloudsSecondaryScale", currentCloudSecondary);
        }   
        yield return null;
    }

    /// <summary>
    /// Change back skybox settings to default
    /// </summary>
    private void OnDisable()
    {
        // Setting colors
        skyBox.SetColor("_daySkyColorTop", floraColor1);
        skyBox.SetColor("_daySkyColorBottom", floraColor2);
        skyBox.SetColor("_nightSkyColorTop", floraColor3);
        skyBox.SetColor("nightSkyColorBottom", floraColor4);

        skyBox.SetColor("_dayCloudsEdge", floraColor5);
        skyBox.SetColor("_dayCloudsMain", floraColor6);
        skyBox.SetColor("_nightCloudsMain", floraColor7);
        skyBox.SetColor("_nightCloudsEdge", floraColor8);

        // Horizion Settings
        skyBox.SetColor("_horizonColor", floraHorizontolColor);
        skyBox.SetFloat("_HorizontalWidth", floraHorizontolWidth);
        skyBox.SetFloat("_horizonBrightness", floraHorizontolBrightness);

        // Setting Sun
        skyBox.SetFloat("_sunRadius", sunDefaultRadius);

        // Setting clouds
        skyBox.SetFloat("_baseNoiseScale", 0.03f);
        skyBox.SetFloat("_cloudsPrimaryScale", 0.03f);
        skyBox.SetFloat("_cloudsSecondaryScale", 0.03f);
    }


    private IEnumerator LerpDirectionalColor(Color startColor, Color endColor, float startIntensity, float endIntensity)
    {
        float lerpTime = 0f;

        while (lerpTime < 1.0f)
        {
            lerpTime += Time.deltaTime / changeDuraiton;
            directionalLight.color = Color.Lerp(startColor, endColor, lerpTime);
            directionalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, lerpTime);
            yield return null;
        }
    }

    /// <summary>
    /// Changing fog color AND intensity mamamiaaa
    /// </summary>
    private IEnumerator LerpFogColor(Color startColor, Color endColor, float startIntensity, float endIntensity)
    {
        float lerpTime = 0f;

        while (lerpTime < 1.0f)
        {
            lerpTime += Time.deltaTime / changeDuraiton;
            RenderSettings.fogColor = Color.Lerp(startColor, endColor, lerpTime);
            RenderSettings.fogDensity = Mathf.Lerp(startIntensity, endIntensity, lerpTime);
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
            //StartCoroutine(ChangeSkyBox("fyre"));
        }
    }
}
