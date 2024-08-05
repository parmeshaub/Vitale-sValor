using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class MagicSlotDrop : MonoBehaviour, IDropHandler
{
    [SerializeField] private int moveNumber; //Assign 0 -3
    private MagicManager manager;

    private void Start() {
        manager = MagicManager.instance;
    }

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("ran");
        GameObject droppedSkill = eventData.pointerDrag;
        DraggableSkill draggableItem = droppedSkill.GetComponent<DraggableSkill>();

        if (draggableItem != null) {
            draggableItem.parentAfterMove = transform; // Set the new parent (magic slot)
            draggableItem.transform.SetParent(transform); // Update parent to this slot
            draggableItem.transform.localPosition = Vector3.zero; // Reset position within slot
            draggableItem.isUsedMagicSlot = true;
            
            Debug.Log("Item dropped in slot.");
            SkillDroppedIntoSlot(draggableItem.magicMove);
        }
    }

    public void SkillDroppedIntoSlot(MagicMoveSO magic) {
        Debug.Log("Loaded Magic");
        manager.magicMoves[moveNumber] = magic;
    }

    public void SkillOutOfSlot() {
        manager.magicMoves[moveNumber] = manager.nullMagic;
    }
}
