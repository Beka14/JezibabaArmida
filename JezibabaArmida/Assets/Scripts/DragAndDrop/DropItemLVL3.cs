using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItemLVL3 : MonoBehaviour, IDropHandler
{
    bool mozemHodit;
    bool mozemVyhodit;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        bool wasInKotol;
        if (dropped.GetComponent<DragItemLVL3>() == null)
        {
            wasInKotol = dropped.GetComponent<DraggableLVL3_2>().wasInKotol;
        }
        else wasInKotol = dropped.GetComponent<DragItemLVL3>().wasInKotol;

        mozemHodit = false;
        mozemVyhodit = (dropped.name == "drag_studeny" && wasInKotol) || (dropped.name == "drag_horuci" && wasInKotol);
        if (!mozemHodit)
        {

            if (dropped.GetComponent<DragItemLVL3>() == null)
            {
                DraggableLVL3_2 draggableItem = dropped.GetComponent<DraggableLVL3_2>();
                draggableItem.parentAfterDrag = transform;
            }
            else
            {
                DragItemLVL3 draggableItem = dropped.GetComponent<DragItemLVL3>();
                draggableItem.parentAfterDrag = transform;
            }
        }

    }
}
