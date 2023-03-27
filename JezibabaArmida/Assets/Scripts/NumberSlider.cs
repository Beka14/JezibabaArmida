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
        slider.value = Random.Range(0, 20);
        t.text = slider.value + "";

        //slider.minValue = -1 * (GameManager.instance.GetFinalValue() + Random.Range(3,18));
        //slider.maxValue = GameManager.instance.GetFinalValue() + Random.Range(3, 19);
    }

    public void ChangeValue(int i)
    {
        //Debug.Log(slider.value);
        slider.value += i;
        //Debug.Log(slider.value);
        t.text = slider.value + "";
    }

    public int GetValue()
    {
        return (int)slider.value;
    }
    public void SetValue(int i)
    {
        slider.value = i;
        t.text = slider.value + "";
    }

    public void SetMinMax(int max)
    {
        slider.minValue = max - Random.Range(10,26);
        slider.maxValue = max + Random.Range(3, 19);
        //ChangeValue(0);
    }
}
