using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    bool[] levels = {true,false,false,false};

    public int level_1 = 0;
    public int level_2 = 0;
    public int level_3 = 0;
    public int level_4 = 0;

    public int level = 0;

    public bool savedEq = false;
    public bool savedEq2 = false;
    public bool savedEq3 = false;
    public bool savedEq4 = false;
    public bool savedEditor1 = false;
    public bool savedEditor2 = false;
    public bool savedEditor3 = false;
    public bool savedEditor4 = false;

    public bool editor1 = false;
    public bool editor2 = false;
    public bool editor3 = false;
    public bool editor4 = false;

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

    private IEnumerator OnLevelWasLoaded(int level)
    {
        yield return new WaitForEndOfFrame();
        if (level == 0)
        {
            if (savedEditor1)
            {
                savedEditor1 = false;
                savedEq = false;
            }
            if (savedEditor2)
            {
                savedEditor2 = false;
                savedEq2 = false;
            }
            if (savedEditor3)
            {
                savedEditor3 = false;
                savedEq3 = false;
            }
            if (savedEditor4)
            {
                savedEditor4 = false;
                savedEq4 = false;
            }
        }
        else if(GameManager.instance.level == 1)
        {
            GameManager.instance.SetProgressionSlider(level_1);
            if(editor1) GameManager.instance.UnlockEditor(1);
        }
        else if(GameManager.instance.level == 2)
        {
            GameManager.instance.SetProgressionSlider(level_2);
            if (editor2) GameManager.instance.UnlockEditor(2);
        }
        else if(GameManager.instance.level == 3)
        {
            GameManager.instance.SetProgressionSlider(level_3);
            if (editor3) GameManager.instance.UnlockEditor(3);
        }
        else
        {
            GameManager.instance.SetProgressionSlider(level_4);
            if (editor4) GameManager.instance.UnlockEditor(4);
        }
    }

    public string GetEquasion(int l)
    {
        if(l == 1) return pociatocna + rovnica + " = " + finalna;
        else return pociatocna2 + rovnica2 + " = " + finalna2;
    }

    private void Update()
    {
        
        if (level_1 == 5 || level_2 == 5 || level_3 == 5) UnlockLevel();
        if (level_1 == 1 && !editor1)
        {
            GameManager.instance.UnlockEditor(1);
            editor1 = true;
        }
        if(level_2 == 1 && !editor2)
        {
            GameManager.instance.UnlockEditor(2);
            editor2 = true;
        }
        if(level_3 == 1 && !editor3)
        {
            GameManager.instance.UnlockEditor(3);
            editor3 = true;
        }
        if (level_4 == 1 && !editor4)
        {
            GameManager.instance.UnlockEditor(4);
            editor4 = true;
        }
    }

    void UnlockLevel()
    {
        if(level_1 == 5 && levels[1] == false)
        {
            GameManager.instance.UnlockLevel(2);
            levels[1] = true;
        }

        else if(level_2 == 5 && levels[2] == false)
        {
            GameManager.instance.UnlockLevel(3);
            levels[2] = true;
        }

        else if (level_3 == 5 && levels[3] == false)
        {
            GameManager.instance.UnlockLevel(4);
            levels[3] = true;
        }
    }
}
