using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoSolutionsButton : MonoBehaviour
{
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Check);
    }

    void Check()
    {
        if (GameManager.instance.playerStats.noSolutions) GameManager.instance.NoSolutions();
    }
}
