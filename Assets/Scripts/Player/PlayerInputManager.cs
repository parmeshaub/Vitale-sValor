using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }
}
