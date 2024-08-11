using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorilAnimationEvents : MonoBehaviour
{
    [SerializeField] private Animator iceBreathAnimator;
    private static readonly int iceBreathBoolHash = Animator.StringToHash("isIceBreath");
    [SerializeField] private GameObject clawPrefab;
    [SerializeField] private GameObject groundSmashPrefab;
    [SerializeField] private Transform groundSmashTransform;
    [SerializeField] private Transform clawSpawnTransform;
    [SerializeField] private Transform clawSpawnTransform2;
    [SerializeField] private Transform clawSpawnTransform3;

    public void TurnOnIceBreath() => iceBreathAnimator.SetBool(iceBreathBoolHash, true);
    public void TurnOffIceBreath() => iceBreathAnimator.SetBool(iceBreathBoolHash, false);
    public void SpawnClawProjectile() {
        Instantiate(clawPrefab, clawSpawnTransform);
        Instantiate(clawPrefab, clawSpawnTransform2);
        Instantiate(clawPrefab, clawSpawnTransform3);
    }

    public void GroundSmash() {
        Instantiate(groundSmashPrefab, groundSmashTransform);
    }
}
