using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableSkill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public MagicMoveSO magicMove;
    public Image uiDisplayImage;
    [SerializeField] private GameObject parentDuringMove;
    [SerializeField] public Transform originalTransform; // Original parent
    [HideInInspector] public Transform parentAfterMove;
    public bool isUsedMagicSlot = false;
    [SerializeField] private TMP_Text skillTitle;

    void Start() {
        uiDisplayImage.sprite = magicMove.icon;
        skillTitle.text = magicMove.moveName;
    }

    private void Update() {

    }

    public void OnBeginDrag(PointerEventData eventData) {
        parentAfterMove = transform.parent; // To save the current parent.
        transform.SetParent(parentDuringMove.transform); // Set the Canvas as Parent DURING Drag.
        transform.SetAsLastSibling(); // Make sure the Image is at the top of the view.
        uiDisplayImage.raycastTarget = false;

        if(isUsedMagicSlot) {
            isUsedMagicSlot = false;
            MagicSlotDrop magic = parentAfterMove.gameObject.GetComponent<MagicSlotDrop>();
            magic.SkillOutOfSlot();
        }
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if(isUsedMagicSlot) {
            transform.SetParent(parentAfterMove);
            uiDisplayImage.raycastTarget = true;
        }
        else {
            transform.SetParent(originalTransform);
            uiDisplayImage.raycastTarget = true;
        }
    }
}
