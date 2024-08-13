using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DragonBoss : MonoBehaviour
{
    public enum DragonState { FlyAround, SwoopAttack, Tired, Death }
    public DragonState currentState;

    public Transform player;
    public Animator animator;

    public Transform centerPoint; // The center of the circular path
    public Transform stageCenter; // The center of the stage where the dragon will fly to blow fire
    public float circleRadius = 20f;
    public float circleSpeed = 5f;

    public float minFlyTime = 5f;
    public float maxFlyTime = 10f;
    public float swoopSpeed = 20f;
    public float tiredSpeed = 5f;
    public int hitsToDie = 3;

    private int hitsTaken = 0;
    private float currentAngle = 0f;

    public bool isDead = false;
    private CinemachineImpulseSource cinemachineImpulse;

    public GameObject portal;

    void Start() {
        // Set the initial position of the dragon at the desired distance from the center point
        transform.position = new Vector3(centerPoint.position.x + circleRadius, transform.position.y, centerPoint.position.z);
        cinemachineImpulse = GetComponent<CinemachineImpulseSource>();

        // Start in FlyAround state
        ChangeState(DragonState.FlyAround);
        
    }

    private void Update() {
        Debug.Log(hitsTaken);
    }

    void ChangeState(DragonState newState) {
        StopAllCoroutines();
        currentState = newState;

        switch (newState) {
            case DragonState.FlyAround:
                StartCoroutine(FlyAround());
                break;
            case DragonState.SwoopAttack:
                StartCoroutine(SwoopAttack());
                break;
            case DragonState.Tired:
                StartCoroutine(MoveToCircularPath()); // Transition to circular path after swoop attack
                break;
            case DragonState.Death:
                Death();
                break;
        }
    }

    IEnumerator FlyAround() {
        float flyTime = Random.Range(minFlyTime, maxFlyTime);
        float timer = 0;

        while (timer < flyTime) {
            // Move the dragon in a circular path
            MoveDragonInCircle();
            timer += Time.deltaTime;
            yield return null;
        }

        // Transition to Swoop Attack state
        ChangeState(DragonState.SwoopAttack);
    }

    IEnumerator SwoopAttack() {
        // Fly to the middle of the stage
        Vector3 targetPosition = stageCenter.position;

        while (Vector3.Distance(transform.position, targetPosition) > 1f) {
            // Move towards the stage center
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, swoopSpeed * Time.deltaTime);

            // Make the dragon look at the stage center while moving
            transform.DOLookAt(stageCenter.position, 0.5f); // Adjust the duration (0.5f) as needed

            yield return null;
        }

        // Face the player
        transform.DOLookAt(player.position, 1f); // Adjust the duration (1f) as needed

        // Play fire-breathing animation
        animator.SetTrigger("Attack");

        // Wait for the fire-breathing animation to finish (adjust duration as necessary)
        yield return new WaitForSeconds(6f);  // Adjust this duration to match the animation

        // After the fire attack, smoothly move back to the circular path
        StartCoroutine(MoveToCircularPath());
    }

    IEnumerator MoveToCircularPath() {
        // Ensure the centerPoint is set
        if (centerPoint == null) {
            Debug.LogError("CenterPoint is not set. Please assign it in the Inspector.");
            yield break;
        }

        // Ensure the dragon's altitude is consistent
        float dragonAltitude = 20f;  // Adjust this to your desired height

        // Calculate the direction from the dragon to the center point
        Vector3 directionToCenter = (transform.position - centerPoint.position).normalized;

        // Calculate the angle of the dragon relative to the circle
        currentAngle = Mathf.Atan2(directionToCenter.z, directionToCenter.x);

        // Calculate the target position on the circular path with the correct altitude
        Vector3 targetPositionOnCircle = centerPoint.position + new Vector3(Mathf.Cos(currentAngle) * circleRadius, dragonAltitude, Mathf.Sin(currentAngle) * circleRadius);

        // Increase the smooth speed to speed up the transition
        float smoothSpeed = 10f;  // Increase this value to make the dragon move faster

        while (Vector3.Distance(transform.position, targetPositionOnCircle) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPositionOnCircle, smoothSpeed * Time.deltaTime);

            // Increase rotation speed for quicker alignment
            Vector3 forwardDirection = (targetPositionOnCircle - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(forwardDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * (smoothSpeed / 2)); // Adjust this factor for desired rotation speed

            yield return null;
        }

        // Once back on the circular path, resume circular movement
        ChangeState(DragonState.FlyAround);
    }


    void MoveDragonInCircle() {
        // Ensure the centerPoint is set
        if (centerPoint == null) {
            Debug.LogError("CenterPoint is not set. Please assign it in the Inspector.");
            return;
        }

        // Define a consistent Y-position (altitude) for the dragon
        float dragonAltitude = 10f;  // Adjust this to the desired height

        // Update the current angle based on the speed and time
        currentAngle += circleSpeed * Time.deltaTime;

        // Calculate the new position on the circle relative to the center point
        float x = Mathf.Cos(currentAngle) * circleRadius;
        float z = Mathf.Sin(currentAngle) * circleRadius;
        Vector3 newPos = new Vector3(x, dragonAltitude, z) + centerPoint.position;
        // Move the dragon to the new position
        transform.position = newPos;

        // Calculate the forward direction (tangent to the circle)
        Vector3 forwardDirection = new Vector3(-Mathf.Sin(currentAngle), 0f, Mathf.Cos(currentAngle)).normalized;

        // Rotate the dragon to face the forward direction
        transform.forward = forwardDirection;
    }

    void Death() {
        // Trigger death animation and logic
        animator.SetTrigger("Death");
        StartCoroutine(DragonDeath());
    }

    private IEnumerator DragonDeath() {
        isDead = true;

        Debug.Log("dead");
        portal.gameObject.SetActive(true) ;

        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public void TakeHit() {
        hitsTaken++;
        Vector3 direction = new Vector3(0f, -1f, 0f);
        cinemachineImpulse.GenerateImpulse(direction);
        if (hitsTaken >= hitsToDie) {
            ChangeState(DragonState.Death);
        }
    }
}
