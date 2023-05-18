using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scaler : MonoBehaviour
{
    GridLayoutGroup glg;
    float width;
    int children;
    bool p = false;
    void Start()
    {
        bool p = false;
        glg = GetComponent<GridLayoutGroup>();
        width = GetComponent<RectTransform>().rect.width;
        children = 0;
    }

    void Update()                                 
    {
        children = transform.childCount;
        if (glg.cellSize != new Vector2((width / children) - 10, (width / children) - 10))
        {
            glg.cellSize = new Vector2((width / children) -10, (width / children) - 10);
        }
        if(glg.cellSize.x > 100) glg.cellSize = new Vector2(100, 100);
    }
}
