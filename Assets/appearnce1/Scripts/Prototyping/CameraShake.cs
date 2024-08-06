using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource screenShake;
    [SerializeField] float shakePower = 10;

    private void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            Debug.Log("Shake");
            ScreenShake();
        }
    }
    public void ScreenShake()
    {
        screenShake.GenerateImpulseWithForce(shakePower);
    }
}
