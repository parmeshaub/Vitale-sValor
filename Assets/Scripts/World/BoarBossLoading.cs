using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class BoarBossLoading : MonoBehaviour
{
    private SceneLoader sceneLoader;
    public Transform exit;
    public WorldDataStore worldDataStore;

    private void OnTriggerEnter(Collider other) {
        sceneLoader = SceneLoader.Instance;
        sceneLoader.LoadDungeon("05 - Boar", exit);
        worldDataStore.boarBoss = true;
        if (RuneManager.instance.worldEnum == CurrentWorld.Flurry) {
            
        }
    }
}
