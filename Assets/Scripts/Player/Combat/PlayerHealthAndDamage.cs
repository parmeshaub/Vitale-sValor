using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerHealthAndDamage : MonoBehaviour
{
    [SerializeField] private float currentPlayerHealth = 100;
    [SerializeField] private float maxPlayerHealth = 100;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;

    private void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        SetMaxHealth();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.O))
        {
            TakeDamage(45);
        }
    }

    private void TakeDamage(int damageDealt)
    {
        //Deal Damage
        currentPlayerHealth -= damageDealt;

        //Check if Death
        CheckDeath();

        //Update the UI
        SetHealthUI();
    }

    private void CheckDeath()
    {
        //if player is alive
        if (currentPlayerHealth > 0) return;

        Debug.Log("PlayerDeath");
    }

    private void SetMaxHealth()
    {
        currentPlayerHealth = maxPlayerHealth;
        healthSlider.value = currentPlayerHealth;
    }

    private void SetHealthUI()
    {
        float calcHealth = Mathf.Lerp(0, 1, currentPlayerHealth / maxPlayerHealth);
        Debug.Log(calcHealth);
        healthSlider.value = calcHealth;

    }
}
