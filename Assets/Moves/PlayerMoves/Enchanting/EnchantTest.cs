using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnchantTest : MonoBehaviour
{
    public VisualEffect particlesEffect;

    public Gradient iceGradient;
    public Gradient fireGradient;

    private bool iceBounce = true;
    private bool fireBounce = true;

    [SerializeField] private GameObject animatorObj;

    private void Start() {
        animatorObj = this.gameObject;
        animatorObj.SetActive(false);
    }

    public void EnchantFireSword() {
        //animatorObj.SetActive(true);
        StartCoroutine(EnchantingFireSword());
        fireBounce = false;
    }

    public void DisenchantFireSword() {
        StartCoroutine(DechantingFireSword());
        fireBounce = true;
    }

    public void EnchantIceSword() {
        //animatorObj.SetActive(true);
        StartCoroutine(EnchantingIceSword());
        iceBounce = false;
    }

    public void DisenchantIceSword() {
        StartCoroutine(DechantingIceSword());
        iceBounce = true;
    }


    public IEnumerator EnchantingIceSword()
    {
        particlesEffect.SetGradient("ColorGradient", iceGradient);

        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("enchantingIce");
        yield return new WaitForSeconds(1f);
        particlesEffect.enabled = true;
    }

    public IEnumerator DechantingIceSword()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("dechantingIce");
        yield return new WaitForSeconds(1f);
        particlesEffect.enabled = false;
        //animatorObj.SetActive(false);
    }

    public IEnumerator EnchantingFireSword()
    {
        particlesEffect.SetGradient("ColorGradient", fireGradient);


        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("enchantingFire");
        yield return new WaitForSeconds(1f);
        particlesEffect.enabled = true;
    }

    public IEnumerator DechantingFireSword()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("dechantingFire");
        yield return new WaitForSeconds(1f);
        particlesEffect.enabled = false;
        //animatorObj.SetActive(false) ;
    }
}
