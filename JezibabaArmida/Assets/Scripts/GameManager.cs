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
    public BoardManager boardScript;
    [HideInInspector] public PlayerStats playerStats;
    AudioManager audioManager;
    public int level = 0;
    private TextMeshProUGUI t;
    private int gotSolutions;
    private int allSolutions;

    [SerializeField] GameObject icon2;
    [SerializeField] GameObject icon3;
    [SerializeField] GameObject icon4;

    [SerializeField] GameObject reset_icon2;
    [SerializeField] GameObject reset_icon3;
    [SerializeField] GameObject reset_icon4;

    public LVL3Manager lvl3man;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        gameObject.name = "GameManager";
    }

    private void Start()
    {
        playerStats = gameObject.GetComponent<PlayerStats>();
        boardScript = gameObject.GetComponent<BoardManager>();
        audioManager = gameObject.GetComponent<AudioManager>();
    }

    public void SetUpPS()
    {
        playerStats = gameObject.GetComponent<PlayerStats>();
    }

    public void Init()
    {
        boardScript = gameObject.GetComponent<BoardManager>();
        boardScript.SetUpBoard();
    }

    public void InitLVL3()
    {
        boardScript = gameObject.GetComponent<BoardManager>();
        lvl3man = GameObject.Find("LVL3Manager").GetComponent<LVL3Manager>();
        boardScript.SetUpBoard();
    }

    public void InfineSolutions()
    {
        StartCoroutine(ShowBubbleLVL3(5));
        ConfettiAnimation();
        playerStats.savedEq4 = false;
        playerStats.infine = false;
        DeleteStonesFromKotol("Kotol_lvl3");
        if (!playerStats.savedEditor3 && !playerStats.savedEditor4) UpdateProgressionSlider();
        boardScript.SetUpBoard();
    }

    public void NoSolutions()
    {
        StartCoroutine(ShowBubbleLVL3(5));
        ConfettiAnimation();
        playerStats.savedEq4 = false;
        playerStats.noSolutions = false;
        DeleteStonesFromKotol("Kotol_lvl3");
        if (!playerStats.savedEditor3 && !playerStats.savedEditor4) UpdateProgressionSlider();
        boardScript.SetUpBoard();
    }

    public void NextTask()
    {
        int right = boardScript.GetAnswer();     
        int left = GetInput();
        if(level == 2 || level == 1)
        {
            if (right == left)
            {
                if(!playerStats.savedEditor1 && !playerStats.savedEditor2) UpdateProgressionSlider();
                ConfettiAnimation();
                StartCoroutine(ShowBubble(1));
                if (level == 1) playerStats.savedEq = false;
                else if (level == 2) playerStats.savedEq2 = false;
                else if (playerStats.savedEditor1) playerStats.savedEditor1 = false;
                else if (playerStats.savedEditor2) playerStats.savedEditor2 = false;
                boardScript.SetUpBoard();
            }
            else
            {
                StartCoroutine(ShowBubble(0));
            }
        }

        else
        {
            if(gotSolutions == allSolutions && playerStats.infine || playerStats.noSolutions)
            {
                StartCoroutine(ShowBubbleLVL3(6));
                return;
            }
            gotSolutions = (level == 3) ? playerStats.solutionsGot : playerStats.solutionsGot4;
            allSolutions = (level == 3) ? playerStats.solutionsAll : playerStats.solutionsAll4;
            if (gotSolutions == allSolutions)
            {
                if (!playerStats.savedEditor3 && !playerStats.savedEditor4) UpdateProgressionSlider();
                ConfettiAnimation();
                if (level==3) playerStats.savedEq3 = false; else playerStats.savedEq4 = false;
                
                DeleteStonesFromKotol("Kotol_lvl3");
                
                boardScript.SetUpBoard();
            }
            else
            {
                StartCoroutine(ShowBubbleLVL3(3));
            }
        }
    }

    public void CheckTask()
    {
        if(lvl3man == null) lvl3man = GameObject.Find("LVL3Manager").GetComponent<LVL3Manager>();
        gotSolutions = (level == 3)? playerStats.solutionsGot : playerStats.solutionsGot4;
        allSolutions = (level == 3) ? playerStats.solutionsAll : playerStats.solutionsAll4;
        int right = (int)GameObject.Find("Slider").GetComponent<Slider>().value;
        int left = GetInput();

        if (gotSolutions == allSolutions)
        {
            StartCoroutine(ShowBubbleLVL3(2));
        }

        if (left == right)
        {
            List<int> gameObjects = new List<int>();        
            
            GameObject g = GameObject.Find("Kotol_lvl3");
            for (var i = g.transform.childCount - 1; i >= 0; i--)
            {
                gameObjects.Add(GetValueFromStone(g.transform.GetChild(i).gameObject));
                Object.Destroy(g.transform.GetChild(i).gameObject);
            }

            if (lvl3man.ContainsAnswer(gameObjects))
            {
                UpdateSolutionsText();
                StartCoroutine(ShowBubbleLVL3(0));

                if (playerStats.zaporne)
                {
                    for (var i = gameObjects.Count - 1; i >= 0; i--)
                    {
                        playerStats.kameneNaPloche.Remove(gameObjects[i]);
                    }
                }

                gotSolutions = (level == 3) ? playerStats.solutionsGot : playerStats.solutionsGot4;
                allSolutions = (level == 3) ? playerStats.solutionsAll : playerStats.solutionsAll4;
            }

            else
            {
                //vypis ze to uz mame
                if (playerStats.zaporne == true)
                {
                    if (ContainsSolution(gameObjects))
                    {
                        StartCoroutine(ShowBubbleLVL3(1));
                    }
                    boardScript.InstantiateStonesLVL3(gameObjects, true);
                }
                
                else StartCoroutine(ShowBubbleLVL3(8));

            }

            if (level == 3) boardScript.SetUpThermo(playerStats.pociatocna3); else boardScript.SetUpThermo(playerStats.pociatocna4);

        }

        else
        {
            StartCoroutine(ShowBubbleLVL3(4));
        }

        if (gotSolutions == allSolutions)
        {
            StartCoroutine(ShowBubbleLVL3(2));
        }


    }

    private bool ContainsSolution(List<int> gameObjects)
    {
        int[] odpoved = gameObjects.ToArray();
        Array.Sort(odpoved);
        List<List<int>> vsetkySolved = (level == 3) ? playerStats.solved : playerStats.solved4;
        foreach (List<int> g in vsetkySolved)
        {
            int[] gg = g.ToArray();
            Array.Sort(gg);
            if (string.Join(",", gg) == string.Join(",", odpoved)) return true;
        }
        return false;
    }

    public int GetValueFromStone(GameObject kamen)
    {
        TextMeshProUGUI tmp = kamen.transform.Find("value").GetComponent<TextMeshProUGUI>();
        int i = 1;
        if (kamen.name == "drag_studeny" || kamen.name == "drag_studeny2" || kamen.name == "edit_studeny" || kamen.name == "studeny") i = -1;
        
        return (Convert.ToInt32(tmp.text) * i);
    }

    public void DeleteStonesFromKotol(string kotol)
    {
        GameObject g = GameObject.Find(kotol);
        for (var i = g.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(g.transform.GetChild(i).gameObject);
        }
    }

    void UpdateSolutionsText()
    {
        TextMeshProUGUI t = GameObject.Find("solutions").GetComponent<TextMeshProUGUI>();
        if(level == 3)
        {
            playerStats.solutionsGot += 1;
            t.text = playerStats.solutionsGot + "/" + playerStats.solutionsAll;
        }
        else
        {
            playerStats.solutionsGot4 += 1;
            t.text = playerStats.solutionsGot4 + "/" + playerStats.solutionsAll4;
        }
    }

    IEnumerator ShowBubble(int i)
    {
        InactivateButtons(true);
        Color pred;
        Image farba;
        Image image;
        Image sprava;
        if (i == 0)
        {
            audioManager.PlaySound(13);
            farba = GameObject.Find("number_fill").GetComponent<Image>();
            image = GameObject.Find("Image").GetComponent<Image>();
            pred = farba.color;
            farba.color = new Color(255,0,0,1);
            image.color = new Color(255, 0, 0, 1);

            sprava = GameObject.Find("zla_sprava").GetComponent<Image>();
            sprava.color = new Color(sprava.color.r, sprava.color.g, sprava.color.b, 1);
        }
        else
        {
            int[] pole = { 1, 3, 4, 14 };
            int x = UnityEngine.Random.Range(1,4);
            audioManager.PlaySound(pole[x]);
            farba = GameObject.Find("number_fill").GetComponent<Image>();
            image = GameObject.Find("Image").GetComponent<Image>();
            pred = farba.color;
            farba.color = new Color(0, 255, 0, 1);
            image.color = new Color(0, 255, 0, 1);

            sprava = GameObject.Find("dobra_sprava").GetComponent<Image>();
            sprava.color = new Color(sprava.color.r, sprava.color.g, sprava.color.b, 1);
        }
        yield return new WaitForSeconds(2);
        farba.color = pred;
        image.color = pred;
        sprava.color = new Color(sprava.color.r, sprava.color.g, sprava.color.b, 0);
        InactivateButtons(false);
    }

    IEnumerator ShowBubbleLVL3(int i)
    {
        InactivateButtons(true);
        Image sprava;
        switch (i)
        {
            case 0:
                int[] pole = { 1, 3, 4, 14 };
                int x = UnityEngine.Random.Range(1, 4);
                audioManager.PlaySound(pole[x]);
                sprava = GameObject.Find("dalej_sprava").GetComponent<Image>();
                break;
            case 1:
                audioManager.PlaySound(15);
                sprava = GameObject.Find("uz_mas_sprava").GetComponent<Image>();
                break;
            case 2:
                audioManager.PlaySound(5);
                sprava = GameObject.Find("dalsia_uloha_sprava").GetComponent<Image>();
                break;
            case 3:
                audioManager.PlaySound(6);
                sprava = GameObject.Find("vsetky_sprava").GetComponent<Image>();
                break;
            case 4:
                audioManager.PlaySound(12);
                sprava = GameObject.Find("napln_sprava").GetComponent<Image>();
                break;
            case 5:
                audioManager.PlaySound(1);
                sprava = GameObject.Find("dobra_sprava").GetComponent<Image>();
                break;
            default:
                audioManager.PlaySound(8);
                sprava = GameObject.Find("zla_sprava").GetComponent<Image>();
                break;
        }
        sprava.color = new Color(sprava.color.r, sprava.color.g, sprava.color.b, 1);
        yield return new WaitForSeconds(2);
        sprava.color = new Color(sprava.color.r, sprava.color.g, sprava.color.b, 0);
        InactivateButtons(false);
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
        if (prog.value == 10) return;
        if (prog.value == 9)
        {
            prog.value = 10;
            if (level == 1) playerStats.level_1 = 10;
            else if (level == 2) playerStats.level_2 = 10;
            else if (level == 3) playerStats.level_3 = 10;
            else playerStats.level_4 = 10;
            return;
        }
        prog.value = (prog.value+1) % 10;
        if(level == 1) playerStats.level_1 = (playerStats.level_1+1)% 10;
        else if(level == 2) playerStats.level_2 = (playerStats.level_2+1)% 10;
        else if(level == 3) playerStats.level_3 = (playerStats.level_3 + 1) % 10;
        else playerStats.level_4 = (playerStats.level_4 + 1) % 10;
    }

    public void SetProgressionSlider(int value)
    {
        if (GameObject.Find("Progression") == null) return;
        Slider prog = GameObject.Find("Progression").GetComponent<Slider>();
        prog.value = value;
    }

    public void AddStoneSlider(GameObject x)
    {
        int e = (x.name == "drag_studeny" || x.name == "drag_studeny2") ? -1:1;
        TextMeshProUGUI t = x.transform.Find("value").GetComponent<TextMeshProUGUI>();
        Slider prog = GameObject.Find("Slider").GetComponent<Slider>();
        prog.value += (t.text == "")? e*1:e*Convert.ToInt16(t.text);
    }

    public void RemoveStoneSlider(GameObject x)
    {
        int e = (x.name == "drag_studeny" || x.name == "drag_studeny2") ? -1:1;
        TextMeshProUGUI t = x.transform.Find("value").GetComponent<TextMeshProUGUI>();
        Slider prog = GameObject.Find("Slider").GetComponent<Slider>();
        prog.value += (t.text == "") ? -1*e : -1*e*Convert.ToInt16(t.text);
    }

    public int GetSliderValue()
    {
        Slider prog = GameObject.Find("Slider").GetComponent<Slider>();
        return Convert.ToInt32(prog.value);
    }

    public int GetFinalValue()
    {
        return boardScript.GetAnswer();
    }

    IEnumerator OnLevelWasLoaded(int lvl)
    {
        yield return new WaitForEndOfFrame();
        if (lvl == 1 || lvl == 2)
        {
            level = lvl;
            Init();
        }
        else if(lvl == 3 || lvl == 4)
        {
            level = lvl;
            InitLVL3();
        }
        level = lvl;
    }

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }

    void InactivateButtons(bool off)
    {
        GameObject g = GameObject.Find("Buttons");
        for (var i = g.transform.childCount - 1; i >= 0; i--)
        {
            Button b = g.transform.GetChild(i).gameObject.GetComponent<Button>();
            b.interactable = (off)? false: true;
        }
    }

    void ConfettiAnimation()
    {
        ConfettiAnimator a = GameObject.Find("Confetti").GetComponent<ConfettiAnimator>();
        StartCoroutine(a.Animate());
    }

    public void UnlockLevel(int i, bool sound=true)
    {
        switch (i)
        {
            case 2:
                if(sound) audioManager.PlaySound(9);
                icon2.GetComponent<Button>().interactable = true;
                reset_icon2.GetComponent<Button>().interactable = true;
                icon2.transform.Find("txt2").GetComponent<TextMeshProUGUI>().color = new Color(217, 217, 217, 255);
                break;
            case 3:
                if(sound) audioManager.PlaySound(10);
                icon3.GetComponent<Button>().interactable = true;
                reset_icon3.GetComponent<Button>().interactable = true;
                icon3.transform.Find("txt3").GetComponent<TextMeshProUGUI>().color = new Color(217, 217, 217, 255);
                break;
            case 4:
                if (sound) audioManager.PlaySound(20);
                icon4.GetComponent<Button>().interactable = true;
                reset_icon4.GetComponent<Button>().interactable = true;
                icon4.transform.Find("txt4").GetComponent<TextMeshProUGUI>().color = new Color(217, 217, 217, 255);
                break;
        }
        if(sound) StartCoroutine(StartAnimation(i));
    }

    public void UnlockEditor(int x)
    {
        if(x == 1)
        {
            GameObject.Find("editor_btn").GetComponent<Button>().interactable = true;
            GameObject.Find("editor_btn").transform.SetParent(GameObject.Find("Buttons").transform);
        }
        else if(x == 2)
        {
            GameObject.Find("editor2_btn").GetComponent<Button>().interactable = true;
            GameObject.Find("editor2_btn").transform.SetParent(GameObject.Find("Buttons").transform);
        }
        else if(x == 3) 
        {
            GameObject.Find("editor3_btn").GetComponent<Button>().interactable = true;
            GameObject.Find("editor3_btn").transform.SetParent(GameObject.Find("Buttons").transform);
        }
        else if (x == 4)
        {
            GameObject.Find("editor4_btn").GetComponent<Button>().interactable = true;
            GameObject.Find("editor4_btn").transform.SetParent(GameObject.Find("Buttons").transform);
        }
    }

    public void ResetLevel(int i)
    {
        switch (i)
        {
            case 1:
                playerStats.level_1 = 0;
                playerStats.savedEq = false;
                break;
            case 2:
                playerStats.level_2 = 0;
                playerStats.savedEq2 = false;
                break;
            case 3:
                playerStats.level_3 = 0;
                playerStats.savedEq3 = false;
                break;
            case 4:
                playerStats.level_4 = 0;
                playerStats.savedEq4 = false;
                break;
            default:
                break;
        }
    }

    IEnumerator StartAnimation(int i)
    {
        string level = "";
        if (i == 2) level = "second_unlock"; else if (i == 3) level = "third_unlock"; else level = "fourth_unlock";
        GameObject.Find(level).GetComponent<Image>().color = new Color(255, 255, 255, 255);
        GameObject.Find("txt" + i).GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
        GameObject.Find(level).GetComponent<Animator>().SetBool("pusti", true);
        yield return new WaitForSeconds(1.4f);
        GameObject.Find(level).GetComponent<Image>().color = new Color(255, 255, 255, 0);
        GameObject.Find("txt" + i).GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 0);
        GameObject.Find(level).GetComponent<Animator>().SetBool("pusti", false);
    }
}
