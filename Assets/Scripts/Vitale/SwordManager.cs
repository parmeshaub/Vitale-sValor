using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SwordManager : MonoBehaviour
{
    [SerializeField] private GameObject swordObject;
    [SerializeField] private GameObject vfxgraph1; //Full body
    [SerializeField] private VisualEffect vfx;
    private Animator animator;
    public static SwordManager instance;
    [SerializeField] private PlayerCombat playerCombat;
    private Renderer swordRenderer;

    private static int swordSheathHash = Animator.StringToHash("Sheath");

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        swordRenderer = GetComponent<Renderer>();

        swordRenderer.enabled = false;
        vfx.Stop();
        
        SetVFXSpeed(3.0f);

    }

    public void UnsheathSword()
    {
        swordRenderer.enabled = true;
        vfx.Play();
    }

    public void SheathSword()
    {
        swordRenderer.enabled = false;
        vfx.Play();
    }

    public void UnsheathSwordDuringAttack()
    {
        //Check if player in combat mode
        if(!swordRenderer.enabled)
        {
            swordRenderer.enabled = true;
            vfx.Play();
        }
    }
    public void SetVFXSpeed(float speed)
    {
        // Adjust the speed of the VFX
        if (vfx.HasFloat("Effect Speed"))
        {
            vfx.SetFloat("Effect Speed", speed);
        }

        // If you have other parameters like spawn rate, adjust them similarly
        if (vfx.HasFloat("Spawn Rate"))
        {
            vfx.SetFloat("Spawn Rate", speed);
        }
    }
}
