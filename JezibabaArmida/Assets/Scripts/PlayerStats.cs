using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    bool[] levels = {true,false,false};

    public int level_1 = 0;
    public int level_2 = 0;
    public int level_3 = 0;
    public int level_4 = 0;

    public int level = 0;

    public bool savedEq = false;
    public bool savedEq2 = false;
    public bool savedEq3 = false;
    public bool savedEq4 = false;

    /// SAVE EQ

    public List<int> kamene = new List<int>();          
    public string rovnica;
    public int pociatocna;
    public int finalna;


    public List<int> kamene2 = new List<int>();
    public List<string> znamienka2 = new List<string>();
    public Dictionary<int, string> zatvorky = new Dictionary<int, string>();
    public string rovnica2;
    public int pociatocna2;
    public int finalna2;


    public List<int> kamene3 = new List<int>();
    public List<int> kameneNaPloche = new List<int>();
    public List<List<int>> solved = new List<List<int>>();
    public List<List<int>> answers = new List<List<int>>();
    public bool zaporne;
    public int solutionsAll;
    public int solutionsGot;
    public int pociatocna3;
    public int finalna3;


    public List<int> kamene4 = new List<int>();
    public List<int> kameneNaPloche4 = new List<int>();
    public List<List<int>> solved4 = new List<List<int>>();
    public List<List<int>> answers4 = new List<List<int>>();
    public int solutionsAll4;
    public int solutionsGot4;
    public int pociatocna4;
    public int finalna4;
    public bool infine;
    public bool noSolutions;

    void Start()
    {
        /*
        Button l1 = GameObject.Find("first_lvl").GetComponent<Button>();
        Button l2 = GameObject.Find("second_lvl").GetComponent<Button>();
        Button l3 = GameObject.Find("third_lvl").GetComponent<Button>();
        Button l4 = GameObject.Find("fourth_lvl").GetComponent<Button>();
        */
    }

    private IEnumerator OnLevelWasLoaded(int level)
    {
        yield return new WaitForEndOfFrame();
        if(GameManager.instance.level == 1)
        {
            GameManager.instance.SetProgressionSlider(level_1);
        }
        else if(GameManager.instance.level == 2)
        {
            GameManager.instance.SetProgressionSlider(level_2);
        }
        else if(GameManager.instance.level == 3) GameManager.instance.SetProgressionSlider(level_3);
        else GameManager.instance.SetProgressionSlider(level_4);
    }

    public string GetEquasion(int l)
    {
        if(l == 1) return pociatocna + rovnica + " = " + finalna;
        else return pociatocna2 + rovnica2 + " = " + finalna2;
    }
}
