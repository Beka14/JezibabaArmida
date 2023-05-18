using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Next);
    }

    void Next()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.NextTask();
    }
}
