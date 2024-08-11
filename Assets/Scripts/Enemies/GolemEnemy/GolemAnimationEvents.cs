using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAnimationEvents : MonoBehaviour
{
    [SerializeField] private GameObject groundSmash;
    [SerializeField] private Transform groundSmashTransform;

    public void SpawnAttack() => Instantiate(groundSmash,groundSmashTransform);
}
