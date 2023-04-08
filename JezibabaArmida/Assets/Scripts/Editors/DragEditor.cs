using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class DragEditor : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [SerializeField] GameObject kamen;
    [SerializeField] GameObject Kotol;
    public Transform parentAfterDrag;
    Editor1Manager manager;
    public bool wasInKotol = false;
    bool mozemHodit = false;
    bool mozemVyhodit = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        manager = GameObject.Find("Editor1Manager").GetComponent<Editor1Manager>();
        if (Kotol == null) Kotol = GameObject.Find("kamene");
        //Debug.Log(mozemVyhodit);
        if (!mozemHodit && !mozemVyhodit)
        {
            if (!wasInKotol)
            {
                GameObject o = Instantiate(kamen, Input.mousePosition, kamen.transform.rotation);
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
            Debug.Log(parentAfterDrag.name);
            Debug.Log(transform.parent.name);
            if (transform.parent.name == "Canvas" && wasInKotol)
            {
                GameObject.Find("Editor1Manager").GetComponent<Editor1Manager>().RemoveValue(kamen);
                Destroy(gameObject);
            }
            else if (parentAfterDrag.name == "kamene" && transform.parent.name == "Canvas" && manager.pocet_kamenov < 8)
            {
                GameObject.Find("Editor1Manager").GetComponent<Editor1Manager>().AddValue(kamen);
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
