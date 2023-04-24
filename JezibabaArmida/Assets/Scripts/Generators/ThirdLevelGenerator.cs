using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThirdLevelGenerator : MonoBehaviour
{
    List<int[]> zasobnik = new List<int[]>();
    List<int[]> zasobnik2 = new List<int[]>();
    public void ThirdLevelEquasion(bool zaporne = false, bool twoStones = false, int mink = 2, int maxk = 8, int mins = 3, int maxs = 6)
    {
        List<int> kamene = new List<int>();
        List<List<int>> solved = new List<List<int>>();
        int max = 68;
        int min = 11;
        bool ok = false;

        int pociatocna = Random.Range(min, max + 1);
        int prvy;
        int druhy = Random.Range(mink, maxk);
        int treti = Random.Range(mink, maxk);
        int vysledna = Random.Range(pociatocna, pociatocna + 25);

        if (!zaporne) while (!ok)
            {
                solved = new List<List<int>>();
                pociatocna = Random.Range(min, max + 1);
                prvy = Random.Range(mink, maxk);
                vysledna = Random.Range(pociatocna+1, pociatocna + 20);
                while (prvy == druhy) druhy = Random.Range(mink, maxk);
                while (treti == druhy || treti == prvy) treti = Random.Range(mink, maxk);
                while (vysledna == pociatocna) vysledna = Random.Range(pociatocna, pociatocna + 25);

                //Debug.Log(pociatocna + " + " + prvy + " + " + druhy + " + " + treti + " = " + vysledna);

                int target = vysledna - pociatocna;
                List<List<int>> result = (twoStones) ? CombinationSum(new int[] { druhy, treti }, target, false) : CombinationSum(new int[] { prvy, druhy, treti }, target, false);
                
                if (result.Count() >= mins && result.Count() <= maxs && result.Count != 0)
                {
                    kamene = (twoStones) ? new List<int> { druhy, treti } : new List<int> { prvy, druhy, treti };
                    int[] k = kamene.ToArray();
                    Array.Sort(k);
                    if (ContainsItem(k)) continue;
                    ok = true;
                    solved = result;
                    zasobnik.Add(k);
                }
            }

        else while (!ok)
            {
                solved = new List<List<int>>();
                pociatocna = Random.Range(min, max + 1);
                prvy = Random.Range(2, 9);
                druhy = Random.Range(-5, 8);
                treti = -1 * Random.Range(2, 8);
                vysledna = Random.Range(pociatocna+1, pociatocna + 25);
                while (prvy == druhy || prvy == druhy * -1 || druhy == -1 || druhy == 1 || druhy == 0) druhy = Random.Range(-5, 8);
                while (treti == druhy || treti * -1 == prvy) treti = -1 * Random.Range(2, 8);
                while (vysledna == pociatocna) vysledna = Random.Range(pociatocna, pociatocna + 25);
                //Debug.Log(pociatocna + " + " + prvy + " + " + druhy + " + " + treti + " = " + vysledna);

                int target = vysledna - pociatocna;
                List<List<int>> result = (twoStones) ? CombinationSum(new int[] { prvy, treti }, target, true) : CombinationSum(new int[] { prvy, druhy, treti }, target, true);
                
                if (result.Count() >= 3 && result.Count() <= 6)
                {
                    int[] k = (twoStones) ? new int[]{ prvy, treti } : new int[]{ prvy, druhy, treti };
                    Array.Sort(k);
                    if (ContainsItem(k)) continue;
                    ok = true;
                    solved = result;
                    kamene = MakeStones(result);
                    zasobnik.Add(k);
                }
            }

        // SOLVED INSTANTIOATE
        if (GameManager.instance.lvl3man == null) GameManager.instance.lvl3man = GameObject.Find("LVL3Manager").GetComponent<LVL3Manager>();
        GameManager.instance.lvl3man.SetUpAnswers(solved, new List<List<int>>());
        //

        //TODO 

        GameManager.instance.boardScript.SetUpThermoLVL3(vysledna);
        GameManager.instance.boardScript.SetUpFinal(vysledna);
        GameManager.instance.boardScript.SetUpThermo(pociatocna);
        GameManager.instance.boardScript.SetUpSolutionsNumber(0, solved.Count());
        GameManager.instance.boardScript.InstantiateStonesLVL3(kamene, zaporne);

        //

        // SAVE EQ

        GameManager.instance.playerStats.kamene3 = new List<int>();
        GameManager.instance.playerStats.kamene3.AddRange(kamene);
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

    }

    public void FourthLevelEquasion(bool twoStones = false, int mink = 2, int maxk = 8, int mins = 3, int maxs = 6)
    {
        List<int> kamene = new List<int>();
        List<List<int>> solved = new List<List<int>>();
        int max = 68;
        int min = 11;
        bool ok = false;

        int pociatocna = Random.Range(min, max + 1);
        int prvy;
        int druhy = Random.Range(mink, maxk);
        int treti = Random.Range(mink, maxk);
        int vysledna = Random.Range(pociatocna+1, pociatocna + 25);
        int r = Random.Range(1, 7);

        if (r % 2 == 0) while (!ok)
            {
                solved = new List<List<int>>();
                pociatocna = Random.Range(min, max + 1);
                prvy = Random.Range(mink, maxk);
                vysledna = Random.Range(pociatocna+1, pociatocna + 25);
                while (prvy == druhy) druhy = Random.Range(mink, maxk);
                while (treti == druhy || treti == prvy) treti = Random.Range(mink, maxk);
                while (vysledna == pociatocna) vysledna = Random.Range(pociatocna + 1, pociatocna + 25);
                int target = vysledna - pociatocna;

                if (twoStones && target % Gcd(druhy, treti) != 0 && r % 3 == 0)
                {
                    int[] k = new int[] { druhy, treti };
                    Array.Sort(k);
                    if (ContainsItemFourth(k)) continue;
                    GameManager.instance.playerStats.noSolutions = true;
                    ok = true;
                    kamene = new List<int> { druhy, treti };
                    zasobnik2.Add(k);
                    break;
                }

                List<List<int>> result = (twoStones) ? CombinationSum(new int[] { druhy, treti }, target, false) : CombinationSum(new int[] { prvy, druhy, treti }, target, false);
                
                if (result.Count() >= mins && result.Count() <= maxs && result.Count != 0)
                {
                    int[] k = (twoStones) ? new int[] { druhy, treti } : new int[] { prvy, druhy, treti };
                    Array.Sort(k);
                    if (ContainsItemFourth(k)) continue;
                    ok = true;
                    solved = result;
                    zasobnik2.Add(k);
                    kamene = (twoStones) ? new List<int> { druhy, treti } : new List<int> { prvy, druhy, treti };
                }
            }

        else
            while (!ok)
            {
                solved = new List<List<int>>();
                pociatocna = Random.Range(min, max + 1);
                prvy = Random.Range(2, 9);
                druhy = Random.Range(-5, 8);
                treti = -1 * Random.Range(2, 8);
                vysledna = Random.Range(pociatocna + 1, pociatocna + 25);
                while (prvy == druhy || prvy == druhy * -1 || druhy == -1 || druhy == 1 || druhy == 0) druhy = Random.Range(-5, 8);
                while (treti == druhy || treti * -1 == prvy) treti = -1 * Random.Range(2, 8);
                while (vysledna == pociatocna) vysledna = Random.Range(pociatocna + 1, pociatocna + 25);
                int target = vysledna - pociatocna;

                if (twoStones && target % Gcd(prvy, treti) != 0 && r %3 == 0)
                {
                    int[] k = new int[] { prvy, treti };
                    Array.Sort(k);
                    GameManager.instance.playerStats.noSolutions = true;
                    ok = true;
                    zasobnik2.Add(k);
                    kamene = new List<int> { prvy, treti };
                    break;
                }

                List<List<int>> result = (twoStones) ? CombinationSum(new int[] { prvy, treti }, target, true) : CombinationSum(new int[] { prvy, druhy, treti }, target, true);
                
                if (result.Count() >= 3 && result.Count() <= 6)
                {
                    int[] k = (twoStones) ? new int[] { prvy, treti } : new int[] { prvy, druhy, treti };
                    Array.Sort(k);
                    if (ContainsItemFourth(k)) continue;
                    GameManager.instance.playerStats.infine = true;
                    ok = true;
                    zasobnik2.Add(k);
                    solved = result;
                    kamene = (twoStones) ? new List<int> { prvy, treti } : new List<int> { prvy, druhy, treti };
                }
            }

        // SOLVED INSTANTIOATE
        if (GameManager.instance.lvl3man == null) GameManager.instance.lvl3man = GameObject.Find("LVL3Manager").GetComponent<LVL3Manager>();
        GameManager.instance.lvl3man.SetUpAnswers(solved, new List<List<int>>());
        //

        //TODO 

        GameManager.instance.boardScript.SetUpThermoLVL3(vysledna);
        GameManager.instance.boardScript.SetUpFinal(vysledna);
        GameManager.instance.boardScript.SetUpThermo(pociatocna);
        int pseudoSolutions = Random.Range(4, 7);
        GameManager.instance.boardScript.SetUpSolutionsNumber(0, (GameManager.instance.playerStats.noSolutions) ? pseudoSolutions : solved.Count());
        GameManager.instance.boardScript.InstantiateStonesLVL3(kamene, false);

        //

        // SAVE EQ

        GameManager.instance.playerStats.kamene4 = new List<int>();
        GameManager.instance.playerStats.kamene4.AddRange(kamene);
        GameManager.instance.playerStats.kameneNaPloche4 = kamene;
        GameManager.instance.playerStats.solved4 = solved;
        GameManager.instance.playerStats.finalna4 = vysledna;
        GameManager.instance.playerStats.pociatocna4 = pociatocna;
        GameManager.instance.playerStats.savedEq4 = true;
        GameManager.instance.playerStats.answers4 = new List<List<int>>();
        GameManager.instance.playerStats.solutionsGot4 = 0;
        GameManager.instance.playerStats.solutionsAll4 = (GameManager.instance.playerStats.noSolutions) ? pseudoSolutions : solved.Count();
        GameManager.instance.playerStats.level = GameManager.instance.level;

    }

    List<int> MakeStones(List<List<int>> s)
    {
        List<int> k = new List<int>();
        foreach (List<int> v in s)
        {
            foreach (int w in v) k.Add(w);
        }

        for (int i = 0; i < k.Count; i++)
        {
            int tmp = k[i];
            int ri = Random.Range(i, k.Count);
            k[i] = k[ri];
            k[ri] = tmp;
        }

        return k;
    }

    public int Gcd(int a, int b)
    {
        if (b == 0)
        {
            return a;
        }
        return Gcd(b, a % b);
    }

    bool ContainsItem(int[] z)
    {
        foreach (int[] i in zasobnik)
        {
            if (string.Join(",", i) == string.Join(",", z)) return true;
        }
        return false;
    }

    bool ContainsItemFourth(int[] z)
    {
        foreach (int[] i in zasobnik2)
        {
            if (string.Join(",", i) == string.Join(",", z)) return true;
        }
        return false;
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
