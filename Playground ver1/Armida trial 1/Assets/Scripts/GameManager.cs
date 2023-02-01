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
    public int level = 4;
    private TextMeshProUGUI t;
    private TextMeshProUGUI input;
    bool p = false;
    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        //boardScript = gameObject.GetComponent<BoardManager>();
        DontDestroyOnLoad(gameObject);
        //SceneManager.LoadScene(0);
    }

    public void Init()
    {
        t = GameObject.Find("txt").GetComponent<TextMeshProUGUI>();
        boardScript = gameObject.GetComponent<BoardManager>();
        //input = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        t.text = boardScript.SetUpBoard(5);
    }

    public void NextTask()
    {
        int right = boardScript.GetAnswer();
        int left = GetInput();
        Debug.Log(right + " == " + left);
        if(right == left)
        {
            t.text = boardScript.SetUpBoard(5);
            UpdateProgressionSlider();
        }
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!p)                 //TODO dat do start
        {
            //Init();
            p = true;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1) Init();
    }

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }

}
