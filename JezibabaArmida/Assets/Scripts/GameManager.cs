using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardScript;
    public int level = 4;
    private TextMeshProUGUI t;
    private AudioSource audioPlayer;
    private bool audio_play;
    private void Awake()
    {
        if(instance == null) instance= this;
        else Destroy(gameObject);

        audioPlayer = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        audio_play = true;
        audioPlayer.Play();
        boardScript = gameObject.GetComponent<BoardManager>();
        DontDestroyOnLoad(gameObject);
        //SceneManager.LoadScene(0);
    }

    public void Init()
    {
        t = GameObject.Find("txt").GetComponent<TextMeshProUGUI>();
        boardScript = gameObject.GetComponent<BoardManager>();
        t.text = boardScript.SetUpBoard(5);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void OnLevelWasLoaded(int level)
    {
        //t = GameObject.Find("txt").GetComponent<TextMeshProUGUI>();
        Init();
    }

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void ManageMusic()
    {
        Debug.Log(audio_play);
        audio_play = !audio_play;
        if(audio_play) audioPlayer.Play();
        else audioPlayer.Stop();
    }
}
