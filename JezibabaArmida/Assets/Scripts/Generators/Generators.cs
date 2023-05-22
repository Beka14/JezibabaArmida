using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generators : MonoBehaviour
{
    public List<List<int>> GenerateFirstDiophine(int stonesCount, int minS, int maxS)
    {
        int firstTemp;
        int cold = 0;
        int hot = 0;
        int finalTemp = 0;

        firstTemp = Random.Range(1, 68);

        List<int[]> pocty = SplitNumber(stonesCount);
        int r = Random.Range(0, pocty.Count);
        cold = pocty[r][0];
        hot = pocty[r][1];

        finalTemp = MakeVysledna(firstTemp, Gcd(cold, hot));
        int fin = finalTemp - firstTemp;

        int[] p = FindLowestXY(cold, hot, fin);
        int[] x = Generate2(cold, hot, p[0], p[1], minS, maxS);
        int index = 0;
        while (x[0] == -100 && index < 30)
        {
            index++;
            firstTemp = Random.Range(1, 68);
            r = Random.Range(0, pocty.Count);
            cold = pocty[r][0];
            hot = pocty[r][1];

            finalTemp = MakeVysledna(firstTemp, Gcd(cold, hot));
            fin = finalTemp - firstTemp;
            p = FindLowestXY(cold, hot, fin);
            x = Generate2(cold, hot, p[0], p[1], minS, maxS);
            if (x[0] != -100) break;
        }

        List<int> k = new List<int>();
        for (int i = 0; i < hot; i++) k.Add(x[1]);
        for (int j = 0; j < cold; j++) k.Add(x[0]);

        return new List<List<int>>() { k, new List<int> { firstTemp, finalTemp } };
    }

    public int[] Generate(int a, int b, int x0, int y0, int min, int max)
    {
        int d = Gcd(a, b);
        int t = 0;
        while (true)
        {
            if (x0 <= max && y0 <= max && x0 >= -min && y0 >= -min && ((x0 > 0 && y0 < 0) || (x0 < 0 && y0 > 0)))
            {   
                return new int[] { x0, y0 };
            }
            else
            {
                int x = x0 + (b / d) * (t + 1);
                int y = y0 - (a / d) * (t + 1);
                if (x0 <= max && y0 <= max && x0 >= -min && y0 >= -min && ((x > 0 && y < 0) || (x < 0 && y > 0)))
                {
                    x0 = x;
                    y0 = y;
                }
                else
                {
                    x = x0 + (b / d) * (t - 1);
                    y = y0 - (a / d) * (t - 1);
                    if (x0 <= max && y0 <= max && x0 >= -min && y0 >= -min && ((x > 0 && y < 0) || (x < 0 && y > 0)))
                    {
                        x0 = x;
                        y0 = y;
                    }
                    else
                    {
                        return new int[] { -100, -100 };
                    }
                }
            }
            t++;
        }
    }

    public int[] Generate2(int a, int b, int x0, int y0, int min, int max)
    {
        int d = Gcd(a, b);
        int t = 0;
        while (true)
        {
            if (CheckMinMax(x0, y0, min, max))
            {   
                return new int[] { x0, y0 };
            }
            else
            {
                int x = x0 + (b / d) * (t + 1);
                int y = y0 - (a / d) * (t + 1);
                if (CheckMinMax(x, y, min, max))
                {
                    x0 = x;
                    y0 = y;
                }
                else
                {
                    x = x0 + (b / d) * (t - 1);
                    y = y0 - (a / d) * (t - 1);
                    if (CheckMinMax(x, y, min, max))
                    {
                        x0 = x;
                        y0 = y;
                    }
                    else if (t >= 5)        
                    {
                        return new int[] { -100, -100 };
                    }
                }
            }
            t++;
        }
    }

    public int[] FindLowestXY(int a, int b, int c)
    {
        int[] vals = ExtendedEuclidean(a, b);
        int d = vals[0];
        int p = vals[1];
        int q = vals[2];
        if (c % d != 0)
        {
            return new int[] { -100, -100 };
        }
        int x0 = p * (c / d);
        int y0 = q * (c / d);
        return new int[] { x0, y0 };
    }

    public int[] ExtendedEuclidean(int a, int b)
    {
        if (b == 0)
        {
            return new int[] { a, 1, 0 };
        }
        int[] vals = ExtendedEuclidean(b, a % b);
        int d = vals[0];
        int p = vals[2];
        int q = vals[1] - (a / b) * vals[2];
        return new int[] { d, p, q };
    }

    public int Gcd(int a, int b)
    {
        if (b == 0)
        {
            return a;
        }
        return Gcd(b, a % b);
    }

    public int MakeVysledna(int firstTemp, int d)
    {
        int vysledna = Random.Range(firstTemp, firstTemp + 30);
        int fin = vysledna - firstTemp;
        while (fin % d != 0 || vysledna == firstTemp)
        {
            vysledna = Random.Range(firstTemp, firstTemp + 30);
            fin = vysledna - firstTemp;
        }
        return vysledna;
    }

    public List<int[]> SplitNumber(int n)
    {
        List<int[]> l = new List<int[]>();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i + j == n)
                {
                    int[] x = new int[] { i, j };
                    Array.Sort(x);
                    if (!l.Contains(x))
                    {
                        l.Add(x);
                    }
                }
            }
        }
        return l;
    }

    public bool CheckMinMax(int x, int y, int min, int max)
    {
        int[] p = new int[] { x, y };
        Array.Sort(p);
        x = p[0];
        y = p[1];
        if (x <= -min && x >= -max && y >= min && y <= max)
        {
            return true;
        }

        return false;
    }
}
