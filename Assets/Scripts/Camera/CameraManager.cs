using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Name: Lee Zhi Hui, Shaun
/// Description: This class holds methods I can use to manage the ingame Camera.
/// </summary>
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private GameObject thirdPersonCamera;
    [SerializeField] private Cinemachine.CinemachineBrain cinemachineBrain;
    [SerializeField] private GameObject magicCamera;
    private CinemachineFreeLook thirdpersoncam;

    private bool magicCameraBool = false;

    private void Awake() {
        cinemachineBrain = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        instance = this;
    }

    private void Start() {
        thirdpersoncam = thirdPersonCamera.GetComponent<CinemachineFreeLook>();
    }

    public void FreezeCamera() {
        cinemachineBrain.enabled = false;
    }

    public void UnfreezeCamera() {
        cinemachineBrain.enabled = true;
    }

    public void TurnOffThirdPersonCamera() {
        thirdPersonCamera.SetActive(false);
    }

    public void TurnOnThirdPersonCamera() {
        thirdPersonCamera.SetActive(true);
    }
    public void TurnOnMagicCamera() {
        magicCamera.SetActive(true);
        TurnOffThirdPersonCamera();
    }

    public void TurnOffMagicCamera() {
        magicCamera.SetActive(false);
        TurnOnThirdPersonCamera();
    }

    public void CantMoveCamera() {
        if (thirdpersoncam != null) {
            thirdpersoncam.m_XAxis.m_MaxSpeed = 0f;
            thirdpersoncam.m_YAxis.m_MaxSpeed = 0f;
        }
        else {
            Debug.LogError("Third person camera is not assigned or not found.");
        }
    }

    public void EnableCameraMovement() {
        if (thirdpersoncam != null) {
            thirdpersoncam.m_XAxis.m_MaxSpeed = 300f; // Set to your desired speed
            thirdpersoncam.m_YAxis.m_MaxSpeed = 2f;  // Set to your desired speed
        }
        else {
            Debug.LogError("Third person camera is not assigned or not found.");
        }
    }
}
