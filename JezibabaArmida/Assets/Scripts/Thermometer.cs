using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Thermometer : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private TextMeshProUGUI t;
    void Start()
    {
        t = GameObject.Find("temp_txt").GetComponent<TextMeshProUGUI>();
        slider.onValueChanged.AddListener( e => {
            t.text = slider.value+"";
        });
        slider.value = Random.Range(-60, 100);
        t.text = slider.value+"";
    }

    public void ChangeValue(int i)
    {
        slider.value += i;
        t.text = slider.value+"";
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
    }
}
