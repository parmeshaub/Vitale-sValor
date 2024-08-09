using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Inventory/Armor")]
public class ArmorSO : ScriptableObject
{
    public string armorName;
    public int armorLevel;
}