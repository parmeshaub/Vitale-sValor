using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerHealthAndDamage : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;
    [SerializeField] PlayerSoundHolder soundHolder;
    [SerializeField] private float currentPlayerHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    private float maxPlayerHealth;

    [Header("Heart Break UI")]
    [SerializeField] private GameObject heartbreakSmall;
    [SerializeField] private GameObject heartbreakMedium;
    [SerializeField] private GameObject heartbreakLarge;

    private Animator animator;
    private PlayerInputManager playerInputManager;
    private SwordManager swordManager;
    private CameraManager cameraManager;
    [SerializeField] private ShieldScript shieldManager;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    private PlayerStatsManager playerStatsManager;
    private Vector3 impulseDirection = new Vector3(0, -0.5f, 0);

    [Header("Healing Settings")]
    public bool CanHeal = false; // Public boolean to check if healing is allowed
    public float currentHealRate = 10f; // Public heal rate, modifiable by other scripts

    private bool deathDoOnce = false;

    private static readonly int deathHash = Animator.StringToHash("Death");

    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private RespawnManager respawnManager;

    private PlayerCombat playerCombat;

    private void Start() {
        playerCombat = GetComponent<PlayerCombat>();    
        cameraManager = CameraManager.instance;
        animator = GetComponentInChildren<Animator>();
        playerInputManager = PlayerInputManager.instance;
        swordManager = SwordManager.instance;
        playerStatsManager = PlayerStatsManager.instance;

        InitializeHealth();

        SetMaxToHealth();
        InitializeHeartBreak();
        deathDoOnce = false;
    }

    private void Update() {
        // Automatically heal the player if CanHeal is true
        if (CanHeal) {
            HealPlayer();
        }
    }

    public void TakeDamage(float damageDealt) {
        // Deal Damage
        if (playerCombat.isBlocking) {
            damageDealt /= playerStatsManager.damageReductionRate;
            currentPlayerHealth -= damageDealt;
            impulseSource.GenerateImpulse(impulseDirection);
        }
        else {
            currentPlayerHealth -= damageDealt;
            impulseSource.GenerateImpulse(impulseDirection);
        }
        

        // Play a sound 1 out of 4 times
        int randomChance = Random.Range(0, 4);
        switch (randomChance) {
            case 0:
                soundManager.PlaySFX(soundHolder.hurt_01);
                break;
            case 1:
                soundManager.PlaySFX(soundHolder.hurt_02);
                break;
            case 2:
                soundManager.PlaySFX(soundHolder.hurt_03);
                break;
            case 3:
                soundManager.PlaySFX(soundHolder.hurt_04);
                break;
        }

        // Check if Death
        CheckDeath();

        // Update the UI
        SetHealthUI();
    }

    private void CheckDeath() {
        // if player is alive
        if (currentPlayerHealth > 0) return;

        // Death
        playerInputManager.playerInput.Gameplay.Disable();
        if (!deathDoOnce) {
            animator.SetTrigger(deathHash);
            swordManager.SheathSword();
            shieldManager.TakeInShield();
            soundManager.PlaySFX(soundHolder.death);
            StartCoroutine(RespawnAfterWait());
            deathDoOnce = true;
        }
    }

    private IEnumerator RespawnAfterWait() {
        // Wait for black fadeout animation?
        yield return new WaitForSeconds(4);
        fadeManager.StartFadeIn(1.8f);
        yield return new WaitForSeconds(2);
        respawnManager.RespawnPlayer();
    }

    private void SetMaxToHealth() {
        currentPlayerHealth = playerStatsManager.maxHealth; ;
        healthSlider.value = currentPlayerHealth;
    }

    private void SetHealthUI() {
        float calcHealth = Mathf.Lerp(0, 1, currentPlayerHealth / maxPlayerHealth);
        healthSlider.value = calcHealth;

        // Player Health 100% - 75%
        if (currentPlayerHealth >= maxPlayerHealth * 0.75) {
            InitializeHeartBreak();
        }
        // Player Health 50% - 75%
        else if (currentPlayerHealth >= maxPlayerHealth * 0.5 && currentPlayerHealth < maxPlayerHealth * 0.75) {
            heartbreakSmall.SetActive(true);
            heartbreakMedium.SetActive(false);
            heartbreakLarge.SetActive(false);
        }
        // Player Health 25% - 50%
        else if (currentPlayerHealth >= maxPlayerHealth * 0.25 && currentPlayerHealth < maxPlayerHealth * 0.5) {
            heartbreakSmall.SetActive(false);
            heartbreakMedium.SetActive(true);
            heartbreakLarge.SetActive(false);
        }
        // Player Health 0% - 25%
        else if (currentPlayerHealth < maxPlayerHealth * 0.25) {
            heartbreakSmall.SetActive(false);
            heartbreakMedium.SetActive(false);
            heartbreakLarge.SetActive(true);
        }
    }

    private void InitializeHeartBreak() {
        heartbreakSmall.SetActive(false);
        heartbreakMedium.SetActive(false);
        heartbreakLarge.SetActive(false);
    }

    private void HealPlayer() {
        Debug.Log(currentPlayerHealth);
        // Check if player health is not full
        if (currentPlayerHealth < maxPlayerHealth) {
            // Heal the player based on the current heal rate
            currentPlayerHealth += currentHealRate * Time.deltaTime * 10;

            // Update the health UI
            SetHealthUI();
        }
        else {
            CanHeal = false;
            currentPlayerHealth = maxPlayerHealth;
            SetHealthUI();
        }
    }

    public void InitializeHealth() {
        maxPlayerHealth = playerStatsManager.maxHealth;
        currentPlayerHealth = maxPlayerHealth;
        SetHealthUI();
        deathDoOnce = false;
    }
}
