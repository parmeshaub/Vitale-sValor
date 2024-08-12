using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBossAnimation : MonoBehaviour
{
    [SerializeField] private EnemyThrowAttack enemyThrow;
    private GameObject player;
    [SerializeField] private GameObject beamObject;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Throw() {
        enemyThrow.Throw(player);
    }

    public void SummonBeam() {
        beamObject.SetActive(true);
        StartCoroutine(TurnOffBeam());
    }

    private IEnumerator TurnOffBeam() {
        yield return new WaitForSeconds(5);
        beamObject.SetActive(false);
    }
}
