using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    GameObject kamene;
    [SerializeField] GameObject studeny;
    [SerializeField] GameObject horuci;
    GameObject thermometer;
    Thermometer thermoScript;
    int answer;

    List<int> vsetky = new List<int>();
    List<GameObject> naPloche = new List<GameObject>();

    private void Start()
    {
        //Debug.Log("*************boardSCRIPT START**************");
        kamene = GameObject.Find("kamene");
        thermometer = GameObject.Find("Slider");
        if(TryGetComponent<Thermometer>(out Thermometer thermoScript)) thermoScript = thermometer.GetComponent<Thermometer>();
        //GetComponent<GameManager>().Init();
    }
    /*
    public void Init()
    {
        Debug.Log("*************boardSCRIPT START**************");
        kamene = GameObject.Find("kamene");
        thermometer = GameObject.Find("Slider");
        thermoScript = thermometer.GetComponent<Thermometer>();
        //GetComponent<GameManager>().Init();
    }
    */
    public string FirstLevelEquasion(int pocet_kamenov)
    {
        List<int> list = new List<int>();
        List<int> kamene = new List<int>();
        int max = 48;
        int min = 11;
        bool ok = false;

        vsetky = new List<int>();

        int pociatocna = Random.Range(min, max + 1);
        int studenyKamen = Random.Range(-5,-1); //studenyKamen = Random.Range(1, 5) * -1;
        int horuciKamen = Random.Range(1, 5);
        int vysledna = Random.Range(min, max + 1);

        while (!ok)
        {
            pociatocna = Random.Range(min, max+1);
            studenyKamen = Random.Range(-5,-1); //studenyKamen = Random.Range(1, 5) * -1;
            horuciKamen = Random.Range(1, 5);
            vysledna = Random.Range(min, max + 1);

            for(int x=0; x < 1000; x++)
            {
                for(int y=0; y < 1000; y++)
                {
                    if ((pociatocna + (horuciKamen * x) + (studenyKamen * y) == vysledna) && x + y <= pocet_kamenov && x != 0 && y != 0)
                    {
                        kamene = new List<int>();
                        ok = true;
                        for (int i = 0; i < x; i++) kamene.Add(horuciKamen);
                        for (int j = 0; j < y; j++) kamene.Add(studenyKamen);
                        //Debug.Log("------------ " + string.Join(",", kamene));
                        answer = vysledna;
                        break;
                    }
                }
            }
        }

        int r = Random.Range(1,3);
        List<int> pole = new List<int>();
        if (r % 2 == 0)
        {
            //zatvorka
            int z = Random.Range(2, 5);
            for(int i = 0; i < z; i++)
            {
                int x = Random.Range(0, kamene.Count-1);
                pole.Add(kamene[x]);        //problem??
                kamene.RemoveAt(x);
            }
        }

        string tmp = Stringify(pole, kamene);
        //TODO 

        SetUpThermo(pociatocna);
        InstantiateStones(vsetky);

        //

        // SAVE EQ

        GameManager.instance.playerStats.kamene = vsetky;
        GameManager.instance.playerStats.pociatocna = pociatocna;
        GameManager.instance.playerStats.rovnica = tmp;
        GameManager.instance.playerStats.finalna = vysledna;
        GameManager.instance.playerStats.savedEq = true;

        //

        return pociatocna + "" + tmp + " = " + vysledna; 
    }

    string Stringify(List<int> a, List<int> b)
    {
        //Debug.Log("------------2 " + string.Join(",", a) + " // " + string.Join(",", b));
        string sb = "";
        int r = Random.Range(0,b.Count);
        for(int i=0;i<r;i++)
        {
            sb += (b[i] < 0) ? " - " + -1*b[i] : " + "+b[i];
            vsetky.Add(b[i]);
        }

        if(a.Count != 0)
        {
            int z = Random.Range(1,3);
            if (z % 2 == 0)
            {
                sb += " + (";
            }
            else sb += " + (";
            for (int i = 0; i < a.Count; i++)
            {
                sb += (a[i] < 0) ? "-" + -1 * a[i] : "+" + a[i];        //TODO druhy level: sb.append((a.get(i) < 0) ? "-" + (-1*a.get(i)): "+" + a.get(i));
                vsetky.Add(a[i]);
            }
            sb += ")";
        }

        for (int i = r; i < b.Count; i++)  //i < b.Count-r
        {
            sb += (b[i] < 0) ? " - " + -1 * b[i] : " + " + b[i];
            vsetky.Add(b[i]);
        }

        //Debug.Log("------------3 " + string.Join(",", vsetky));
        return sb;
    }

    //TODO: NASTAVIT TEPLOMER

    public void SetUpThermo(int value)
    {
        kamene = GameObject.Find("kamene");
        thermometer = GameObject.Find("Slider");
        thermoScript = thermometer.GetComponent<Thermometer>();
        //Debug.Log(thermometer.name);
        thermoScript.SetValue(value);
    }

    public void InstantiateStones(List<int> stones)
    {
        kamene = GameObject.Find("kamene");
        GameObject g;
        foreach (int i in stones)
        {
            //GameObject g;
            if (i < 0)
            {
                g = Instantiate(studeny);
                TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (i == -1)? "":i *-1 + "";
                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
            }
            else
            {
                g = Instantiate(horuci);
                TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (i == 1) ? "":i + "";
                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
            }

            naPloche.Add(g);
            //Debug.Log("------------ " + string.Join(",", naPloche));
        }
    }

    public string SetUpBoard(int x)
    {
        /*
        for (int i=0; i<naPloche.Count; i++)
        {
            Debug.Log("------REMOVE------ " + string.Join(",", naPloche));
            //GameObject a = naPloche[0];
            Destroy(naPloche[0]);
        }
        //Debug.Log(naPloche.Count);
        naPloche = new List<GameObject>();
        */

        //                  TODO if stats ma ulozeny stav, nacitaj ten, ak nie nacitaj novy

        GameObject g = GameObject.Find("kamene");
        for (var i = g.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(g.transform.GetChild(i).gameObject);
        }

        if (GameManager.instance.playerStats.savedEq)
        {
            SetUpThermo(GameManager.instance.playerStats.pociatocna);
            InstantiateStones(GameManager.instance.playerStats.kamene);
            return GameManager.instance.playerStats.GetEquasion();
        }

        else
        {
            return FirstLevelEquasion(x);
        }
       
    }

    public int GetAnswer()
    {
        return answer;
    }
}
