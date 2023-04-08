using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropEditor : MonoBehaviour, IDropHandler
{
    bool mozemHodit;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        mozemHodit = false;
        if (!mozemHodit)
        {
            DragEditor draggableItem = dropped.GetComponent<DragEditor>();
            draggableItem.parentAfterDrag = transform;

        }

    }
}
