using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformActivation : MonoBehaviour
{
    public Animator animator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Activate");
        }
    }
}
