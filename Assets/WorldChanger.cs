using UnityEditor;
using UnityEditor.ShaderGraph;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Collections;

public class WorldChanger : MonoBehaviour
{
    public GameObject controller;
    public Vector3 targetScale = new Vector3(2f, 2f, 2f); // The scale you want to reach
    public float duration = 2f; // The duration over which the scale change will occur

    private Vector3 initialScale;
    private float timeElapsed;

    /// <summary>
    /// Textures for nodes in shader graph to control environmental blending
    /// </summary>
    public Texture floraTexture; 
    public Texture flurryTexture;
    public Texture fyreTexture;


    public void Start()
    {
        initialScale = transform.localScale; // Store the initial scale
        StartCoroutine(Grow()); 
    }

    public IEnumerator Grow()
    {
        timeElapsed = 0f;
         
        while(timeElapsed < duration)
        {
            controller.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the target scale is set at the end of the duration
        controller.transform.localScale = targetScale;
    }

    public void SwapNodes()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
