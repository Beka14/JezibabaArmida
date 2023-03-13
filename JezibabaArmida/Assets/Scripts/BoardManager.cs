using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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

    [SerializeField] GameObject drag_studeny;
    [SerializeField] GameObject drag_horuci;
    [SerializeField] GameObject drag_studeny2;
    [SerializeField] GameObject drag_horuci2;

    GameObject thermometer;
    Thermometer thermoScript;
    NumberSlider numberSlider;

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
        numberSlider = GameObject.Find("Numbers").GetComponent<NumberSlider>();
        if(TryGetComponent<Thermometer>(out Thermometer thermoScript)) thermoScript = thermometer.GetComponent<Thermometer>();
        //GetComponent<GameManager>().Init();
    }
    
    public string FirstLevelEquasion(int mink, int maxk, int pocet_kamenov, bool zatvorka = false, int pocet_zatvorke = 2, bool minus = false, bool vymena_znam = false)
    {
        List<int> list = new List<int>();
        List<int> kamene = new List<int>();
        int max = 68;
        int min = 1;
        bool ok = false;

        vsetky = new List<int>();
        znamienka = new List<string>();
        zatvorky = new Dictionary<int, string>();

        int pociatocna = Random.Range(min, max + 1);
        int studenyKamen = Random.Range(-maxk,-mink); //studenyKamen = Random.Range(1, 5) * -1;
        int horuciKamen = Random.Range(mink, maxk);
        int vysledna = Random.Range(min, max + 1);

        while (!ok)
        {
            pociatocna = Random.Range(-min, max+1);
            while(studenyKamen==0) studenyKamen = Random.Range(-maxk, -mink); //studenyKamen = Random.Range(1, 5) * -1;
            while(horuciKamen == 0 || horuciKamen == studenyKamen*-1) horuciKamen = Random.Range(mink, maxk+1);
            vysledna = Random.Range(-min, max + 1);

            for(int x=1; x < 1000; x++)
            {
                for(int y=1; y < 1000; y++)
                {
                    if ((pociatocna + (horuciKamen * x) + (studenyKamen * y) == vysledna) && x + y == pocet_kamenov)
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
        //zatvorky
        if (kamene.Count() > 3 && zatvorka)
        {
            int z = Random.Range(pocet_zatvorke-2, pocet_zatvorke);
            for(int i = z; i >=0 ; i--)                      //for(int i = 0; i < z; i++)
            {
                int x = Random.Range(0, kamene.Count() -1);
                pole.Add(kamene[x]);        //problem??
                kamene.RemoveAt(x);
            }
        }

        string tmp;
        if (GameManager.instance.level == 1) tmp = Stringify(pole, kamene);
        else
        {
            answer = pociatocna;
            tmp = Stringify_lvl2(pole, kamene, minus, vymena_znam);
        } 

        //TODO 

        SetUpThermo(pociatocna);
        InstantiateStones(vsetky, znamienka);
        if(numberSlider == null) numberSlider = GameObject.Find("Numbers").GetComponent<NumberSlider>();
        numberSlider.SetMinMax(vysledna);

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
            GameManager.instance.playerStats.zatvorky = zatvorky;
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



    string Stringify_lvl2(List<int> a, List<int> b, bool minus, bool vymena)
    {
        int index = 0;
        string sb = "";
        int r = (vymena)? Random.Range(1, b.Count()-1):0;
        //vymenenie znamienok pre kamene
        for (int i = 0; i < r; i++)
        {
            sb += (b[i] < 0) ? "-" + -1 * b[i] : "+" + b[i];
            vsetky.Add(b[i]);
            answer += b[i];
            znamienka.Add((b[i] < 0) ? "+" : "+");
            index++;
        }

        //a su kamene v zatvorke
        if (a.Count() != 0)
        {
            int x = 0;
            int p = Random.Range(1, 3);
            if (p % 2 == 0 && minus)
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
            //Debug.Log("------------3 " + string.Join(",", zatvorky));
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

    public void SetUpThermoLVL3(int value)
    {
        GameObject thermometer2 = GameObject.Find("Slider2");
        Thermometer thermo = thermometer2.GetComponent<Thermometer>();
        thermo.SetValue(value);
    }

    public void InstantiateStones(List<int> stones, List<string> znam)     
    {
        bool poZatvorke = false;
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
                g.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                g = Instantiate(horuci);
                TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (stones[i] == 1) ? "" : stones[i] + "";
                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
                g.transform.localScale = new Vector3(1, 1, 1);
            }

            index++;
            naPloche.Add(g);

            if (zatvorky.ContainsKey(index))
            {
                if (zatvorky[index] == "+(")
                {
                    g = Instantiate(plusPar);
                    g.transform.localScale = new Vector3(1, 1, 1);
                    poZatvorke = true;
                }

                else if (zatvorky[index] == "-(") 
                {
                    g = Instantiate(minusPar);
                    poZatvorke = true;
                }

                else g = Instantiate(endPar);

                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
                g.transform.localScale = new Vector3(1, 1, 1);
                naPloche.Add(g);
            }

            if (znam.Count() != 0 && i+1 < znam.Count())
            {
                //Debug.Log("------------ " + string.Join(",", znamienka));
                if (znam[i+1] == "-")
                {
                    if (poZatvorke)
                    {
                        poZatvorke = false;
                    }
                    g = Instantiate(minus);
                    g.transform.SetParent(kamene.transform);
                    g.transform.SetAsLastSibling();
                    g.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                }
                else
                {
                    if (poZatvorke)
                    {
                        poZatvorke = false;
                        continue;
                    }
                    g = Instantiate(plus);
                    g.transform.SetParent(kamene.transform);
                    g.transform.SetAsLastSibling();
                    g.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                }

                naPloche.Add(g);
            }

            //naPloche.Add(g);
            //g.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public string SetUpBoard(int x)
    {
        if(GameManager.instance.level != 3)
        {
            GameObject g = GameObject.Find("kamene");
            for (var i = g.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(g.transform.GetChild(i).gameObject);
            }
        }

        else if(GameManager.instance.playerStats.zaporne)
        {
            GameObject gg = GameObject.Find("kamene2");
            for (var i = gg.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(gg.transform.GetChild(i).gameObject);
            }
        }

        else if(GameManager.instance.level == 3)
        {
            for(int i = 1; i <= 3; i++)
            {
                GameObject gg = GameObject.Find("kamene"+i+"i");
                for (var ii = gg.transform.childCount - 1; ii >= 0; ii--)
                {
                    Object.Destroy(gg.transform.GetChild(ii).gameObject);
                }
            }
        }

        ////////// SAVED EQ

        if (GameManager.instance.playerStats.savedEq && GameManager.instance.level == 1)
        {
            SetUpThermo(GameManager.instance.playerStats.pociatocna);
            zatvorky = new Dictionary<int, string>();
            InstantiateStones(GameManager.instance.playerStats.kamene, new List<string>());
            return GameManager.instance.playerStats.GetEquasion(1);
        }

        else if (GameManager.instance.playerStats.savedEq2 && GameManager.instance.level == 2)
        {
            SetUpThermo(GameManager.instance.playerStats.pociatocna2);
            zatvorky = GameManager.instance.playerStats.zatvorky;
            InstantiateStones(GameManager.instance.playerStats.kamene2, GameManager.instance.playerStats.znamienka2);
            return GameManager.instance.playerStats.GetEquasion(2);
        }

        else if (GameManager.instance.playerStats.savedEq3 && GameManager.instance.level == 3)
        {
            /*
            if(GameManager.instance.playerStats.level_3 == 2)
            {
                GameManager.instance.playerStats.savedEq3 = false;
                ThirdLevelEquasion(true);
                return "";
            }
            */

            SetUpThermo(GameManager.instance.playerStats.pociatocna3);
            InstantiateStonesLVL3(GameManager.instance.playerStats.kameneNaPloche, GameManager.instance.playerStats.zaporne);
            //SetUpFinal(GameManager.instance.playerStats.finalna3);
            SetUpThermoLVL3(GameManager.instance.playerStats.finalna3);
            SetUpFinal(GameManager.instance.playerStats.finalna3);
            SetUpThermo(GameManager.instance.playerStats.pociatocna3);
            SetUpSolutionsNumber(GameManager.instance.playerStats.solutionsGot, GameManager.instance.playerStats.solutionsAll);
            //TODO naspat dat solutions do lvl3managera
            GameManager.instance.lvl3man.InstantiateAnswersStart(GameManager.instance.playerStats.solved, GameManager.instance.playerStats.answers);

            //GameManager.instance.lvl3man.SetUpAnswers(GameManager.instance.playerStats.solved, GameManager.instance.playerStats.answers);
            return "";
        }

        else
        {
            if (GameManager.instance.level == 3)
            {
                int[] b = GetTaskBounds(GameManager.instance.level);
                //GameManager.instance.lvl3man.ClearAnswers();
                /*
                if (GameManager.instance.playerStats.level_3 == 1)
                {
                    ThirdLevelEquasion(true);
                    return "";
                }
                */

                ThirdLevelEquasion((b[0] == 1) ? true : false, (b[1] == 1) ? true : false, b[2], b[3], b[4], b[5]);
                return "";
            }

            else if(GameManager.instance.level == 2)
            {
                int[] b = GetTaskBounds(GameManager.instance.level);
                return FirstLevelEquasion(b[0], b[1], b[2], (b[3]==1)?true:false, b[4], (b[5] == 1) ? true : false, (b[6] == 1) ? true : false);
            }


            else 
            {
                int[] b = GetTaskBounds(GameManager.instance.level);
                return FirstLevelEquasion(b[0], b[1], b[2]);
            }
        }
       
        ////////// SAVED EQ
        //return FirstLevelEquasion(x);           
    }

    private int[] GetTaskBounds(int level)
    {
        if(level == 1)
        {
            int prog = GameManager.instance.playerStats.level_1;
            int x = (int)Math.Ceiling((double)((prog + 1) / 2));
            int min = x;
            int max = (x+3) - (x%3);
            return new int[]{min,max,x+3};
        }

        if(level == 2)
        {
            int prog = GameManager.instance.playerStats.level_2;
            int x = (int)Math.Ceiling((double)((prog + 1) / 2));
            int min = x;
            int max = x + 3;
            //bool zatvorka = false, int pocet_zatvorke = 2, bool minus = false, bool vymena_znam = false
            int zatvorka = (prog >= 5) ? 1 : 0;
            int pocetzat = x - 1;
            int minus = (prog >= 7) ? 1 : 0;
            int vymen = (prog >= 3) ? 1 : 0;
            int pocet = (prog >= 5) ? x + 2 : x + 3;
            return new int[] { min, max, pocet, zatvorka, pocetzat, minus, vymen};
        }

        if(level == 3)
        {
            //bool zaporne = false, bool twoStones = false, int mink = 2, int maxk = 8, int mins = 3, int maxs = 6
            int prog = GameManager.instance.playerStats.level_3;
            int[] p = {3,3,4,4,5,5,6,6,6,6};
            int x = (int)Math.Ceiling((double)((prog + 1) / 2));
            int zaporne = (prog >= 4)? 1 : 0;
            if (zaporne==1) GameManager.instance.lvl3man.TurnOnButton();
            else GameManager.instance.lvl3man.TurnOffButton();
            int two = (prog < 2 || (prog > 4 && prog < 8))? 1 : 0;
            return new int[] {zaporne, two, 2, 8, p[prog], p[prog]};
        }

        return new int[] { 5, 10, 5 };
    }

    public int GetAnswer()
    {
        if(GameManager.instance.level == 1) return GameManager.instance.playerStats.finalna;
        else if(GameManager.instance.level == 2) return GameManager.instance.playerStats.finalna2;
        else return GameManager.instance.playerStats.pociatocna3;
    }

    public void ThirdLevelEquasion(bool zaporne = false, bool twoStones = false, int mink = 2, int maxk = 8, int mins = 3, int maxs = 6)
    {
        //zaporne = true;                     //TODO po debugu vymazat
        List<int> list = new List<int>();
        List<int> kamene = new List<int>();
        List<List<int>> solved = new List<List<int>>();
        int max = 68;
        int min = 11;
        bool ok = false;

        int pociatocna = Random.Range(min, max + 1);
        int prvy = Random.Range(mink, maxk);
        int druhy = Random.Range(mink, maxk);
        int treti = Random.Range(mink, maxk);
        int vysledna = Random.Range(pociatocna, pociatocna + 20);

        if(!zaporne) while (!ok)
        {
            solved = new List<List<int>>();
            pociatocna = Random.Range(min, max + 1);
            prvy = Random.Range(mink, maxk);
            vysledna = Random.Range(pociatocna, pociatocna + 20);
            while (prvy == druhy) druhy = Random.Range(mink, maxk);
            while (treti == druhy || treti == prvy) treti = Random.Range(mink, maxk);
            while (vysledna == pociatocna) vysledna = Random.Range(pociatocna, pociatocna + 20);    //TODO ZAPORNE CISLO

            Debug.Log(pociatocna + " + " + prvy + " + " + druhy + " + " + treti + " = " + vysledna);

            int target = vysledna - pociatocna;
            List<List<int>> result = (twoStones)? CombinationSum(new int[]{druhy,treti}, target, false): CombinationSum(new int[] {prvy, druhy, treti }, target, false);
            if (result.Count() < mins || result.Count > maxs)
            {
                continue;
            }
            else if (result.Count() >= mins && result.Count() <= maxs && result.Count != 0)
            {
                ok = true;
                solved = result;
                kamene = (twoStones)? new List<int> {druhy, treti }:new List<int> { prvy, druhy, treti };
            }
        }

        else while (!ok)
        {
            solved = new List<List<int>>();
            pociatocna = Random.Range(min, max + 1);
            prvy = Random.Range(2, 9);
            druhy = Random.Range(-5, 8);
            treti = -1 * Random.Range(2, 8);        //TODO skusit s 2 kamenmi
            while (prvy == druhy || druhy == -1 || druhy == 1 || druhy == 0) druhy = Random.Range(-5, 8);
            while (treti == druhy || treti == prvy) treti = -1 * Random.Range(2, 8);
            while (vysledna == pociatocna) vysledna = Random.Range(pociatocna, pociatocna + 20);    //TODO ZAPORNE CISLO
            Debug.Log(pociatocna + " + " + prvy + " + " + druhy + " + " + treti + " = " + vysledna);

            int target = vysledna - pociatocna;
            List<List<int>> result = (twoStones) ? CombinationSum(new int[] { prvy, treti }, target, true) : CombinationSum(new int[] { prvy, druhy, treti }, target, true);
            if (result.Count() < 3 || result.Count > 6)
            {
                continue;
            }
            else if (result.Count() >= 3 && result.Count() <= 6)
            {
                ok = true;
                solved = result;
                kamene = MakeStones(result);
            }
        }

        Debug.Log("------------ " + solved.Count());
        // SOLVED INSTANTIOATE
        if (GameManager.instance.lvl3man == null) GameManager.instance.lvl3man = GameObject.Find("LVL3Manager").GetComponent<LVL3Manager>();
        GameManager.instance.lvl3man.SetUpAnswers(solved, new List<List<int>>());
        //

        //TODO 

        SetUpThermoLVL3(vysledna);
        SetUpFinal(vysledna);
        SetUpThermo(pociatocna);
        SetUpSolutionsNumber(0,solved.Count());
        InstantiateStonesLVL3(kamene, zaporne);

        //

        // SAVE EQ

        GameManager.instance.playerStats.kamene3 = kamene;
        GameManager.instance.playerStats.kameneNaPloche = kamene;
        GameManager.instance.playerStats.solved = solved;
        GameManager.instance.playerStats.finalna3 = vysledna;
        GameManager.instance.playerStats.pociatocna3 = pociatocna;
        GameManager.instance.playerStats.savedEq3 = true;
        GameManager.instance.playerStats.zaporne = zaporne;
        GameManager.instance.playerStats.answers = new List<List<int>>();
        GameManager.instance.playerStats.solutionsGot = 0;
        GameManager.instance.playerStats.solutionsAll = solved.Count();
        GameManager.instance.playerStats.level = GameManager.instance.level;

        //

        //Debug.Log("POROVNANIE: poc: " + vysledna + " answ: " + answer);
        //return pociatocna + "" + tmp + " = " + answer;
    }

    List<int> MakeStones(List<List<int>> s)
    {
        List<int> k = new List<int>();
        foreach(List<int> v in s)
        {
            foreach(int w in v) k.Add(w);   
        }

        for(int i=0; i < k.Count; i++)
        {
            int tmp = k[i];
            int ri = Random.Range(i, k.Count);
            k[i] = k[ri];
            k[ri] = tmp;
        }

        return k;
    }

    public void InstantiateStonesLVL3(List<int> stones, bool zaporne)
    {
        if (!zaporne)
        {
            kamene = GameObject.Find("kamene");
        }
        else 
        {
            kamene = GameObject.Find("kamene2");
        }

        GameObject g;

        for (int i = 0; i < stones.Count; i++)
        {
            if(!zaporne) kamene = GameObject.Find("kamene"+(i+1)+"i");

            if (stones[i] < 0)
            {
                g = (zaporne)? Instantiate(drag_studeny2):Instantiate(drag_studeny);
                TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (stones[i] == -1) ? "" : stones[i] * -1 + "";
                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
                g.name = "drag_studeny2";
                g.transform.localScale = new Vector3(1, 1, 1);
                naPloche.Add(g);
            }
            else if (stones[i] != 0)
            {
                g = (zaporne)? Instantiate(drag_horuci2) : Instantiate(drag_horuci);
                TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (stones[i] == 1) ? "" : stones[i] + "";
                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
                g.name = "drag_horuci2";
                g.transform.localScale = new Vector3(1, 1, 1);
                naPloche.Add(g);
            }

        }
    }

    void SetUpFinal(int i)
    {
        Debug.Log("finalna: " + i);
        TextMeshProUGUI t = GameObject.Find("final").GetComponent<TextMeshProUGUI>();
        t.text = i + "";
    }

    void SetUpSolutionsNumber(int ii, int i)
    {
        TextMeshProUGUI t = GameObject.Find("solutions").GetComponent<TextMeshProUGUI>();
        t.text = ii + "/" + i;
    }

    public static List<List<int>> CombinationSum(int[] nums, int target, bool depth)
    {
        List<List<int>> result = new List<List<int>>();
        Array.Sort(nums);
        Backtrack(nums, target, 0, new List<int>(), result, 0, depth);
        return result;
    }

    private static void Backtrack(int[] nums, int target, int start, List<int> list, List<List<int>> result, int depth, bool d)
    {
        if (target == 0)
        {
            result.Add(new List<int>(list));
        }
        if (depth >= 10 && d) return;
        else if (target > 0)
        {
            for (int i = start; i < nums.Length && nums[i] <= target; i++)
            {
                if (nums[i] < 0 && (depth >= 10 && d))    //&& depth >= 8
                {
                    continue;
                }
                list.Add(nums[i]);
                Backtrack(nums, target - nums[i], i, list, result, depth + 1, d);
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
