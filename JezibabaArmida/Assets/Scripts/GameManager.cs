using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardScript;
    [HideInInspector] public PlayerStats playerStats;
    public int level = 0;
    private TextMeshProUGUI t;
    private TextMeshProUGUI input;
    bool p = false;

    bool b = true;

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
        playerStats = GetComponent<PlayerStats>();
    }

    public void Init()
    {
        t = GameObject.Find("txt").GetComponent<TextMeshProUGUI>();
        boardScript = gameObject.GetComponent<BoardManager>();
        //input = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        //boardScript.Init();
        t.text = boardScript.SetUpBoard(5);
    }

    public void NextTask()
    {
        int right = boardScript.GetAnswer();        //TODO upravit aby checklo oba levels
        int left = GetInput();
        Debug.Log(right + " == " + left);
        if(right == left)
        {
            StartCoroutine(ShowBubble(1));
            if(level == 1) playerStats.savedEq = false;
            else playerStats.savedEq2 = false;
            t.text = boardScript.SetUpBoard(5);
            UpdateProgressionSlider();
        }
        else
        {
            StartCoroutine(ShowBubble(0));
        }
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
        Slider input = GameObject.Find("Numbers").GetComponent<Slider>();
        return (int)input.value;
    }

    public void UpdateProgressionSlider()
    {
        Slider prog = GameObject.Find("Progression").GetComponent<Slider>();
        prog.value = (prog.value+1) % 6;
        if(level == 1) playerStats.level_1 += 1;
        else playerStats.level_2 += 1;
    }

    public void SetProgressionSlider(int value)
    {
        Debug.Log("settujem");
        Slider prog = GameObject.Find("Progression").GetComponent<Slider>();
        prog.value = value;
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

        Debug.Log("GAME LEVEL:::::: " + level);
    }

    public void LoadLevel(int i)
    {
        //level = i;
        SceneManager.LoadScene(i);
    }

}
