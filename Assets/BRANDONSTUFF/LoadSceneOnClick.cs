using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            Debug.Log("Mouse Click Detected!"); // Log message when mouse is clicked
            SceneManager.LoadScene("02 - Dungeon_Tutorial");
        }
    }
}
