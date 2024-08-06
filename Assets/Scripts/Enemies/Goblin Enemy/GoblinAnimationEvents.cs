using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to handle all animation events for goblin.
/// </summary>
public class GoblinAnimationEvents : MonoBehaviour
{
    [SerializeField] private EnemyAttack enemyAttack;

    public void TurnOnAttackCollider() => enemyAttack.TurnAttackColliderOn();
    public void TurnOffAttackCollider() => enemyAttack.TurnAttackColliderOff();
}
