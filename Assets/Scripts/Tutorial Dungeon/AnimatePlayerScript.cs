using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePlayerScript : MonoBehaviour
{
    public Animator animator;

    public void TriggerCasting() {
        animator.SetTrigger("Ablaze");
    }

    public void Attack1() {
        animator.SetTrigger("Light_Attack_01");
    }

    public void Death() {
        animator.SetTrigger("Death");
    }

    public void End() {

    }
}
