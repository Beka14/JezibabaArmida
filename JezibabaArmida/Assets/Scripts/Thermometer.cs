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
    // Start is called before the first frame update
    void Start()
    {
        t = GameObject.Find("temp_txt").GetComponent<TextMeshProUGUI>();
        slider.onValueChanged.AddListener( e => {
            t.text = slider.value+"";
        });
        slider.value = Random.Range(-60, 100);
    }

    public void ChangeValue(int i)
    {
        slider.value += i;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}
