using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
/// <summary>
/// Name: Lee Zhi Hui, Shaun
/// Description: This Script handles the animation events.
/// Allowed to be called during animations.
/// </summary>
public class MCAnimationEventsController : MonoBehaviour
{
    private SwordManager swordManager;
    [SerializeField] private LightAttackCollider lightAttackCollider;
    [SerializeField] private HeavyAttackCollider heavyAttackCollider;
    [SerializeField] private ShieldScript shieldScript;
    [SerializeField] private PlayerCombat playerCombat;

    private void Start()
    {
        swordManager = SwordManager.instance;
    }

    public void Unsheath()
    {
        swordManager.UnsheathSword();
    }

    public void Sheath() => swordManager.SheathSword();
    public void UnsheathAttack() => swordManager.UnsheathSwordDuringAttack();
    public void TurnLightAttackOn() => lightAttackCollider.TurnLightAttackColliderOn();
    public void TurnHeavyAttackOn() => heavyAttackCollider.TurnHeavyAttackColliderOn();
    public void TurnLightAttackOff() => lightAttackCollider.TurnLightAttackColliderOff();
    public void TurnHeavyAttackOff() => heavyAttackCollider.TurnHeavyAttackColliderOff();
    public void TakeOutShield() => shieldScript.TakeOutShield();
    public void KeepShield() => shieldScript.TakeInShield();
    public void SetAttackingBoolTrue() => playerCombat.SetIsAttackingTrue();
    public void SetAttackingBoolFalse() => playerCombat.SetIsAttackingFalse();
    public void TurnShieldAttackOn() => shieldScript.EnableShieldAttackCollider();
    public void TurnShieldAttackOff() => shieldScript.DisableShieldAttackCollider();
}
