using System.Collections;
using System.Collections.Generic;
using PathCreation.Examples;
using UnityEngine;
using UnityEngine.VFX;

public class GelidityTest : MonoBehaviour
{
    public VisualEffect gelidtyVFXGraph;
    public GameObject player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            gelidtyVFXGraph.enabled = true;

            Animator animator = player.GetComponent<Animator>();
            animator.SetTrigger("Play");

        }
    }


}
