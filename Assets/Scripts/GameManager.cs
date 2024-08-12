using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public int miniDungeonsCompleted;
    [HideInInspector] public int bossDefeated;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

   
}
