using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldInitializer : MonoBehaviour
{
    private WorldDataStore worldData;

    public GameObject ablazeDungeon;
    public GameObject judgementDungeon;
    public GameObject blistfulnessDungeon;
    public GameObject glaciateDungeon;
    public GameObject volleyDungeon;
    public GameObject razorFangDungeon;
    public GameObject wocDungeon;
    public GameObject combustDungeon;
    public GameObject sanctDungeon;

    public ArtemisPillar animatorHolder1;
    public ArtemisPillar animatorHolder2;
    public ArtemisPillar animatorHolder3;
    public ArtemisPillar animatorHolder4;
    public ArtemisPillar animatorHolder5;
    public ArtemisPillar animatorHolder6;

    public GameObject finalBoss;

    private void Start() {
        StartCoroutine(LoadWorld());
    }

    private IEnumerator LoadWorld() {
        yield return new WaitForSeconds(1f);
        worldData = WorldDataStore.instance;

        UpdateMiniDungeons();
        UpdateRespawn();
    }

    public void UpdateMiniDungeons() {
        if (worldData.ablazeCompleted) ablazeDungeon.SetActive(false);
        if (worldData.judgementCompleted) judgementDungeon.SetActive(false);
        if (worldData.blistfulnessCompleted) blistfulnessDungeon.SetActive(false);
        if (worldData.glaciateCompleted) glaciateDungeon.SetActive(false);
        if (worldData.volleyCompleted) volleyDungeon.SetActive(false);
        if (worldData.razorFangCompleted) razorFangDungeon.SetActive(false);
        if (worldData.wocCompleted) wocDungeon.SetActive(false);
        if (worldData.combustCompleted) combustDungeon.SetActive(false);
        if (worldData.sanctCompleted) sanctDungeon.SetActive(false);
    }

    public void UpdateRespawn() {
        if (worldData.ablazeCompleted) animatorHolder1.ActivateArtemis();
        if (worldData.judgementCompleted) animatorHolder2.ActivateArtemis();
        if (worldData.blistfulnessCompleted) animatorHolder3.ActivateArtemis();
        if (worldData.glaciateCompleted) animatorHolder4.ActivateArtemis();
        if (worldData.volleyCompleted) animatorHolder5.ActivateArtemis();
        if (worldData.razorFangCompleted) animatorHolder6.ActivateArtemis();
    }

    //Final Boss Entrance - Check if 2 boss has been completed.
    //Spawn the Entrance to final boss
    private void FinalDungeon() {
        if (worldData.boarBoss && worldData.dragonBoss) {
            finalBoss.SetActive(true);
        }
    }
}
