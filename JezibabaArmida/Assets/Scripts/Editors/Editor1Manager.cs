using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Editor1Manager : MonoBehaviour
{
    [SerializeField] GameObject studeny;
    [SerializeField] GameObject horuci;

    Slider slider;
    TextMeshProUGUI value;
    List<int> kamene;

    int hodnota;
    public int pocet_kamenov;
    private void Start()
    {
        hodnota = 0;
        pocet_kamenov = 0;
        kamene = new List<int>();
        GameObject kamene2 = GameObject.Find("kamene2");
        value = GameObject.Find("value").GetComponent<TextMeshProUGUI>();
        slider = GameObject.Find("SliderEditor").GetComponent<Slider>();
        for(int i = 1; i < 10; i++)
        {
            GameObject h = Instantiate(horuci);
            GameObject s = Instantiate(studeny);
            TextMeshProUGUI v = h.transform.Find("value").GetComponent<TextMeshProUGUI>();
            v.text = (i == 1)? "":i+"";
            h.transform.SetParent(kamene2.transform);
            h.transform.SetAsLastSibling();
            h.name = "horuci";
            h.transform.localScale = new Vector3(1, 1, 1);

            v = s.transform.Find("value").GetComponent<TextMeshProUGUI>();
            v.text = (i == 1) ? "" : i + "";
            s.transform.SetParent(kamene2.transform);
            s.transform.SetAsLastSibling();
            s.name = "studeny";
            s.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public void AddValue(GameObject x)
    {
        int e = (x.name == "studeny") ? -1 : 1;
        TextMeshProUGUI t = x.transform.Find("value").GetComponent<TextMeshProUGUI>();
        kamene.Add((t.text == "") ? 1 * e : e * Convert.ToInt16(t.text));
        hodnota += (t.text == "") ? e * 1 : e * Convert.ToInt16(t.text);
        value.text = (t.text == "") ? (e + (Convert.ToInt16(value.text))) + "" : (e * Convert.ToInt16(t.text) + (Convert.ToInt16(value.text))) + "";
        Debug.Log(hodnota);
        pocet_kamenov++;
    }
    public void RemoveValue(GameObject x)
    {
        int e = (x.name == "studeny") ? -1 : 1;
        TextMeshProUGUI t = x.transform.Find("value").GetComponent<TextMeshProUGUI>();
        kamene.Remove((t.text == "") ? 1 * e : e * Convert.ToInt16(t.text));
        hodnota += (t.text == "") ? -1 * e : -1 * e * Convert.ToInt16(t.text);
        value.text = (t.text == "") ? (-1 * e + (Convert.ToInt16(value.text)))+"" : (-1 * e * Convert.ToInt16(t.text) + (Convert.ToInt16(value.text))) +"";
        Debug.Log(hodnota);
        pocet_kamenov--;
    }

    public void SubmitTask()
    {
        GameManager.instance.playerStats.kamene = kamene;
        GameManager.instance.playerStats.pociatocna = (int)slider.value;
        GameManager.instance.playerStats.finalna = Convert.ToInt16(value.text);
        GameManager.instance.playerStats.savedEq = true;
        GameManager.instance.playerStats.savedEditor1 = true;

        SceneManager.LoadScene(1);
    }

    public void CheckTask()
    {
        if(kamene.Count == 0)
        {
            StartCoroutine(ChangeColor(0));
        }
        else
        {
            GameObject.Find("next_btn").GetComponent<Button>().interactable = true;
            StartCoroutine(ChangeColor(1));
        }
    }

    IEnumerator ChangeColor(int x)
    {
        Image i = GameObject.Find("Numbers").GetComponent<Image>();
        Color color = i.color;
        if(x == 0) i.color = Color.red;
        else i.color = Color.green;
        yield return new WaitForSeconds(1f);
        i.color = color;
    }

    public int GetHodnota()
    {
        return hodnota;
    }
}
