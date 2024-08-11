using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAP : MonoBehaviour
{
    ///////////////////////////////////////////
    /// <summary>
    ///  This script is for Chests and Portals!!
    /// </summary>
    ////////////////////////////////////////////
    public GameObject portal1;
    public GameObject portal2;
    public GameObject portal3;

    public GameObject portal4;
    public GameObject portal5;
    public GameObject portal6;

    public GameObject portal7;
    public GameObject portal8;
    public GameObject portal9;

    public GameObject Tier1Chest;
    public GameObject Tier2Chest;
    public GameObject Tier3Chest;

    public GameObject volcanoPortal;

    public string goingWhere;

    /// <summary>
    /// Setting dimension
    /// </summary>
    public void SetWorld(string yuh)
    {
        goingWhere = yuh;
        ActivatePortals(); // When setting a diff world, portals change too!
    }

    /// <summary>
    /// Based on game's progression stage, activate the following tier chests
    /// </summary>
    public void ActivateChestTiers(string stage)
    {
        if (stage == "stage2") // aftering defeating boril
        {
            foreach (Transform child in Tier1Chest.transform)
            {
                child.gameObject.SetActive(false);
            }

            foreach (Transform child in Tier2Chest.transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        if (stage == "stage3") // aftering defeating boril
        {
            foreach (Transform child in Tier2Chest.transform)
            {
                child.gameObject.SetActive(false);
            }

            foreach (Transform child in Tier3Chest.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }




    /// <summary>
    /// Active portal based on current dimension
    /// </summary>
    public void ActivatePortals()
    {
        if (goingWhere == "flora")
        {
            portal1.SetActive(true);
            portal2.SetActive(true);
            portal3.SetActive(true);

            portal4.SetActive(false);
            portal5.SetActive(false);
            portal6.SetActive(false);

            portal7.SetActive(false);
            portal8.SetActive(false);
            portal9.SetActive(false);

            volcanoPortal.SetActive(false);
        }

        if (goingWhere == "flurry")
        {
            portal1.SetActive(false);
            portal2.SetActive(false);
            portal3.SetActive(false);

            portal4.SetActive(true);
            portal5.SetActive(true);
            portal6.SetActive(true);

            portal7.SetActive(false);
            portal8.SetActive(false);
            portal9.SetActive(false);

            volcanoPortal.SetActive(false);
        }

        if (goingWhere == "fyre")
        {
            portal1.SetActive(false);
            portal2.SetActive(false);
            portal3.SetActive(false);

            portal4.SetActive(false);
            portal5.SetActive(false);
            portal6.SetActive(false);

            portal7.SetActive(true);
            portal8.SetActive(true);
            portal9.SetActive(true);

            volcanoPortal.SetActive(true);
        }





    }
}
