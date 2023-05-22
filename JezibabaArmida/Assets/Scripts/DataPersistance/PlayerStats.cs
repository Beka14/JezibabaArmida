using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public bool[] levels = {true,false,false,false};

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
            if (levels[1] && GameObject.Find("second_lvl").GetComponent<Button>().interactable == false)
            {
                GameObject.Find("second_lvl").GetComponent<Button>().interactable = true;
                GameObject.Find("txt2").GetComponent<TextMeshProUGUI>().color = new Color(255,255,255,1);
            }
            if (levels[2] && GameObject.Find("third_lvl").GetComponent<Button>().interactable == false)
            {
                GameObject.Find("third_lvl").GetComponent<Button>().interactable = true;
                GameObject.Find("txt3").GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 1);
            }
            if (levels[3] && GameObject.Find("fourth_lvl").GetComponent<Button>().interactable == false)
            {
                GameObject.Find("fourth_lvl").GetComponent<Button>().interactable = true;
                GameObject.Find("txt4").GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 1);
            }

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
    
    private void Start()
    {
        if (levels[1])
        {
            GameManager.instance.UnlockLevel(2, false);
        }
        if (levels[2])
        {
            GameManager.instance.UnlockLevel(3, false);
        }
        if (levels[3])
        {
            GameManager.instance.UnlockLevel(4, false);
        }
        SceneManager.LoadScene(0);
    }
    

    private void Update()
    {
        
        if (level_1 == 5 || level_2 == 5 || level_3 == 5) UnlockLevel();
        if (level_1 == 10 && !editor1)
        {
            GameManager.instance.UnlockEditor(1);
            editor1 = true;
        }
        if(level_2 == 10 && !editor2)
        {
            GameManager.instance.UnlockEditor(2);
            editor2 = true;
        }
        if(level_3 == 10 && !editor3)
        {
            GameManager.instance.UnlockEditor(3);
            editor3 = true;
        }
        if (level_4 == 10 && !editor4)
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

    public void SaveData(ref SavedData data)
    {
        data.levels = levels;
        data.level_1 = level_1;
        data.level_2 = level_2;
        data.level_3 = level_3;
        data.level_4 = level_4;
        data.level = level;

        data.savedEq = savedEq;
        data.savedEq2 = savedEq2;
        data.savedEq3 = savedEq3;
        data.savedEq4 = savedEq4;
        data.savedEditor1 = savedEditor1;
        data.savedEditor2 = savedEditor2;
        data.savedEditor3 = savedEditor3;
        data.savedEditor4 = savedEditor4;

        data.editor1 = editor1;
        data.editor2 = editor2;
        data.editor3 = editor3;
        data.editor4 = editor4;

        data.kamene = kamene;
        data.rovnica = rovnica;
        data.pociatocna = pociatocna;
        data.finalna = finalna;

        data.kamene2 = kamene2;
        data.znamienka2 = znamienka2;
        data.zatvorky = zatvorky;
        data.rovnica2 = rovnica2;
        data.pociatocna2 = pociatocna2;
        data.finalna2 = finalna2;

        data.kamene3 = kamene3;
        data.kameneNaPloche = kameneNaPloche;
        data.solved = solved;
        data.answers = answers;
        data.zaporne = zaporne;
        data.solutionsAll = solutionsAll;
        data.solutionsGot = solutionsGot;
        data.pociatocna3 = pociatocna3;
        data.finalna3 = finalna3;

        data.kamene4 = kamene4;
        data.kameneNaPloche4 = kameneNaPloche4;
        data.solved4 = solved4;
        data.answers4 = answers4;
        data.solutionsAll4 = solutionsAll4;
        data.solutionsGot4 = solutionsGot4;
        data.pociatocna4 = pociatocna4;
        data.finalna4 = finalna4;
        data.infine = infine;
        data.noSolutions = noSolutions;
    }

    public void LoadData(SavedData data)
    {
        levels = data.levels;
        level_1 = data.level_1;
        level_2 = data.level_2;
        level_3 = data.level_3;
        level_4 = data.level_4;
        level = data.level;

        savedEq = data.savedEq;
        savedEq2 = data.savedEq2;
        savedEq3 = data.savedEq3;
        savedEq4 = data.savedEq4;
        savedEditor1 = data.savedEditor1;
        savedEditor2 = data.savedEditor2;
        savedEditor3 = data.savedEditor3;
        savedEditor4 = data.savedEditor4;

        editor1 = data.editor1;
        editor2 = data.editor2;
        editor3 = data.editor3;
        editor4 = data.editor4;

        kamene = data.kamene;
        rovnica = data.rovnica;
        pociatocna = data.pociatocna;
        finalna = data.finalna;

        kamene2 = data.kamene2;
        znamienka2 = data.znamienka2;
        zatvorky = data.zatvorky;
        rovnica2 = data.rovnica2;
        pociatocna2 = data.pociatocna2;
        finalna2 = data.finalna2;

        kamene3 = data.kamene3;
        kameneNaPloche = data.kameneNaPloche;
        solved = data.solved;
        answers = data.answers;
        zaporne = data.zaporne;
        solutionsAll = data.solutionsAll;
        solutionsGot = data.solutionsGot;
        pociatocna3 = data.pociatocna3;
        finalna3 = data.finalna3;

        kamene4 = data.kamene4;
        kameneNaPloche4 = data.kameneNaPloche4;
        solved4 = data.solved4;
        answers4 = data.answers4;
        solutionsAll4 = data.solutionsAll4;
        solutionsGot4 = data.solutionsGot4;
        pociatocna4 = data.pociatocna4;
        finalna4 = data.finalna4;
        infine = data.infine;
        noSolutions = data.noSolutions;
    }
}
