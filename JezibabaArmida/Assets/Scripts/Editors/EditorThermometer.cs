using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorThermometer : MonoBehaviour
{
    Slider slider;
    TextMeshProUGUI finalNumber;
    private TextMeshProUGUI t;
    int prvy = 0;
    Editor1Manager manager;
    Editor2Manager manager2;
    Editor3Manager manager3;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("EditorManager").GetComponent<Editor1Manager>();
        if (manager != null) prvy = 1;
        manager2 = GameObject.Find("EditorManager").GetComponent<Editor2Manager>();
        if (manager2 != null) prvy = 2;
        manager3 = GameObject.Find("EditorManager").GetComponent<Editor3Manager>();
        if (manager3 != null) prvy = 3;
        slider = GetComponent<Slider>();
        t = GameObject.Find("temp_txt").GetComponent<TextMeshProUGUI>();
        if(prvy!=3) finalNumber = GameObject.Find("value").GetComponent<TextMeshProUGUI>();
        slider.onValueChanged.AddListener(e => {
            t.text = slider.value + "";
            if(prvy!=3) finalNumber.text = (prvy==1)? manager.GetHodnota() + slider.value+"": manager2.GetHodnota() + slider.value + "";
            if (prvy == 3 && (manager3.valid || manager3.infine || manager3.zero)) manager3.SetSolutions(0);
        });
        slider.value = Random.Range(-60, 100);
        t.text = slider.value + "";

    }

    public void ChangeValue(int i)
    {
        slider.value += i;
        t.text = slider.value + "";
    }
    public int GetValue()
    {
        t = GameObject.Find("temp_txt").GetComponent<TextMeshProUGUI>();
        return (int)slider.value;
    }
    public void SetValue(int i)
    {
        t = GameObject.Find("temp_txt").GetComponent<TextMeshProUGUI>();
        slider.value = i;
        t.text = slider.value + "";
        finalNumber.text = slider.value + "";
    }

}
