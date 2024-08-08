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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (iceBounce)
            {
                StartCoroutine(EnchantingIceSword());
                iceBounce = false;
            }
            else
            {
                StartCoroutine(DechantingIceSword());
                iceBounce = true;
            }     
        }

        if (Input.GetKeyDown(KeyCode.E))
        {

            if (fireBounce)
            {
                StartCoroutine(EnchantingFireSword());
                fireBounce = false;
            }
            else
            {
                StartCoroutine(DechantingFireSword());
                fireBounce = true;
            }
        }

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
    }
}
