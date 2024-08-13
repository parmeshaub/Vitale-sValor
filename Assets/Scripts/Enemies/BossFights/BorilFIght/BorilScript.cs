using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorilScript : MonoBehaviour
{
    public static BorilScript instance;

    //General
    private LayerMask borilLayerMask;
    private BorilPhases currentPhase;
    private BorilStates currentState;
    private Rigidbody rb;
    [SerializeField] private Animator animator;

    //Boril Stats - Health and Damage
    [Header("Boril Stats")]
    [SerializeField] private int maxTimesHit;
    [SerializeField] public float minDamage;
    [SerializeField] public float maxDamage;
    private int currentHits;

    //Boril Stats - Locomotion
    [SerializeField] private float moveSpeed;
    [SerializeField] private float chargeSpeed;

    //Boolean Checks
    private bool isMoving = false;
    private bool isCharging = false;
    private bool canBeAttacked = false;
    private int hitsDuringStun = 6;

    //Animation Hashes
    private static readonly int isMovingHash = Animator.StringToHash("isMoving");
    private static readonly int isChargingHash = Animator.StringToHash("isCharging");
    private static readonly int deathHash = Animator.StringToHash("Death");
    private static readonly int smashHash = Animator.StringToHash("Smash");
    private static readonly int swipeHash = Animator.StringToHash("Swipe");
    private static readonly int chargeHurtHash = Animator.StringToHash("Charge_Hurt");
    private static readonly int iceBreathHash = Animator.StringToHash("Ice_Breath");
    private static readonly int chargeHash = Animator.StringToHash("Charge");

    // Color and Material for visual effects
    [Header("Color and Texture")]
    [SerializeField] private Material borilMaterial;
    [SerializeField] private Material borilPhase2Material;
    [SerializeField] private Material borilPhase3Material;
    [SerializeField] private GameObject borilBody;
    private Renderer borilBodyRenderer;

    [SerializeField] public GameObject portalBack;

    //Coroutine
    private Coroutine currentCoroutine;

    //References
    private GameObject playerObject;
    [SerializeField] private BoxCollider chargeCollider;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        instance = this;
    }

    private void Start() {
        playerObject = GameObject.FindWithTag("Player");
        borilBodyRenderer = borilBody.GetComponent<Renderer>();

        //Ensure the proper material is on Boril.
        borilBodyRenderer.material = borilMaterial;

        //Boril Initialization
        canBeAttacked = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        currentPhase = BorilPhases.Phase_Waiting;
        currentState = BorilStates.Waiting;
        chargeCollider.enabled = false;

        StartBorilFight();
    }

    private void StartBorilFight() {
        currentPhase = BorilPhases.Phase_One;
        currentState = BorilStates.Find_Player;
        TransitionToCoroutine();
    }

    private void TransitionToCoroutine() {
        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
        }

        switch (currentState) {
            case BorilStates.Find_Player:
                currentCoroutine = StartCoroutine(FindPlayerState());
                break;
            case BorilStates.Ice_Breath:
                currentCoroutine = StartCoroutine(IceBreathState());
                break;
            case BorilStates.Boar_Claws_Swipe:
                currentCoroutine = StartCoroutine(BoarClawsSwipeState());
                break;
            case BorilStates.Ground_Smash:
                currentCoroutine = StartCoroutine(GroundSmashState());
                break;
            case BorilStates.Charge:
                currentCoroutine = StartCoroutine(ChargeState());
                break;
            case BorilStates.Stunned:
                currentCoroutine = StartCoroutine(StunnedState());
                break;
            case BorilStates.Dead:
                currentCoroutine = StartCoroutine(DeadState());
                break;
        }
    }

    #region States
    private IEnumerator FindPlayerState() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Debug.Log("FindPlayer State");
        isMoving = true;
        rb.isKinematic = false; // Ensure the Rigidbody is not kinematic

        animator.SetBool(isMovingHash, true);

        while (true) {
            // Find the direction towards the player
            Vector3 directionToPlayer = (playerObject.transform.position - transform.position).normalized;

            // Ignore the Y axis to prevent tilting
            directionToPlayer.y = 0f;

            // Calculate the target position based on the movement speed and time
            Vector3 targetPosition = transform.position + directionToPlayer * moveSpeed * Time.deltaTime * 10f;

            // Rotate Boril to face the player using DOTween
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.DORotateQuaternion(targetRotation, 0.5f); // Smoothly rotate over 0.5 seconds

            // Use MovePosition for smooth movement
            rb.MovePosition(targetPosition);

            // Debugging: Log the current position
            Debug.Log("Current Position: " + transform.position);

            // Check if Boril is close enough to the player to attack
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
            Debug.Log("Distance to Player: " + distanceToPlayer);

            if (distanceToPlayer < 5f) // Example threshold for starting an attack
            {
                // Stop the movement
                rb.velocity = Vector3.zero;

                // Transition to an attack state (e.g., Ice Breath)
                currentState = BorilStates.Ice_Breath;
                TransitionToCoroutine();
                isMoving = false;
                animator.SetBool(isMovingHash, isMoving);
                yield break; // Exit the FindPlayerState coroutine
            }

            yield return null; // Wait until the next frame
        }
    }

    private IEnumerator IceBreathState() {
        // Logic for Ice Breath attack
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        animator.SetTrigger(iceBreathHash);
        yield return new WaitForSeconds(5); // Wait for attack and cooldown.
        currentState = BorilStates.Boar_Claws_Swipe;
        TransitionToCoroutine(); // Return to waiting state
    }

    private IEnumerator BoarClawsSwipeState() {
        // Logic for Boar Claws Swipe attack
        int count = 3;
        int currentCount = 0;

        while (currentCount < count) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            // Find the direction towards the player
            Vector3 directionToPlayer = (playerObject.transform.position - transform.position).normalized;
            directionToPlayer.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            transform.DORotateQuaternion(targetRotation, 0.1f); // Smoothly rotate over 0.5 seconds
            animator.SetTrigger(swipeHash);
            yield return new WaitForSeconds(3);
            currentCount++;
        }

        yield return new WaitForSeconds(1f); // Example delay for the attack

        //Check Phase.
        if(currentPhase == BorilPhases.Phase_One) {
            currentState = BorilStates.Charge;
        }
        else {
            currentState = BorilStates.Ground_Smash;
        }
        TransitionToCoroutine();
    }

    private IEnumerator GroundSmashState() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Debug.Log("FindPlayer State");
        isMoving = true;
        rb.isKinematic = false; // Ensure the Rigidbody is not kinematic

        animator.SetBool(isMovingHash, true);

        while (true) {
            // Find the direction towards the player
            Vector3 directionToPlayer = (playerObject.transform.position - transform.position).normalized;

            // Ignore the Y axis to prevent tilting
            directionToPlayer.y = 0f;

            // Calculate the target position based on the movement speed and time
            Vector3 targetPosition = transform.position + directionToPlayer * moveSpeed * Time.deltaTime * 10f;

            // Rotate Boril to face the player using DOTween
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.DORotateQuaternion(targetRotation, 0.5f); // Smoothly rotate over 0.5 seconds

            // Use MovePosition for smooth movement
            rb.MovePosition(targetPosition);

            // Debugging: Log the current position
            Debug.Log("Current Position: " + transform.position);

            // Check if Boril is close enough to the player to attack
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);
            Debug.Log("Distance to Player: " + distanceToPlayer);

            if (distanceToPlayer < 5f) // Example threshold for starting an attack
            {
                // Stop the movement
                rb.velocity = Vector3.zero;
                isMoving = false;
                animator.SetBool(isMovingHash, isMoving);

                // Transition to an attack state (e.g., Ice Breath)
                animator.SetTrigger(smashHash);
                yield return new WaitForSeconds(6f);

                currentState = BorilStates.Charge;
                TransitionToCoroutine();
                
                yield break; // Exit the FindPlayerState coroutine
            }

            yield return null; // Wait until the next frame
        }
    }

    private IEnumerator ChargeState() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Debug.Log("Charge State");
        isCharging = true;
        rb.isKinematic = false;

        animator.SetBool(isChargingHash, isCharging);
        chargeCollider.enabled = true;

        // Get the player's position at the start of the charge
        Vector3 playerPosition = playerObject.transform.position;

        // Calculate the direction towards the player's position
        Vector3 chargeDirection = (playerPosition - transform.position).normalized;

        // Ignore the Y axis to prevent tilting during the charge
        chargeDirection.y = 0f;

        // Rotate Boril to face the charge direction using DOTween
        Quaternion targetRotation = Quaternion.LookRotation(chargeDirection);
        transform.DORotateQuaternion(targetRotation, 0.5f); // Smoothly rotate over 0.5 seconds

        float chargeDuration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < chargeDuration) {
            // Calculate the target position for this frame
            Vector3 targetPosition = transform.position + chargeDirection * chargeSpeed * 10 * Time.deltaTime;

            // Move Boril to the target position
            rb.MovePosition(targetPosition);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Charge complete, reset state
        Debug.Log("charge complete");
        chargeCollider.enabled = false;
        rb.velocity = Vector3.zero;
        isCharging = false;
        animator.SetBool(isChargingHash, isCharging);
        currentState = BorilStates.Find_Player;
        TransitionToCoroutine(); // Move to the next state
    }



    private IEnumerator StunnedState() {
        Debug.Log("stunned");

        hitsDuringStun = 0;
        // Change the object's layer to "Enemy"
        gameObject.layer = LayerMask.NameToLayer("Enemy");

        animator.SetBool(isChargingHash, isCharging);
        canBeAttacked = true;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        yield return new WaitForSeconds(8f); // Stunned duration
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        canBeAttacked = false;
        currentState = BorilStates.Find_Player;

        // Change the object's layer back to "Default"
        gameObject.layer = LayerMask.NameToLayer("Default");

        // Notify the BattleSphereDetection to remove the boss from the target list
        PlayerCombat.Instance.battleSphereDetection.RemoveEnemy(gameObject);

        TransitionToCoroutine();
    }


    /// <summary>
    /// What happens the boar dies
    /// </summary>
    private IEnumerator DeadState() {
        animator.SetTrigger(deathHash);

        portalBack.SetActive(true);

        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
    #endregion

    private void OnCollisionEnter(Collision collision) {
        if (isCharging && collision.gameObject.CompareTag("Pillar")) {
            Debug.Log("Collided with Pillar");

            // Handle the pillar collision
            HandlePillarCollision();

            // Get the BorilPillarScript component
            BorilPillarScript pillar = collision.gameObject.GetComponent<BorilPillarScript>();

            if (pillar != null) {
                // Calculate the direction of the hit
                Vector3 hitDirection = (collision.transform.position - transform.position).normalized;

                // Call PillarHit with the calculated direction
                pillar.PillarHit(hitDirection);
            }
        }
    }

    private void HandlePillarCollision() {
        // Stop the charge
        StopCoroutine(currentCoroutine);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Handle what happens when the boss collides with a pillar (e.g., take damage, stop charging)
        Debug.Log("Charge interrupted by pillar.");

        // Disable the charge collider and reset the charge state
        chargeCollider.enabled = false;
        isCharging = false;
        animator.SetTrigger(chargeHurtHash);

        // Transition to the next state if needed
        currentState = BorilStates.Stunned;
        TransitionToCoroutine();
    }

    public void Damage() {
        Debug.Log("hit");

        hitsDuringStun++;  // Increment the hit count for this phase
        Debug.Log($"Hit {hitsDuringStun}/3 during stunned phase");

        // Handle damage and phase transitions (this can be the same as your existing logic)
        if (hitsDuringStun == 3) {
            // After 3 hits, make Boril invulnerable again
            Debug.Log("Boril is now invulnerable until the next stunned phase.");
            currentState = BorilStates.Find_Player;

            // Change the object's layer back to "Default"
            gameObject.layer = LayerMask.NameToLayer("Default");

            // Notify the BattleSphereDetection to remove the boss from the target list
            PlayerCombat.Instance.battleSphereDetection.RemoveEnemy(gameObject);

            TransitionToCoroutine();
        }
        currentHits++;

        if (currentHits >= 3 && currentHits <= 4) {
            currentPhase = BorilPhases.Phase_Two;
            borilBodyRenderer.material = borilPhase2Material;
        }
        else if (currentHits > 4 && currentHits < 6) {
            // Handle logic for when currentHits is greater than 4
            currentPhase = BorilPhases.Phase_Three; // Example: Transition to Phase Three
            borilBodyRenderer.material = borilPhase3Material;
        }
        else if (currentHits > maxTimesHit) {
            // Death
            if (currentCoroutine != null) {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null; // Optional: reset after stopping to avoid future issues
            }

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            currentState = BorilStates.Dead;
            TransitionToCoroutine();
        }



    }
}

public enum BorilStates
{
    Waiting,
    Find_Player,
    Ice_Breath,
    Boar_Claws_Swipe,
    Ground_Smash,
    Charge,
    Stunned,
    Dead
}

public enum BorilPhases
{
    Phase_Waiting,
    Phase_One,
    Phase_Two,
    Phase_Three,
    Dead
}
