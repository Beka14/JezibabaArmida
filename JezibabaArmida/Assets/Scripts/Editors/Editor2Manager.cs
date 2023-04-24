using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Editor2Manager : MonoBehaviour
{
    [SerializeField] GameObject studeny;
    [SerializeField] GameObject horuci;
    [SerializeField] GameObject plus;
    [SerializeField] GameObject minus;
    [SerializeField] GameObject web;
    [SerializeField] GameObject web_znam;

    Slider slider;
    TextMeshProUGUI value;
    List<int> kamene;
    List<string> znam;

    int hodnota;
    public int znamienko;
    public int pocet_kamenov;
    private void Start()
    {
        hodnota = 0;
        znamienko = 0;
        pocet_kamenov = 0;
        kamene = new List<int>();
        znam = new List<string>();
        GameObject kamene2 = GameObject.Find("kamene2");
        GameObject minusSpace = GameObject.Find("minus");
        GameObject plusSpace = GameObject.Find("plus");
        value = GameObject.Find("value").GetComponent<TextMeshProUGUI>();
        slider = GameObject.Find("SliderEditor").GetComponent<Slider>();
        for (int i = 1; i < 10; i++)
        {
            GameObject h = Instantiate(horuci);
            GameObject s = Instantiate(studeny);
            TextMeshProUGUI v = h.transform.Find("value").GetComponent<TextMeshProUGUI>();
            v.text = (i == 1) ? "" : i + "";
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
        GameObject m = Instantiate(minus);
        GameObject p = Instantiate(plus);
        m.transform.SetParent(minusSpace.transform);
        m.transform.localScale = new Vector3(1, 1, 1);
        m.name = "minus";
        p.transform.SetParent(plusSpace.transform);
        p.transform.localScale = new Vector3(1, 1, 1);
        p.name = "plus";
    }

    private void Update()
    {
        if(znamienko == 0 || znamienko == -2)
        {
            web.SetActive(true);
            web_znam.SetActive(false);
        }
        else
        {
            web.SetActive(false);
            web_znam.SetActive(true);
        }
    }
    public void AddValue(GameObject x)
    {
        if (x.name == "minus")
        {
            znamienko = -1;
            znam.Add("-");
            pocet_kamenov++;
            return;
        }
        else if(x.name == "plus")
        {
            znamienko = 1;
            znam.Add("+");
            pocet_kamenov++;
            return;
        }
        int z = (znamienko == -1) ? -1 : 1;
        int e = (x.name == "studeny") ? -1 : 1;
        TextMeshProUGUI t = x.transform.Find("value").GetComponent<TextMeshProUGUI>();
        kamene.Add((t.text =="")? 1*e:e * Convert.ToInt16(t.text));
        hodnota += (t.text == "") ? z*e * 1 : z*e * Convert.ToInt16(t.text);
        value.text = (t.text == "") ? (e + (z * Convert.ToInt16(value.text))) + "" : (z * e * Convert.ToInt16(t.text) + (Convert.ToInt16(value.text))) + "";
        Debug.Log(hodnota);
        pocet_kamenov++;
        znamienko = 0;
    }
    public void RemoveValue(GameObject x)
    {
        if (x.name == "minus")
        {
            znamienko = -2;
            znam.Remove("-");
            pocet_kamenov--;
            return;
        }
        else if (x.name == "plus")
        {
            znamienko = 0;
            znam.Remove("+");
            pocet_kamenov--;
            return;
        }

        int z = (znamienko == -2) ? -1 : 1;
        int e = (x.name == "studeny") ? -1 : 1;
        TextMeshProUGUI t = x.transform.Find("value").GetComponent<TextMeshProUGUI>();
        kamene.Remove((t.text == "") ? 1 * e : e * Convert.ToInt16(t.text));
        hodnota += (t.text == "") ? z * -1 * e : z * -1 * e * Convert.ToInt16(t.text);
        value.text = (t.text == "") ? (z * -1 * e + (Convert.ToInt16(value.text))) + "" : (z * -1 * e * Convert.ToInt16(t.text) + (Convert.ToInt16(value.text))) + "";
        Debug.Log(hodnota);
        pocet_kamenov--;
        znamienko = 1;

    }

    public void SubmitTask()
    {
        GameManager.instance.playerStats.kamene2 = kamene;
        GameManager.instance.playerStats.znamienka2 = znam;
        GameManager.instance.playerStats.pociatocna2 = (int)slider.value;
        GameManager.instance.playerStats.finalna2 = Convert.ToInt16(value.text);
        GameManager.instance.playerStats.zatvorky = new Dictionary<int, string>();
        GameManager.instance.playerStats.savedEq2 = true;
        GameManager.instance.playerStats.savedEditor2 = true;

        SceneManager.LoadScene(2);
    }

    public void CheckTask()
    {
        if (kamene.Count == 0)
        {
            //ZLE
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
        if (x == 0) i.color = Color.red;
        else i.color = Color.green;
        yield return new WaitForSeconds(1f);
        i.color = color;
    }

    public int GetHodnota()
    {
        return hodnota;
    }
}
