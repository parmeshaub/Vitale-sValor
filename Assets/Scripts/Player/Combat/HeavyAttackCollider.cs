using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider heavyAttackCollider;
    [SerializeField] private PlayerCombat playerCombat;

    [SerializeField] private GameObject normalHitVFXPrefab;

    [SerializeField] private CinemachineImpulseSource impulseSource;
    private Vector3 impulseVelocity = new Vector3(0f, -0.3f, 0f);

    private void Start() {
        heavyAttackCollider.enabled = false; //Make sure that collider is off.
        //impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void OnTriggerEnter(Collider other) {
        // Get the EnemyClass component from the collided object.
        EnemyBase enemyClass = other.GetComponent<EnemyBase>();
        if (enemyClass != null) {
            Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
            Instantiate(normalHitVFXPrefab, contactPoint, Quaternion.identity);

            float damage = 0;

            damage = playerCombat.HeavyRandomizeDamage();

            impulseSource.GenerateImpulse(impulseVelocity);

            enemyClass.TakeDamage(damage);
        }
    }

    public void TurnHeavyAttackColliderOn() {
        heavyAttackCollider.enabled = true;
    }

    public void TurnHeavyAttackColliderOff() {
        heavyAttackCollider.enabled = false;
    }
}