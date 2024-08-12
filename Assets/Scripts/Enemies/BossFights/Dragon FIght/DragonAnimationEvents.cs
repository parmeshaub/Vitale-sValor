using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAnimationEvents : MonoBehaviour
{
    [SerializeField] private GameObject dragonBreath;
    [SerializeField] private Animator animator;

    public void TurnOnBreath() {
        animator.SetBool("isIceBreath", true);
    }

    public void TurnOffBreath() {
        animator.SetBool("isIceBreath", false);
    }
}
