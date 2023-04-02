using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Next);
    }

    void Next()
    {
        //Debug.Log("klik");
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //gm.CheckTask();
        gm.NextTask();
    }
}
