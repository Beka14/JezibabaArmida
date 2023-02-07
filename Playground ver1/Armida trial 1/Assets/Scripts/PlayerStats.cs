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

    public bool savedEq = false;

    /// SAVE EQ
        
    public List<int> kamene = new List<int>();
    public string rovnica;
    public int pociatocna;
    public int finalna;

    /// SAVE EQ

    private void Awake()
    {
        //if (Instance == null) Instance = this;
        //else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("startujem");

        Button l1 = GameObject.Find("first_lvl").GetComponent<Button>();
        Button l2 = GameObject.Find("second_lvl").GetComponent<Button>();
        Button l3 = GameObject.Find("third_lvl").GetComponent<Button>();

        //if (!levels[1]) l2.interactable = false;    
        //if (!levels[2]) l3.interactable = false;

    }

    private IEnumerator OnLevelWasLoaded(int level)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("level: " + level);
        if(level == 1)
        {
            GameManager.instance.SetProgressionSlider(level_1);
        }
    }

    public string GetEquasion()
    {
        return pociatocna + " + " + rovnica + " = " + finalna;
    }
}
