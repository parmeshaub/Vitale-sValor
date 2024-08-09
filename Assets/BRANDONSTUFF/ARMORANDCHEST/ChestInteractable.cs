using UnityEngine;

public class ChestInteractable : Interactable
{
    public ArmorSO armorStored; // The armor stored in the chest

    private InventoryArmour playerInventory;

    void Start()
    {
        playerInventory = FindObjectOfType<InventoryArmour>(); // Find the inventory component on the player
    }

    public override void Interact()
    {
        if (armorStored != null && playerInventory != null)
        {
            playerInventory.AddArmor(armorStored);
            Debug.Log("Armor retrieved: " + armorStored.armorName);
            armorStored = null;
        }
        else if (armorStored == null)
        {
            Debug.Log("No armor stored in the chest.");
        }
        else
        {
            Debug.LogError("Player inventory not found!");
        }
    }
}