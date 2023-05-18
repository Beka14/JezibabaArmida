using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalerKotol : MonoBehaviour
{
    GridLayoutGroup glg;
    RectTransform rc;
    float width;
    float height;
    int children;
    int pred_children;
    bool p = false;
    int kameneMax;
    int wmax;
    int hmax;
    void Start()
    {
        bool p = false;
        glg = GetComponent<GridLayoutGroup>();
        width = GetComponent<RectTransform>().rect.width;
        height = GetComponent<RectTransform>().rect.height;
        rc = GetComponent<RectTransform>();
        children = 0;
        pred_children = 0;
        
        wmax = Mathf.FloorToInt(rc.rect.width / (glg.cellSize.x + glg.spacing.x));
        hmax = Mathf.FloorToInt(rc.rect.height / (glg.cellSize.y + glg.spacing.y));
        kameneMax = wmax * hmax;
        Debug.Log(kameneMax);
    }

    void Update()                                  
    {
        children = transform.childCount;
        if(children < pred_children)
        {
            glg.cellSize = new Vector2(glg.cellSize.x + 5, glg.cellSize.x + 5);
            wmax = Mathf.FloorToInt(rc.rect.width / (glg.cellSize.x + glg.spacing.x));
            hmax = Mathf.FloorToInt(rc.rect.height / (glg.cellSize.y + glg.spacing.y));
            kameneMax = wmax * hmax;
        }

        pred_children = children;
        if (children >= kameneMax)
        {
            glg.cellSize = new Vector2(glg.cellSize.x - 5, glg.cellSize.x - 5);
            wmax = Mathf.FloorToInt(rc.rect.width / (glg.cellSize.x + glg.spacing.x));
            hmax = Mathf.FloorToInt(rc.rect.height / (glg.cellSize.y + glg.spacing.y));
            kameneMax = wmax * hmax;
        }

        if (glg.cellSize.x > 60 && glg.cellSize.y > 60) glg.cellSize = new Vector2(60, 60);
    }
}
