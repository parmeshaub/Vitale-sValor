using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireImpAnimationEvents : MonoBehaviour
{
    [SerializeField] FireImpEnemy fireImp;
    [SerializeField] EnemyThrowAttack enemyThrowAttack;

    public void Throw() {
        enemyThrowAttack.Throw(fireImp.playerObject);
    }
}
