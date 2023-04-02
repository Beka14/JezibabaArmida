using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FirstLevelGenerator : MonoBehaviour
{
    public int answer;
    List<int> vsetky = new List<int>();
    public List<string> znamienka = new List<string>();
    public Dictionary<int, string> zatvorky = new Dictionary<int, string>();
    List<List<int>> zasobnik = new List<List<int>>();

    Generators generators;
    NumberSlider numberSlider;
    public string FirstLevelEquasion(int mink, int maxk, int pocet_kamenov, bool zatvorka = false, int pocet_zatvorke = 2, bool minus = false, bool vymena_znam = false)
    {
        List<int> kamene = new List<int>();
        int max = 68;
        int min = 1;
        bool ok = false;

        vsetky = new List<int>();
        znamienka = new List<string>();
        zatvorky = new Dictionary<int, string>();

        int pociatocna = Random.Range(min, max + 1);
        int studenyKamen = Random.Range(-maxk, -mink); 
        int horuciKamen = Random.Range(mink, maxk);
        int vysledna = Random.Range(min, max + 1);

        while (!ok)
        {
            pociatocna = Random.Range(-min, max + 1);
            while (studenyKamen == 0) studenyKamen = Random.Range(-maxk, -mink); 
            while (horuciKamen == 0 || horuciKamen == studenyKamen * -1) horuciKamen = Random.Range(mink, maxk + 1);
            vysledna = Random.Range(-min, max + 1);

            for (int x = 1; x < 1000; x++)
            {
                for (int y = 1; y < 1000; y++)
                {
                    if ((pociatocna + (horuciKamen * x) + (studenyKamen * y) == vysledna) && x + y == pocet_kamenov)
                    {
                        kamene = new List<int>();
                        ok = true;
                        for (int i = 0; i < x; i++) kamene.Add(horuciKamen);
                        for (int j = 0; j < y; j++) kamene.Add(studenyKamen);
                        answer = vysledna;
                        break;
                    }
                }
            }
        }

        if (generators == null) generators = gameObject.GetComponent<Generators>();
        //Debug.Log(pocet_kamenov + " " + mink + " " + maxk);
        List<List<int>> gen = generators.GenerateFirstDiophine(pocet_kamenov, mink, maxk);

        
        if (gen[0][0] != -100 && !ContainsItem(gen[0]))
        {
            vysledna = gen[1][1];
            pociatocna = gen[1][0];
            kamene = new List<int>();
            kamene.AddRange(gen[0]);
            answer = vysledna;
        }
        

        zasobnik.Add(kamene);

        List<int> pole = new List<int>();
        //zatvorky
        if (kamene.Count() > 3 && zatvorka)
        {
            int z = Random.Range(pocet_zatvorke - 2, pocet_zatvorke);
            for (int i = z; i >= 0; i--)                      
            {
                int x = Random.Range(0, kamene.Count() - 1);
                pole.Add(kamene[x]);        
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
        /*
        GameManager.instance.boardScript.SetUpThermo(pociatocna);
        if (numberSlider == null) numberSlider = GameObject.Find("Numbers").GetComponent<NumberSlider>();
        GameManager.instance.boardScript.InstantiateStones(vsetky, znamienka);
        */
        //

        // SAVE EQ

        if (GameManager.instance.level == 1)
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

        GameManager.instance.boardScript.SetUpThermo(pociatocna);
        if (numberSlider == null) numberSlider = GameObject.Find("Numbers").GetComponent<NumberSlider>();
        GameManager.instance.boardScript.InstantiateStones(vsetky, znamienka);

        return pociatocna + "" + tmp + " = " + answer;
    }

    bool ContainsItem(List<int> z)
    {
        foreach (List<int> i in zasobnik)
        {
            if (string.Join(",", i) == string.Join(",", z)) return true;
        }
        return false;
    }

    string Stringify(List<int> a, List<int> b)
    {
        string sb = "";
        int r = Random.Range(0, b.Count());
        for (int i = 0; i < r; i++)
        {
            sb += (b[i] < 0) ? " - " + -1 * b[i] : " + " + b[i];
            vsetky.Add(b[i]);
        }

        if (a.Count() != 0)
        {
            int z = Random.Range(1, 3);
            if (z % 2 == 0)
            {
                sb += " + (";
            }
            else sb += " + (";
            for (int i = 0; i < a.Count(); i++)
            {
                sb += (a[i] < 0) ? "-" + -1 * a[i] : "+" + a[i];        
                vsetky.Add(a[i]);
            }
            sb += ")";
        }

        for (int i = r; i < b.Count(); i++) 
        {
            sb += (b[i] < 0) ? " - " + -1 * b[i] : " + " + b[i];
            vsetky.Add(b[i]);
        }

        return sb;
    }

    string Stringify_lvl2(List<int> a, List<int> b, bool minus, bool vymena)
    {
        int index = 0;
        string sb = "";
        int r = (vymena) ? Random.Range(1, b.Count() - 1) : 0;
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
            if (p % 2 == 0)
            {
                sb += "-(";
                zatvorky.Add(index, "-(");
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
                answer += -1 * x;
                sb += ")";
                zatvorky.Add(index, ")");
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
        }

        for (int i = r; i < b.Count(); i++) 
        {
            int z = Random.Range(1, 4);
            if (z % 3 == 0 && vymena)
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

        return sb;
    }
}
