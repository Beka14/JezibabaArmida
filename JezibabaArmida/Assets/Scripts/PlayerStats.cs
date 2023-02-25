using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    //public static PlayerStats Instance = null;
    bool[] levels = {true,false,false};
    //int[] create_levels = new int[3];

    public int level_1 = 0;
    public int level_2 = 0;
    public int level_3 = 0;

    public int level = 0;

    public bool savedEq = false;
    public bool savedEq2 = false;
    public bool savedEq3 = false;

    /// SAVE EQ

    public List<int> kamene = new List<int>();          //TODO SIGLETOOOOOOOOOOOOOOOOOOOOOOOOOON
    public string rovnica;
    public int pociatocna;
    public int finalna;


    public List<int> kamene2 = new List<int>();
    public List<string> znamienka2 = new List<string>();
    public string rovnica2;
    public int pociatocna2;
    public int finalna2;


    public List<int> kamene3 = new List<int>();
    public List<List<int>> solved = new List<List<int>>();
    public List<List<int>> answers = new List<List<int>>();
    public int solutionsAll;
    public int solutionsGot;
    public int pociatocna3;
    public int finalna3;


    /// SAVE EQ

    private void Awake()
    {
        //if (Instance == null) Instance = this;
        //else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("startujem");

        Button l1 = GameObject.Find("first_lvl").GetComponent<Button>();
        Button l2 = GameObject.Find("second_lvl").GetComponent<Button>();
        Button l3 = GameObject.Find("third_lvl").GetComponent<Button>();

        //if (!levels[1]) l2.interactable = false;    
        //if (!levels[2]) l3.interactable = false;

    }

    private IEnumerator OnLevelWasLoaded(int level)
    {
        yield return new WaitForEndOfFrame();
        //Debug.Log("level: " + level);
        if(GameManager.instance.level == 1)
        {
            GameManager.instance.SetProgressionSlider(level_1);
        }
        else if(GameManager.instance.level == 2)
        {
            GameManager.instance.SetProgressionSlider(level_2);
        }
        else GameManager.instance.SetProgressionSlider(level_3);
    }

    public string GetEquasion(int l)
    {
        if(l == 1) return pociatocna + rovnica + " = " + finalna;
        else return pociatocna2 + rovnica2 + " = " + finalna2;
    }
}
