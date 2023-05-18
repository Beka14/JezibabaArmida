using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragEditor3 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [SerializeField] GameObject kamen;
    [SerializeField] GameObject Kotol;
    public Transform parentAfterDrag;
    Editor3Manager manager;
    int prvy = 0;
    public bool wasInKotol = false;
    bool mozemHodit = false;
    bool mozemVyhodit = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        manager = GameObject.Find("EditorManager").GetComponent<Editor3Manager>();
        if (Kotol == null) Kotol = GameObject.Find("kamene");
        int k = (manager == null) ? manager.pocet_kamenov : manager.pocet_kamenov;
        if (transform.GetSiblingIndex() != k - 1 && k != 0 && wasInKotol) mozemVyhodit = true;
        else mozemVyhodit = false;
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
                Destroy(gameObject);
            }
            else if (transform.parent.name == "Canvas" && (prvy == 1) ? manager.pocet_kamenov < 8 : manager.pocet_kamenov < 8)
            {
                wasInKotol = true;
            }
            else
            {
                Destroy(gameObject);
            }

            transform.SetParent(Kotol.transform);     
            image.raycastTarget = true;
            transform.localScale = new Vector3(1, 1, 1);
        }

    }
}
