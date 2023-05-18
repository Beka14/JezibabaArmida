using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItemSlot : MonoBehaviour, IDropHandler
{
    bool mozemHodit;
    bool mozemVyhodit;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        int v = PlaygroundManager.instance.GetThermoValue();
        bool wasInKotol = dropped.GetComponent<DraggableItem>().wasInKotol;
        mozemHodit = (dropped.name == "studeny" && v == -60 && !wasInKotol) || (dropped.name == "horuci" && v == 100 && !wasInKotol);
        mozemVyhodit = (dropped.name == "studeny" && v == 100 && wasInKotol) || (dropped.name == "horuci" && v == -60 && wasInKotol);
        if (!mozemHodit)
        {
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
        }
        
    }
}
