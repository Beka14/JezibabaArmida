using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableLVL3_2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [SerializeField] GameObject kamen;
    [SerializeField] GameObject Kotol;
    [SerializeField] GameObject kamene;
    GameObject txt;
    public Transform parentAfterDrag;
    public bool wasInKotol = false;
    bool mozemHodit = false;
    bool mozemVyhodit = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Kotol == null) Kotol = GameObject.Find("Kotol_lvl3");
        if (kamene == null) kamene = GameObject.Find("kamene2");
        if (!mozemHodit && !mozemVyhodit)
        {
            if (!wasInKotol)
            {
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
            if (transform.parent.name == "Canvas" && wasInKotol) //(parentAfterDrag.name == "Kotol_lvl3" || parentAfterDrag.name == "hod") && 
            {
                //Debug.Log("vyhod");
                GameManager.instance.RemoveStoneSlider(kamen);
                kamen.transform.SetParent(kamene.transform);
                //Destroy(gameObject);
            }
            else if (transform.parent.name == "Canvas" && !wasInKotol)
            {
                //Debug.Log("prihod");
                GameManager.instance.AddStoneSlider(kamen);
                //wasInKotol = true;
            }
            else
            {
                //Destroy(gameObject);
                kamen.transform.SetParent(kamene.transform);
            }

            //transform.SetParent(parentAfterDrag);       //parentAfterDrag
            if ((parentAfterDrag.name == "hod" || parentAfterDrag.name == "Kotol_lvl3") && !wasInKotol)
            {
                transform.SetParent(Kotol.transform);
                wasInKotol = true;
            }
            else wasInKotol = false;
            
            image.raycastTarget = true;
            transform.localScale = new Vector3(1, 1, 1);
        }

    }
}
