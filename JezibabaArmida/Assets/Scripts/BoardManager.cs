using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    GameObject kamene;
    [SerializeField] GameObject studeny;
    [SerializeField] GameObject horuci;
    [SerializeField] GameObject minus;
    [SerializeField] GameObject plus;
    [SerializeField] GameObject minusPar;
    [SerializeField] GameObject plusPar;
    [SerializeField] GameObject endPar;
    GameObject thermometer;
    Thermometer thermoScript;

    int answer;

    List<int> vsetky = new List<int>();
    List<GameObject> naPloche = new List<GameObject>();
    List<string> znamienka = new List<string>();
    Dictionary<int, string> zatvorky = new Dictionary<int, string>();

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
        znamienka = new List<string>();
        zatvorky = new Dictionary<int, string>();

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
            int z = Random.Range(2, kamene.Count()-1);
            for(int i = 0; i < z; i++)
            {
                int x = Random.Range(0, kamene.Count() -1);
                pole.Add(kamene[x]);        //problem??
                kamene.RemoveAt(x);
            }
        }

        string tmp;
        if (GameManager.instance.level == 1) tmp = Stringify(pole, kamene);
        else {
            answer = pociatocna;
            tmp = Stringify_lvl2(pole, kamene);
        } 

        //TODO 

        SetUpThermo(pociatocna);
        InstantiateStones(vsetky, znamienka);

        //

        // SAVE EQ

        if(GameManager.instance.level == 1)
        {
            GameManager.instance.playerStats.kamene = vsetky;
            GameManager.instance.playerStats.pociatocna = pociatocna;
            GameManager.instance.playerStats.rovnica = tmp;
            GameManager.instance.playerStats.finalna = vysledna;
            GameManager.instance.playerStats.savedEq = true;
            GameManager.instance.playerStats.level = GameManager.instance.level;
        }

        else if (GameManager.instance.level == 2) 
        {
            GameManager.instance.playerStats.kamene2 = vsetky;
            GameManager.instance.playerStats.znamienka2 = znamienka;
            GameManager.instance.playerStats.pociatocna2 = pociatocna;
            GameManager.instance.playerStats.rovnica2 = tmp;
            GameManager.instance.playerStats.finalna2 = answer;
            GameManager.instance.playerStats.savedEq2 = true;
            GameManager.instance.playerStats.level = GameManager.instance.level;
        }

        //

        Debug.Log("POROVNANIE: poc: " + vysledna + " answ: " + answer);
        return pociatocna + "" + tmp + " = " + answer; 
    }

    string Stringify(List<int> a, List<int> b)
    {
        //Debug.Log("------------2 " + string.Join(",", a) + " // " + string.Join(",", b));
        string sb = "";
        int r = Random.Range(0,b.Count());
        for(int i=0;i<r;i++)
        {
            sb += (b[i] < 0) ? " - " + -1*b[i] : " + "+b[i];
            vsetky.Add(b[i]);
        }

        if(a.Count() != 0)
        {
            int z = Random.Range(1,3);
            if (z % 2 == 0)
            {
                sb += " + (";
            }
            else sb += " + (";
            for (int i = 0; i < a.Count(); i++)
            {
                sb += (a[i] < 0) ? "-" + -1 * a[i] : "+" + a[i];        //TODO druhy level: sb.append((a.get(i) < 0) ? "-" + (-1*a.get(i)): "+" + a.get(i));
                vsetky.Add(a[i]);
            }
            sb += ")";
        }

        for (int i = r; i < b.Count(); i++)  //i < b.Count-r
        {
            sb += (b[i] < 0) ? " - " + -1 * b[i] : " + " + b[i];
            vsetky.Add(b[i]);
        }

        //Debug.Log("------------3 " + string.Join(",", vsetky));
        return sb;
    }



    string Stringify_lvl2(List<int> a, List<int> b)
    {
        int index = 0;
        string sb = "";
        int r = Random.Range(1, b.Count()-1);
        for (int i = 0; i < r; i++)
        {
            sb += (b[i] < 0) ? "-" + -1 * b[i] : "+" + b[i];
            vsetky.Add(b[i]);
            answer += b[i];
            znamienka.Add((b[i] < 0) ? "+" : "+");
            index++;
        }

        if (a.Count() != 0)
        {
            int x = 0;
            int p = Random.Range(1, 3);
            if (p % 2 == 0)
            {
                sb += "-(";
                zatvorky.Add(index,"-(");
                for (int i = 0; i < a.Count(); i++)
                {
                    int z = Random.Range(1, 4);
                    if (z % 3 == 0)
                    {
                        sb += (a[i] < 0) ? "-" + -1 * a[i] : "-" + a[i];
                        vsetky.Add((a[i] < 0) ? -1 * a[i] : -1 * a[i]);
                        x += a[i];
                        znamienka.Add("-");
                        index++;
                    }
                    else
                    {
                        sb += (a[i] < 0) ? "-" + -1 * a[i] : "+" + a[i];
                        vsetky.Add(a[i]);
                        x += a[i];
                        znamienka.Add((a[i] < 0) ? "+" : "+");
                        index++;
                    }

                }
                Debug.Log("x: " + x);
                answer += -1 * x;
                sb += ")";
                zatvorky.Add(index,")");
            }
            else
            {
                sb += "+(";
                zatvorky.Add(index, "+(");
                for (int i = 0; i < a.Count(); i++)
                {
                    int z = Random.Range(1, 4);
                    if (z % 3 == 0)
                    {
                        sb += (a[i] < 0) ? "-" + -1 * a[i] : "-" + a[i];
                        vsetky.Add((a[i] < 0) ? -1 * a[i] : -1 * a[i]);
                        znamienka.Add("-");
                        answer += a[i];
                        index++;
                    }
                    else
                    {
                        sb += (a[i] < 0) ? "-" + -1 * a[i] : "+" + a[i];
                        vsetky.Add(a[i]);
                        znamienka.Add((a[i] < 0) ? "+" : "+");
                        answer += a[i];
                        index++;
                    }
                }
                sb += ")";
                zatvorky.Add(index, ")");
            }
            Debug.Log("------------3 " + string.Join(",", zatvorky));
            /*
            for (int i = 0; i < a.Count(); i++)
            {
                int z = Random.Range(1, 4);
                if (z % 3 == 0)
                {
                    sb += (a[i] < 0) ? "-" + -1 * a[i] : "-" + a[i];
                    vsetky.Add((a[i] < 0) ? -1 * a[i] : -1 * a[i]);
                    znamienka.Add("-");
                }
                else {
                    sb += (a[i] < 0) ? "-" + -1 * a[i] : "+" + a[i];
                    vsetky.Add(a[i]);
                    znamienka.Add((a[i] < 0) ? "+" : "+");
                }   
            }
            sb += ")";
            */
        }

        for (int i = r; i < b.Count(); i++)  //i < b.Count-r
        {
            int z = Random.Range(1, 4);
            if (z % 3 == 0)
            {
                sb += (b[i] < 0) ? "-" + -1 * b[i] : "-" + b[i];
                vsetky.Add((b[i] < 0) ? -1 * b[i] : -1 * b[i]);
                answer += b[i];
                znamienka.Add("-");
            }
            else
            {
                sb += (b[i] < 0) ? "-" + -1 * b[i] : "+" + b[i];
                vsetky.Add(b[i]);
                answer += b[i];
                znamienka.Add((b[i] < 0) ? "+" : "+");
            }
        }

        //Debug.Log("------------3 " + string.Join(",", vsetky));
        return sb;
    }

    public void SetUpThermo(int value)
    {
        kamene = GameObject.Find("kamene");
        thermometer = GameObject.Find("Slider");
        thermoScript = thermometer.GetComponent<Thermometer>();
        //Debug.Log(thermometer.name);
        thermoScript.SetValue(value);
    }

    public void InstantiateStones(List<int> stones, List<string> znam)         //TODO pridat aj znamienka na SAVE
    {
        int index = 0;
        kamene = GameObject.Find("kamene");
        if(GameManager.instance.level == 1) kamene.GetComponent<GridLayoutGroup>().spacing = new Vector2(15,0);
        else kamene.GetComponent<GridLayoutGroup>().spacing = new Vector2(0, 0);
        GameObject g;

        for(int i = 0; i < stones.Count; i++)
        {

            if (stones[i] < 0)
            {
                g = Instantiate(studeny);
                TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (stones[i] == -1) ? "" : stones[i] * -1 + "";
                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
            }
            else
            {
                g = Instantiate(horuci);
                TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (stones[i] == 1) ? "" : stones[i] + "";
                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
            }

            index++;
            naPloche.Add(g);

            if (zatvorky.ContainsKey(index))
            {
                if (zatvorky[index] == "+(")
                {
                    g = Instantiate(plusPar);
                }

                else if (zatvorky[index] == "-(") g = Instantiate(minusPar);

                else g = Instantiate(endPar);

                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
                naPloche.Add(g);
            }

            if (znam.Count() != 0 && i+1 < znam.Count())
            {
                //Debug.Log("------------ " + string.Join(",", znamienka));
                if (znam[i+1] == "-")
                {
                    g = Instantiate(minus);
                    g.transform.SetParent(kamene.transform);
                    g.transform.SetAsLastSibling();
                }
                else
                {
                    g = Instantiate(plus);
                    g.transform.SetParent(kamene.transform);
                    g.transform.SetAsLastSibling();
                }

                naPloche.Add(g);
            }

            //naPloche.Add(g);

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
        

        GameObject g = GameObject.Find("kamene");
        for (var i = g.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(g.transform.GetChild(i).gameObject);
        }

        ////////// SAVED EQ
        
        if (GameManager.instance.playerStats.savedEq && GameManager.instance.level == 1)
        {
            SetUpThermo(GameManager.instance.playerStats.pociatocna);
            InstantiateStones(GameManager.instance.playerStats.kamene, new List<string>());
            return GameManager.instance.playerStats.GetEquasion(1);
        }

        else if (GameManager.instance.playerStats.savedEq2 && GameManager.instance.level == 2)
        {
            SetUpThermo(GameManager.instance.playerStats.pociatocna2);
            InstantiateStones(GameManager.instance.playerStats.kamene2, GameManager.instance.playerStats.znamienka2);
            return GameManager.instance.playerStats.GetEquasion(2);
        }

        else
        {
            return FirstLevelEquasion(x);
        }
       
        ////////// SAVED EQ
        //return FirstLevelEquasion(x);           
    }

    public int GetAnswer()
    {
        if(GameManager.instance.level == 1) return GameManager.instance.playerStats.finalna;
        else return GameManager.instance.playerStats.finalna2;
    }
}
