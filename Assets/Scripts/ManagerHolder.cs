using Guirao.UltimateTextDamage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerHolder : MonoBehaviour
{
    public static ManagerHolder Instance;
    [SerializeField] public UltimateTextDamageManager damagerNumber;

    private void Awake() {
        Instance = this;
    }
}
