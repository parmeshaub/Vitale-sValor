using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDataStore : MonoBehaviour
{
    public static WorldDataStore instance;
    public bool ablazeCompleted = false;
    public bool judgementCompleted = false;
    public bool blistfulnessCompleted = false;
    public bool glaciateCompleted = false;
    public bool volleyCompleted = false;
    public bool razorFangCompleted = false;
    public bool wocCompleted = false;
    public bool combustCompleted = false;
    public bool sanctCompleted = false;

    public bool animatorHolder1 = false;
    public bool animatorHolder2 = false;
    public bool animatorHolder3 = false;
    public bool animatorHolder4 = false;
    public bool animatorHolder5 = false;
    public bool animatorHolder6 = false;

    public bool boarBoss = false;
    public bool dragonBoss = false;

    private void Awake() {
        instance = this;
    }
}
