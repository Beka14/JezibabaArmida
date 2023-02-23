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
        Debug.Log("DROPLI DO MNA ");
        GameObject dropped = eventData.pointerDrag;
        //int v = PlaygroundManager.instance.GetThermoValue();
        bool wasInKotol = dropped.GetComponent<DragItemLVL3>().wasInKotol;
        mozemHodit = false; //(dropped.name == "drag_studeny" && !wasInKotol) || (dropped.name == "drag_horuci" && !wasInKotol);
        mozemVyhodit = (dropped.name == "drag_studeny" && wasInKotol) || (dropped.name == "drag_horuci" && wasInKotol);
        if (!mozemHodit)
        {
            DragItemLVL3 draggableItem = dropped.GetComponent<DragItemLVL3>();
            draggableItem.parentAfterDrag = transform;
        }

    }
}
