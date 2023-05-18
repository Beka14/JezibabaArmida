using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [SerializeField] GameObject kamen;
    [SerializeField] GameObject Kotol;
    public Transform parentAfterDrag;
    public bool wasInKotol = false;
    bool mozemHodit = false;
    bool mozemVyhodit = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Kotol == null) Kotol = GameObject.Find("Kotol");
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        int v = PlaygroundManager.instance.GetThermoValue();
        mozemHodit = (kamen.name == "studeny" && v == -60 && !wasInKotol) || (kamen.name == "horuci" && v == 100 && !wasInKotol);
        mozemVyhodit = (kamen.name == "studeny" && v == 100 && wasInKotol) || (kamen.name == "horuci" && v == -60 && wasInKotol);
        if (!mozemHodit && !mozemVyhodit)
        {
            if (!wasInKotol)
            {
                GameObject o = Instantiate(kamen, Input.mousePosition, kamen.transform.rotation);
                Image i = o.GetComponent<Image>();
                i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
                o.transform.SetParent(transform.parent);
                o.name = kamen.name;
                o.transform.localScale = new Vector3(1, 1, 1);

                parentAfterDrag = transform.parent;
                transform.SetParent(transform.root);
                transform.SetAsLastSibling();
                image.raycastTarget = false;
            }

            else
            {
                parentAfterDrag = transform.parent;
                transform.SetParent(transform.root);
                transform.SetAsLastSibling();
                image.raycastTarget = false;
            }
        }
        else if(!wasInKotol) image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!mozemHodit && !mozemVyhodit)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!mozemHodit && !mozemVyhodit)
        {
            if (transform.parent.name == "Canvas" && wasInKotol) 
            {
                if (gameObject.name == "studeny") PlaygroundManager.instance.RemoveStone(kamen);
                else PlaygroundManager.instance.RemoveStone(kamen);
                Destroy(gameObject);
            }
            else if ((parentAfterDrag.name == "Kotol" || parentAfterDrag.name == "hod") && transform.parent.name == "Canvas")
            {
                if (gameObject.name == "studeny") PlaygroundManager.instance.AddStone(kamen);
                else PlaygroundManager.instance.AddStone(kamen);
                wasInKotol = true;
            }
            else
            {
                Destroy(gameObject);
            }

            transform.SetParent(Kotol.transform);       //parentAfterDrag
            image.raycastTarget = true;
            transform.localScale = new Vector3(1, 1, 1);
        }

    }

}
