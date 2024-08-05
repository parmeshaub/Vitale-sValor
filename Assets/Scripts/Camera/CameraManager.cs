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

    private void Awake(){
        cinemachineBrain = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        instance = this;
    }

    public void FreezeCamera() {
        cinemachineBrain.enabled = false;
    }

    public void UnfreezeCamera() {
        cinemachineBrain.enabled = true;
    }
    public void TurnOffThirdPersonCamera(){
        thirdPersonCamera.SetActive(false);
    }

    public void TurnOnThirdPersonCamera(){
        thirdPersonCamera.SetActive(true);
    }

    //TODO - Flip FLop Function
    public void SwitchToMagicCamera() {

    }
}
