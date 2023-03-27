using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfineButton : MonoBehaviour
{
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Check);
    }

    void Check()
    {
        if(GameManager.instance.playerStats.infine) GameManager.instance.InfineSolutions();
    }
}
