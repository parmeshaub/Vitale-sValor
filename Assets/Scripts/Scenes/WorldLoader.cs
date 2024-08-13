using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private SceneLoader sceneLoader;

    public void Start()
    {
        sceneLoader = SceneLoader.Instance;

        if (sceneLoader != null)
        {
            Debug.Log("able switch");
        }
    }

    private void OnTriggerEnter(Collider other) {

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (other.name == player.name)
        {
            Debug.Log("found");
            sceneLoader.LoadToWorld(sceneName);
            //GameManager.Instance.miniDungeonsCompleted++;
        }
    }
}
