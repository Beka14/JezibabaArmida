using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckButton : MonoBehaviour
{
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Check);
    }

    void Check()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.CheckTask();
    }
}
