using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelResetButtons : MonoBehaviour
{
    Button button;
    int button_number;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Reset);
        switch (gameObject.name)
        {
            case "reset1":
                button_number = 1;
                break;
            case "reset2":
                button_number = 2;
                break;
            case "reset3":
                button_number = 3;
                break;
            case "reset4":
                button_number = 4;
                break;
            default:
                break;
        }
    }

    void Reset()
    {
        StartCoroutine("ChangeColor");
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.ResetLevel(button_number);
    }

    IEnumerator ChangeColor()
    {
        Image i = gameObject.GetComponent<Image>();
        i.color = Color.green;
        yield return new WaitForSeconds(0.2f);
        i.color = Color.white;
    }
}
