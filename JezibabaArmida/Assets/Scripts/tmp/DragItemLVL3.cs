using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;

public class DragItemLVL3 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [SerializeField] GameObject kamen;
    GameObject txt;
    public Transform parentAfterDrag;
    public bool wasInKotol = false;
    bool mozemHodit = false;
    bool mozemVyhodit = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("MAM KAME: ---------- " + kamen.name);
        //mozemHodit = false; //(kamen.name == "drag_studeny" && !wasInKotol) || (kamen.name == "drag_horuci" && !wasInKotol);
        //mozemVyhodit = (kamen.name == "drag_studeny" && wasInKotol) || (kamen.name == "drag_horuci" && wasInKotol);
        //Debug.Log(mozemHodit);
        //Debug.Log(mozemVyhodit);
        if (!mozemHodit && !mozemVyhodit)
        {
            if (!wasInKotol)
            {
                GameObject o = Instantiate(kamen, Input.mousePosition, kamen.transform.rotation);
                o.transform.SetParent(transform.parent);
                o.name = kamen.name;
                o.transform.localScale = new Vector3(1, 0.88f, 1);

                txt = kamen.transform.Find("value").GetComponent<GameObject>();

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
            //Debug.Log(parentAfterDrag.name);
            //Debug.Log(transform.parent.name);
            if ((parentAfterDrag.name == "Kotol_lvl3" || parentAfterDrag.name == "kamene") && transform.parent.name == "Canvas" && wasInKotol)
            {
                //Debug.Log("vyhod");
                if (gameObject.name == "drag_studeny") GameManager.instance.RemoveStoneSlider(kamen);
                else GameManager.instance.RemoveStoneSlider(kamen);
                Destroy(gameObject);
            }
            else if (parentAfterDrag.name == "Kotol_lvl3" && transform.parent.name == "Canvas")
            {
                //Debug.Log("prihod");
                if (gameObject.name == "drag_studeny") GameManager.instance.AddStoneSlider(kamen);
                else GameManager.instance.AddStoneSlider(kamen);
                wasInKotol = true;
            }
            else
            {
                Destroy(gameObject);
            }

            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
            transform.localScale = new Vector3(1, 1, 1);
        }

    }
}
