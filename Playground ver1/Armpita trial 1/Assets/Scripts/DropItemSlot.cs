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
        Debug.Log("DROPLI DO MNA ");
        GameObject dropped = eventData.pointerDrag;
        int v = PlaygroundManager.instance.GetThermoValue();
        bool wasInKotol = dropped.GetComponent<DraggableItem>().wasInKotol;
        mozemHodit = (dropped.name == "studeny" && v == -60 && !wasInKotol) || (dropped.name == "horuci" && v == 100 && !wasInKotol);
        mozemVyhodit = (dropped.name == "studeny" && v == 100 && wasInKotol) || (dropped.name == "horuci" && v == -60 && wasInKotol);
        if (!mozemHodit)
        {
            //GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
            /*
            Debug.Log("////////////");
            if (dropped.name == "studeny") PlaygroundManager.instance.AddStone(-1);
            else PlaygroundManager.instance.AddStone(1);
            */
            Debug.Log("DROPLI DO MNA " + dropped.transform.parent.name);
        }
        
    }
}
