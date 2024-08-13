using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class TutorialBossAnimation : MonoBehaviour
{
    [SerializeField] private EnemyThrowAttack enemyThrow;
    private GameObject player;
    [SerializeField] private GameObject beamObject;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowPrefabTransform;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ShootArrow()
    {
        Instantiate(arrowPrefab, arrowPrefabTransform.position, arrowPrefabTransform.rotation, null);
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
