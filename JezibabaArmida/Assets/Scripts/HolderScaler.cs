using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HolderScaler : MonoBehaviour
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
        if (glg.cellSize != new Vector2((width / children) - 10, (width / children) - 10))
        {
            glg.cellSize = new Vector2((width / children) - 10, (width / children) - 10);
        }
        if (glg.cellSize.x > 75) glg.cellSize = new Vector2(75, 75);
    }
}
