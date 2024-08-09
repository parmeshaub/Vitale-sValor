using System.Collections.Generic;
using UnityEngine;

public class InventoryArmour : MonoBehaviour
{
    public List<ArmorSO> armors = new List<ArmorSO>();

    public void AddArmor(ArmorSO armor)
    {
        armors.Add(armor);
        Debug.Log(armor.armorName + " added to inventory.");
    }

    public void RemoveArmor(ArmorSO armor)
    {
        if (armors.Contains(armor))
        {
            armors.Remove(armor);
            Debug.Log(armor.armorName + " removed from inventory.");
        }
    }
}
