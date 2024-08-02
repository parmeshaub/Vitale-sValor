using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCAnimationEventsController : MonoBehaviour
{
    private SwordManager swordManager;

    private void Start()
    {
        swordManager = SwordManager.instance;
    }

    public void Unsheath()
    {
        swordManager.UnsheathSword();
    }

    public void Sheath()
    {
        swordManager.SheathSword();
    }

    public void UnsheathAttack()
    {
        swordManager.UnsheathSwordDuringAttack();
    }
}
