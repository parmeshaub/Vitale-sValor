using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistPlayer : MonoBehaviour
{
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
