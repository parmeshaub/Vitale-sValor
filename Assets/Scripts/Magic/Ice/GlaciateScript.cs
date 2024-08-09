using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magic Move", menuName = "Magic Move/Glaciate")]
public class GlaciateScript : MagicMoveSO
{

    [SerializeField] private EnchantTest enchantScript;
    private PlayerCombat playerCombat;
    private SwordManager swordManager;
    private PlayerStatsManager playerStatsManager;

    private float originalMinLightDamage;
    private float originalMaxLightDamage;
    private float originalMinHeavyDamage;
    private float originalMaxHeavyDamage;

    public override void Activate() {
        playerCombat = PlayerCombat.Instance;
        playerCombat.CheckCombatMode();
        playerCombat.animator.SetTrigger("Ablaze");
        playerCombat.isAttacking = true;
        playerCombat.isEnchanted = true;
        playerStatsManager = PlayerStatsManager.instance;
        if (!playerCombat.inCombatMode) return;
        MagicCoroutineHelper.Instance.StartCoroutine(ActivateMagic());
    }

    public override void Cast() {
        throw new System.NotImplementedException();
    }

    private IEnumerator ActivateMagic() {
        yield return new WaitForSeconds(0.1f);
        if (enchantScript == null) {
            enchantScript = FindAnyObjectByType<EnchantTest>();
        }

        if (enchantScript != null) {
            enchantScript.EnchantIceSword();

            originalMaxHeavyDamage = playerStatsManager.maxHeavyAttackDamage;
            playerStatsManager.minHeavyAttackDamage *= 1.8f;
            originalMinHeavyDamage = playerStatsManager.minHeavyAttackDamage;
            playerStatsManager.minHeavyAttackDamage *= 1.8f;
            originalMaxLightDamage = playerStatsManager.maxLightAttackDamage;
            playerStatsManager.maxLightAttackDamage *= 1.8f;
            originalMinLightDamage = playerStatsManager.minLightAttackDamage;
            playerStatsManager.minLightAttackDamage *= 1.8f;
        }
        else {
            Debug.LogError("EnchantScript is not found!");
        }

        yield return new WaitForSeconds(1);
        playerCombat.isAttacking = false;

        yield return new WaitForSeconds(19);

        if (enchantScript != null) {
            enchantScript.DisenchantIceSword();
        }
        playerCombat.isEnchanted = false;
        playerStatsManager.minLightAttackDamage = originalMinLightDamage;
        playerStatsManager.maxLightAttackDamage = originalMaxLightDamage;
        playerStatsManager.minHeavyAttackDamage = originalMinHeavyDamage;
        playerStatsManager.maxHeavyAttackDamage = originalMaxHeavyDamage;
    }

}
