using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlideController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerController controller;
    private CharacterController characterController;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }
}
