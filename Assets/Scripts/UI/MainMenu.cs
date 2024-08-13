using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenus : MonoBehaviour
{
    public void Click() {
        SceneManager.LoadScene("02 - Dungeon_Tutorial");
    }
}
