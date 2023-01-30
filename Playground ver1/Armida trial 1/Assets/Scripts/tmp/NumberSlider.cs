using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private TextMeshProUGUI t;
    void Start()
    {
        t = GameObject.Find("text").GetComponent<TextMeshProUGUI>();
        slider.onValueChanged.AddListener(e => {
            t.text = slider.value + "";
        });
        slider.value = Random.Range(-60, 100);
        t.text = slider.value + "";
    }

    public void ChangeValue(int i)
    {
        Debug.Log(slider.value);
        slider.value += i;
        Debug.Log(slider.value);
        t.text = slider.value + "";
    }

    // Update is called once per frame
    public int GetValue()
    {
        return (int)slider.value;
    }
    public void SetValue(int i)
    {
        slider.value = i;
        t.text = slider.value + "";
    }
}
