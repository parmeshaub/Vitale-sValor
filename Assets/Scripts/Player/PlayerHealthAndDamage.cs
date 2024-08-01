using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthAndDamage : MonoBehaviour
{
    [SerializeField] private int currentPlayerHeath= 100;
    [SerializeField] private int maxPlayerHealth = 100;

    private void Start()
    {
        currentPlayerHeath = maxPlayerHealth;
    }

    private void TakeDamage(int damageDealt)
    {

    }

    private void Death()
    {
        if (currentPlayerHeath <= 0)
        {
            
        }
    }
}
