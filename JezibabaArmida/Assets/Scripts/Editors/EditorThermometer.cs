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
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        t = GameObject.Find("temp_txt").GetComponent<TextMeshProUGUI>();
        finalNumber = GameObject.Find("value").GetComponent<TextMeshProUGUI>();
        slider.onValueChanged.AddListener(e => {
            t.text = slider.value + "";
            finalNumber.text = slider.value+"";
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
