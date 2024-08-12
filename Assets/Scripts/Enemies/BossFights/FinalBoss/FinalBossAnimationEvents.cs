using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossAnimationEvents : MonoBehaviour
{
    public Animator beamAnimator;
    public GameObject arrowObject;
    public Transform arrowTransform;
    public GameObject Tornado;
    public Transform TornadoTransform;
    public Transform TornadoTransform3;
    public Transform TornadoTransform2;
    public EnemyThrowAttack attack;

    private GameObject playerObject;

    private void Start() {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    public void ShootBeam() => beamAnimator.SetTrigger("ShootBeam");

    public void ShootArrow() => Instantiate(arrowObject, arrowTransform.position, arrowTransform.rotation, null);

    public void ShootFireBall() => attack.Throw(playerObject);

    public void ShootTornado() {
        Instantiate(Tornado, TornadoTransform.position, TornadoTransform.rotation, null);
        Instantiate(Tornado, TornadoTransform2.position, TornadoTransform2.rotation, null);
        Instantiate(Tornado, TornadoTransform3.position, TornadoTransform3.rotation, null);
    }
}
