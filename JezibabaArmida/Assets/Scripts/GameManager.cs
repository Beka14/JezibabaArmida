using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardScript;
    [HideInInspector] public PlayerStats playerStats;
    public int level = 0;
    private TextMeshProUGUI t;
    private TextMeshProUGUI input;

    public LVL3Manager lvl3man;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        //dolezite = 0;

        //if(gameObject.TryGetComponent<BoardManager>(out BoardManager boardScript)) boardScript = gameObject.GetComponent<BoardManager>();
        DontDestroyOnLoad(gameObject);
        gameObject.name = "GameManager";
        //SceneManager.LoadScene(0);
    }

    private void Start()
    {
        Debug.Log("--------------- STARTING ---------------");
        playerStats = GetComponent<PlayerStats>();
        boardScript = gameObject.GetComponent<BoardManager>();
    }

    public void Init()
    {
        t = GameObject.Find("txt").GetComponent<TextMeshProUGUI>();
        boardScript = gameObject.GetComponent<BoardManager>();
        //input = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        //boardScript.Init();
        t.text = boardScript.SetUpBoard(5);
    }

    public void InitLVL3()
    {
        boardScript = gameObject.GetComponent<BoardManager>();
        //input = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        //boardScript.Init();
        boardScript.SetUpBoard(0);
    }

    public void NextTask()
    {
        int right = boardScript.GetAnswer();     
        int left = GetInput();
        Debug.Log(right + " == " + left);
        if(level == 2 || level == 1)
        {
            if (right == left)
            {
                StartCoroutine(ShowBubble(1));
                if (level == 1) playerStats.savedEq = false;
                else if (level == 2) playerStats.savedEq2 = false;
                t.text = boardScript.SetUpBoard(5);
                UpdateProgressionSlider();
            }
            else
            {
                StartCoroutine(ShowBubble(0));
            }
        }

        else
        {
            if (playerStats.solutionsGot == playerStats.solutionsAll)
            {
                boardScript.SetUpBoard(0);
                UpdateProgressionSlider();
            }
            else
            {
                //VYPIS BUBLINU NECH DOKONCI ZADANIE
            }
        }
    }

    public void CheckTask()
    {
        if(lvl3man == null) lvl3man = GameObject.Find("LVL3Manager").GetComponent<LVL3Manager>();
        //boardScript = gameObject.GetComponent<BoardManager>();
        int right = (int)GameObject.Find("Slider").GetComponent<Slider>().value;
        int left = GetInput();
        int a = playerStats.solutionsGot;
        int b = playerStats.solutionsAll;
        Debug.Log(left + " == " + right);
        Debug.Log(a + " / " + b);
        if (left == right)
        {
           if(a == b)
            {
                Debug.Log("jeej rovnaju sa super mame vsetky riesenia");
                /*
                UpdateProgressionSlider();

                GameObject g = GameObject.Find("Kotol_lvl3");                       //TODO vytiahnut do metody
                for (var i = g.transform.childCount - 1; i >= 0; i--)
                {
                    Object.Destroy(g.transform.GetChild(i).gameObject);
                }

                boardScript.SetUpBoard(0);
                */
            }

            else
            {
                Debug.Log("supris este nemas vsetky zadania tho");
                UpdateSolutionsText();

                List<GameObject> gameObjects = new List<GameObject>();          //PRIDAT INT NIE OBJECT

                GameObject g = GameObject.Find("Kotol_lvl3");
                for (var i = g.transform.childCount - 1; i >= 0; i--)
                {
                    gameObjects.Add(g.transform.GetChild(i).gameObject);
                    Object.Destroy(g.transform.GetChild(i).gameObject);
                }

                if (lvl3man.ContainsAnswer(gameObjects))
                {
                    //vypis ze to uz mame
                }
                else
                {
                    //lvl3man.AddAnswer();
                    //vypis ze super
                }
            }
        }
    }

    void UpdateSolutionsText()
    {
        TextMeshProUGUI t = GameObject.Find("solutions").GetComponent<TextMeshProUGUI>();
        playerStats.solutionsGot += 1;
        t.text = playerStats.solutionsGot + "/" + playerStats.solutionsAll;
    }

    IEnumerator ShowBubble(int i)
    {
        Button button = GameObject.Find("next_btn").GetComponent<Button>();
        button.interactable = false;
        Color pred;
        Image farba;
        Image sprava;
        if (i == 0)
        {
            farba = GameObject.Find("number_fill").GetComponent<Image>();
            pred = farba.color;
            farba.color = new Color(255,0,0,1);

            sprava = GameObject.Find("zla_sprava").GetComponent<Image>();
            sprava.color = new Color(sprava.color.r, sprava.color.g, sprava.color.b, 1);
            //yield return new WaitForSeconds(4);
        }
        else
        {
            farba = GameObject.Find("number_fill").GetComponent<Image>();
            pred = farba.color;
            farba.color = new Color(0, 255, 0, 1);

            sprava = GameObject.Find("dobra_sprava").GetComponent<Image>();
            sprava.color = new Color(sprava.color.r, sprava.color.g, sprava.color.b, 1);
            //yield return new WaitForSeconds(4);
        }
        yield return new WaitForSeconds(2);
        farba.color = pred;
        sprava.color = new Color(sprava.color.r, sprava.color.g, sprava.color.b, 0);
        button.interactable = true;
    }

    public int GetInput()
    {
        if(level == 1 || level == 2)
        {
            Slider input = GameObject.Find("Numbers").GetComponent<Slider>();
            return (int)input.value;
        }
        else
        {
            Slider input = GameObject.Find("Slider2").GetComponent<Slider>();
            return (int)input.value;
        }
    }

    public void UpdateProgressionSlider()
    {
        Slider prog = GameObject.Find("Progression").GetComponent<Slider>();
        prog.value = (prog.value+1) % 6;
        if(level == 1) playerStats.level_1 += 1;
        else if(level == 2) playerStats.level_2 += 1;
        else playerStats.level_3 += 1;
    }

    public void SetProgressionSlider(int value)
    {
        Debug.Log("settujem");
        Slider prog = GameObject.Find("Progression").GetComponent<Slider>();
        prog.value = value;
    }

    public void AddStoneSlider(GameObject x)
    {
        int e = (x.name == "drag_studeny")? -1:1;
        TextMeshProUGUI t = x.transform.Find("value").GetComponent<TextMeshProUGUI>();
        Slider prog = GameObject.Find("Slider").GetComponent<Slider>();
        prog.value += (t.text == "")? e*1:e*Convert.ToInt16(t.text);
        //prog.value += x;
    }

    public void RemoveStoneSlider(GameObject x)
    {
        int e = (x.name == "drag_studeny") ? -1:1;
        TextMeshProUGUI t = x.transform.Find("value").GetComponent<TextMeshProUGUI>();
        Slider prog = GameObject.Find("Slider").GetComponent<Slider>();
        prog.value += (t.text == "") ? -1*e : -1*e*Convert.ToInt16(t.text);
        //prog.value += v;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!p)                 //TODO dat do start
        {
            //Init();
            p = true;
        }
        */
    }

    IEnumerator OnLevelWasLoaded(int lvl)
    {
        yield return new WaitForEndOfFrame();
        if (lvl == 1 || lvl == 2)
        {
            level = lvl;
            Init();
        }
        else if(lvl == 3)
        {
            level = lvl;
            InitLVL3();
        }

        //Debug.Log("GAME LEVEL:::::: " + level);
    }

    public void LoadLevel(int i)
    {
        //level = i;
        SceneManager.LoadScene(i);
    }

}
