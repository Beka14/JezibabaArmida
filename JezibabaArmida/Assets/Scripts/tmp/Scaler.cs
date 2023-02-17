using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scaler : MonoBehaviour
{
    GridLayoutGroup glg;
    float width;
    int children;
    void Start()
    {
        glg = GetComponent<GridLayoutGroup>();
        width = GetComponent<RectTransform>().rect.width;
        children = 0;
    }

    // Update is called once per frame
    void Update()                                   //TODO nieje to bad???
    {
        children = transform.childCount;
        if (children * glg.cellSize.x >= width)
        {
            glg.cellSize = new Vector2(width / children, width / children);
        }
        else
        {
            glg.cellSize = new Vector2(100, 100);
        }
    }
}
