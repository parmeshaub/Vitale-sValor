using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCAnimationEventsController : MonoBehaviour
{
    private SwordManager swordManager;
    [SerializeField] private LightAttackCollider lightAttackCollider;
    [SerializeField] private HeavyAttackCollider heavyAttackCollider;

    private void Start()
    {
        swordManager = SwordManager.instance;
    }

    public void Unsheath()
    {
        swordManager.UnsheathSword();
    }

    public void Sheath()
    {
        swordManager.SheathSword();
    }

    public void UnsheathAttack()
    {
        swordManager.UnsheathSwordDuringAttack();
    }

   public void TurnLightAttackOn()
    {
        lightAttackCollider.TurnLightAttackColliderOn();
    }

    public void TurnHeavyAttackOn()
    {
        heavyAttackCollider.TurnHeavyAttackColliderOn();
    }

    public void TurnLightAttackOff()
    {
        lightAttackCollider.TurnLightAttackColliderOff();
    }

    public void TurnHeavyAttackOff()
    {
        heavyAttackCollider.TurnHeavyAttackColliderOff();
    }
}
