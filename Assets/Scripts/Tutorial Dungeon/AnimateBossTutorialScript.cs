using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBossTutorialScript : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Stand() {
        animator.SetTrigger("Stand");
    }

    public void Point() => animator.SetTrigger("Pointing");
}
