using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager instance;

    // Health and Mana Stats
    public float maxHealth;
    public float maxMana;

    // Combat
    public float maxLightAttackDamage;
    public float minLightAttackDamage;
    public float maxHeavyAttackDamage;
    public float minHeavyAttackDamage;

    //Damage
    public float damageReductionRate;

    private void Awake()
    {
        instance = this;
    }
}
