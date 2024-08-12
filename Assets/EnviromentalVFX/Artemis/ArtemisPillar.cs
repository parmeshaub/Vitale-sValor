using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ArtemisPillar : Interactable
{
    public Animator artemisAnimator;
    public bool isUnlocked = false;
    public Transform respawnPoint;
    public int number;
    public WorldDataStore worldData;

    private void Awake() {
        worldData = WorldDataStore.instance;
    }
    public void ActivateArtemis()
    {
        if(worldData == null) {
            worldData = WorldDataStore.instance;
        }
        artemisAnimator.SetTrigger("activated");
        UpdatePillarData(number);
    }

    public override void Interact() {
        isUnlocked = true;
        ActivateArtemis();
    }

    public void UpdatePillarData(int number) {
        switch (number) {
            case 0:
                worldData.animatorHolder1 = true;
                break;
            case 1:
                worldData.animatorHolder2 = true;
                break;
            case 2:
                worldData.animatorHolder3 = true;
                break;
            case 3:
                worldData.animatorHolder4 = true;
                break;
            case 4:
                worldData.animatorHolder5 = true;
                break;
            case 5:
                worldData.animatorHolder6 = true;
                break;
            default:
                Debug.LogWarning("Invalid pillar number: " + number);
                break;
        }
    }

}
