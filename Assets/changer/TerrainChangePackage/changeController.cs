using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeController : MonoBehaviour
{
    private Material mat;
    private GameObject playerInfoObj;

    private Vector3 oldPlayerPosition;
    private Vector3 oldPlayerSize;

    private bool runProcess = false;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        playerInfoObj = GameObject.Find("Controller");

        if (playerInfoObj != null)
        {
            runProcess = true;
            SetPlayerPosition();
            SetPlayerSize();
        }
    }

    /// <summary>
    /// Checks if there is any movement or changes with sphere's transform
    /// </summary>
    private void Update()
    {
        if (runProcess) 
        {
            if (playerInfoObj.transform.position != oldPlayerPosition)
            {
                SetPlayerPosition();
            }

            if (playerInfoObj.transform.localScale != oldPlayerSize)
            {
                SetPlayerSize();
            }
        }
    }

    /// <summary>
    /// Updates position with where the sphere is in local space
    /// </summary>
    public void SetPlayerPosition()
    {
        if (playerInfoObj != null)
        {
            mat.SetVector("_playerPos", playerInfoObj.transform.position);
            oldPlayerPosition = playerInfoObj.transform.position;
        }
    }

    /// <summary>
    /// Adjusts accordingly with the size of the growing sphere
    /// </summary>
    public void SetPlayerSize()
    {
        if (playerInfoObj != null)
        {
            float dist = (playerInfoObj.transform.localScale.x / 2); // 2 radius is the length of the sphere
            mat.SetFloat("_radius", dist);
            oldPlayerSize = playerInfoObj.transform.localScale;
        }
    }
}
