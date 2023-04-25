using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System;

public class DragEditor : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [SerializeField] GameObject kamen;
    [SerializeField] GameObject Kotol;
    public Transform parentAfterDrag;
    Editor1Manager manager;
    Editor2Manager manager2;
    Editor3Manager manager3;
    int prvy = 0;
    int k = 0;
    public bool wasInKotol = false;
    bool mozemHodit = false;
    bool mozemVyhodit = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        manager = GameObject.Find("EditorManager").GetComponent<Editor1Manager>();
        if (manager != null) prvy = 1;
        manager2 = GameObject.Find("EditorManager").GetComponent<Editor2Manager>();
        if (manager2 != null) prvy = 2;
        manager3 = GameObject.Find("EditorManager").GetComponent<Editor3Manager>();
        if (manager3 != null) prvy = 3;
        Debug.Log(prvy);
        if (Kotol == null) Kotol = GameObject.Find("kamene");
        k = PocetKamenov();
        if (transform.GetSiblingIndex() != k-1 && k != 0 && wasInKotol) mozemVyhodit = true;
        else mozemVyhodit = false;
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

    private int PocetKamenov()
    {
        if (prvy == 1) return manager.pocet_kamenov;
        if (prvy == 2) return manager2.pocet_kamenov;
        //else return manager3.pocet_kamenov;
        return 0;
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
                if (prvy == 3 && ((parentAfterDrag.name == "kamene1i" && manager3.prvy_slot) || (parentAfterDrag.name == "kamene2i" && manager3.druhy_slot)))
                {
                    if (parentAfterDrag.name == "kamene1i")
                    {
                        manager3.prvy_slot = false;
                        manager3.prvy_kamen = 0;
                        manager3.SetSolutions(0);
                    }

                    else
                    {
                        manager3.druhy_slot = false;
                        manager3.druhy_kamen = 0;
                        manager3.SetSolutions(0);
                    }
                }
                else if (prvy == 3)
                {
                    Destroy(gameObject);
                    return;
                }
                else if (prvy==1) manager.RemoveValue(kamen); else manager2.RemoveValue(kamen);
                Destroy(gameObject);
            }
            else if (parentAfterDrag.name != "kamene2" && parentAfterDrag.name != "minus" && parentAfterDrag.name != "plus" && transform.parent.name == "Canvas" && k < 8)
            {
                if (prvy == 3 && ((parentAfterDrag.name == "kamene1i" && !manager3.prvy_slot) || (parentAfterDrag.name == "kamene2i" && !manager3.druhy_slot)))
                {
                    if (parentAfterDrag.name == "kamene1i")
                    {
                        manager3.prvy_slot = true;
                        manager3.prvy_kamen = GameManager.instance.GetValueFromStone(kamen);
                    }

                    else
                    {
                        manager3.druhy_slot = true;
                        manager3.druhy_kamen = GameManager.instance.GetValueFromStone(kamen);
                    }
                    wasInKotol = true;
                }
                else if (prvy == 3)
                {
                    Destroy(gameObject);
                    return;
                }
                
                else if (prvy == 1) manager.AddValue(kamen); else manager2.AddValue(kamen);
                wasInKotol = true;
            }
            else
            {
                Destroy(gameObject);
            }

            transform.SetParent(parentAfterDrag);       //parentAfterDrag kotol.transform
            image.raycastTarget = true;
            transform.localScale = new Vector3(1, 1, 1);
        }

    }
}
