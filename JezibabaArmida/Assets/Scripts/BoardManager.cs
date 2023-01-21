using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public string FirstLevelEquasion(int pocet_kamenov)
    {
        List<int> list = new List<int>();
        List<int> kamene = new List<int>();
        int max = 48;
        int min = 11;
        bool ok = false;

        int pociatocna = Random.Range(min, max + 1);
        int studenyKamen = Random.Range(1, 5) * -1;
        int horuciKamen = Random.Range(1, 5);
        int vysledna = Random.Range(min, max + 1);

        while (!ok)
        {
            pociatocna = Random.Range(min, max+1);
            studenyKamen = Random.Range(1, 5) * -1;
            horuciKamen = Random.Range(1, 5);
            vysledna = Random.Range(min, max + 1);

            for(int x=0; x < 1000; x++)
            {
                for(int y=0; y < 1000; y++)
                {
                    if (pociatocna + (horuciKamen * x) + (studenyKamen * y) == vysledna && x + y <= pocet_kamenov && x != 0 && y != 0)
                    {
                        kamene = new List<int>();
                        ok = true;
                        for (int i = 0; i < x; i++) kamene.Add(horuciKamen);
                        for (int j = 0; j < y; j++) kamene.Add(studenyKamen);
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
                int x = Random.Range(0, kamene.Count);
                pole.Add(kamene[x]);
                kamene.RemoveAt(x);
            }
        }

        return pociatocna + Stringify(pole,kamene) + " = " + vysledna; 
    }

    string Stringify(List<int> a, List<int> b)
    {
        string sb = "";
        int r = Random.Range(0,b.Count);
        for(int i=0;i<r;i++)
        {
            sb += (b[i] < 0) ? " - " + -1*b[i] : " + "+b[i];
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
            }
            sb += ")";
        }

        for (int i = r; i < b.Count - r; i++)
        {
            sb += (b[i] < 0) ? " - " + -1 * b[i] : " + " + b[i];
        }

        return sb;
    }

    //TODO: NASTAVIT TEPLOMER

    public string SetUpBoard(int i)
    {
        return FirstLevelEquasion(i);
    }
}
