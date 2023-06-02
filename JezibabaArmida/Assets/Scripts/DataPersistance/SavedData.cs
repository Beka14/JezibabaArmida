using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SavedData 
{
    public bool[] levels = { true, false, false, false };

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

    /*
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
    */
}
