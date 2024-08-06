using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPillarScript : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> pillarPartList = new List<Rigidbody>();

    private void Start() {
        foreach (var item in pillarPartList) {
            item.constraints = RigidbodyConstraints.FreezeAll; // This freezes both position and rotation
            item.useGravity = false;
        }
    }

    public void PillarHit(Vector3 directionHit) {
        foreach (var item in pillarPartList) {
            item.constraints = RigidbodyConstraints.None;
            item.useGravity = true;
            item.AddForce(directionHit, ForceMode.Impulse);
        }
    }    
}

