using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttackCollider : MonoBehaviour
{
    [SerializeField] private BoxCollider lightAttackCollider;
    [SerializeField] private PlayerCombat playerCombat;

    [SerializeField] private GameObject normalHitVFXPrefab;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    private Vector3 impulseVelocity = new Vector3(0f, -0.1f, 0f);

    private void Start()
    {
        lightAttackCollider.enabled = false; //Make sure that collider is off.
        //impulseSource = GetComponent<CinemachineImpulseSource>();  
    }
    private void OnTriggerEnter(Collider other) {
        //Debug.Log("run");
        // Get the EnemyClass component from the collided object.
        EnemyBase enemyClass = other.GetComponent<EnemyBase>();

        if (enemyClass != null) {

            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Vector3 directionToPlayer = (transform.position - contactPoint).normalized;
            float offsetDistance = 0.2f; // Adjust this value as needed
            Vector3 offsetContactPoint = contactPoint + directionToPlayer * offsetDistance;

            if (normalHitVFXPrefab != null) {
                Instantiate(normalHitVFXPrefab, offsetContactPoint, Quaternion.identity);
            }

            float damage = playerCombat.LightRandomizeDamage();
            enemyClass.TakeDamage(damage);
            impulseSource.GenerateImpulse(impulseVelocity);
        }
    }



    public void TurnLightAttackColliderOn()
    {
        lightAttackCollider.enabled = true;
    }

    public void TurnLightAttackColliderOff()
    {
        lightAttackCollider.enabled = false;
    }
}
