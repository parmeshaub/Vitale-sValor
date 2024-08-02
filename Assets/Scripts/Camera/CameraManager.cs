using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private GameObject thirdPersonCamera;

    private void Awake()
    {
        instance = this;
    }
    public void TurnOffThirdPersonCamera()
    {
        thirdPersonCamera.SetActive(false);
    }

    public void TurnOnThirdPersonCamera()
    {
        thirdPersonCamera.SetActive(true);
    }
}
