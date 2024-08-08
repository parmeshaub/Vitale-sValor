using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingRays : MonoBehaviour
{
    public float rotateAmount;
    public float rayApperanceDuraiton = 100f;

    public float rotaterFinalSpeed = 200f;
    private int value = 0;

    private int counterCheck = 0;

    private Transform randomRay;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotateAmount * Time.deltaTime, 0);

        if (Input.GetKeyDown(KeyCode.P))
        {
            CheckRay();
        }
    }

    /// <summary>
    /// Getting a ray and checking if it has been activated
    /// </summary>
    public void CheckRay()
    {
        if (counterCheck != 3)
        {
            int randomIndex = Random.Range(0, transform.childCount);
            randomRay = transform.GetChild(randomIndex);

            Renderer renderer = randomRay.GetComponent<Renderer>();
            Material rayMat = renderer.material;

            float valueCheck = rayMat.GetFloat("_Clip");
            Debug.Log(valueCheck);

            if (valueCheck == 0f)
            {
                CheckRay();
            }
            else
            {
                StartCoroutine(RandomRayApperance());
                counterCheck++;
            }
        }
       
    }

    /// <summary>
    /// Making the ray appear by changing its alpha clip value
    /// </summary>
    /// <returns></returns>
    public IEnumerator RandomRayApperance()
    {
        Renderer renderer = randomRay.GetComponent<Renderer>();
        Material rayMat = renderer.material;

        float elaspedTime = 0f;
        while (elaspedTime < rayApperanceDuraiton)
        {
            yield return new WaitForSeconds(0.001f);

            elaspedTime += Time.deltaTime;
            float transitionValue = Mathf.Clamp01(1 - (elaspedTime / rayApperanceDuraiton));


            rayMat.SetFloat("_Clip", transitionValue);
        }

        rayMat.SetFloat("_Clip", 0);
    }


}
