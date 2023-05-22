using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    GameObject kamene;
    [SerializeField] GameObject studeny;
    [SerializeField] GameObject horuci;
    [SerializeField] GameObject minus;
    [SerializeField] GameObject plus;
    [SerializeField] GameObject minusPar;
    [SerializeField] GameObject plusPar;
    [SerializeField] GameObject endPar;

    [SerializeField] GameObject drag_studeny;
    [SerializeField] GameObject drag_horuci;
    [SerializeField] GameObject drag_studeny2;
    [SerializeField] GameObject drag_horuci2;

    GameObject thermometer;
    Thermometer thermoScript;
    FirstLevelGenerator firstLevelGenerator;
    ThirdLevelGenerator thirdLevelGenerator;
    NumberSlider numberSlider;

    List<GameObject> naPloche = new List<GameObject>();
    Dictionary<int, string> zatvorky = new Dictionary<int, string>();

    private void Start()
    {
        kamene = GameObject.Find("kamene");
        thermometer = GameObject.Find("Slider");
        if (TryGetComponent(out Thermometer thermoScript)) thermoScript = thermometer.GetComponent<Thermometer>();
        firstLevelGenerator = gameObject.GetComponent<FirstLevelGenerator>();
        thirdLevelGenerator = gameObject.GetComponent<ThirdLevelGenerator>();
    }
    public void SetUpThermo(int value)
    {
        kamene = GameObject.Find("kamene");
        thermometer = GameObject.Find("Slider");
        thermoScript = thermometer.GetComponent<Thermometer>();
        thermoScript.SetValue(value);
    }

    public void SetUpThermoLVL3(int value)
    {
        GameObject thermometer2 = GameObject.Find("Slider2");
        Thermometer thermo = thermometer2.GetComponent<Thermometer>();
        thermo.SetValue(value);
    }

    public void SetUpNumberSlider(List<int> stones)
    {
        int valueOfStones = 0;
        foreach (int s in stones) valueOfStones += (s < 0) ? s * -1 : s;
        if (numberSlider == null) numberSlider = GameObject.Find("Numbers").GetComponent<NumberSlider>();
        if(GameManager.instance.level == 1) numberSlider.SetMinMax(GameManager.instance.playerStats.finalna, valueOfStones);
        else numberSlider.SetMinMax(GameManager.instance.playerStats.finalna2, valueOfStones);
    }

    public void InstantiateStones(List<int> stones, List<string> znam)     
    {
        SetUpNumberSlider(stones);
        if (znam.Count() == 0) zatvorky = new Dictionary<int, string>(); else zatvorky = GameManager.instance.playerStats.zatvorky;
        bool poZatvorke = false;
        int index = 0;
        kamene = GameObject.Find("kamene");
        if(GameManager.instance.level == 1) kamene.GetComponent<GridLayoutGroup>().spacing = new Vector2(15,0);
        else kamene.GetComponent<GridLayoutGroup>().spacing = new Vector2(0, 0);
        GameObject g;

        for(int i = 0; i < stones.Count; i++)
        {
            if (zatvorky.ContainsKey(index))
            {
                if (zatvorky[index] == "+(")
                {
                    g = Instantiate(plusPar);
                    g.transform.localScale = new Vector3(1, 1, 1);
                    poZatvorke = true;
                }

                else if (zatvorky[index] == "-(")
                {
                    g = Instantiate(minusPar);
                    poZatvorke = true;
                }

                else g = Instantiate(endPar);

                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
                g.transform.localScale = new Vector3(1, 1, 1);
                naPloche.Add(g);
            }

            if (znam.Count() != 0 && i < znam.Count())     
            {
                if (znam[i] == "-")       
                {
                    if (poZatvorke)
                    {
                        poZatvorke = false;
                    }
                    g = Instantiate(minus);
                    g.transform.SetParent(kamene.transform);
                    g.transform.SetAsLastSibling();
                    g.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                    naPloche.Add(g);
                }
                else
                {
                    if (poZatvorke)
                    {
                        poZatvorke = false;
                    }
                    else if(i != 0)
                    {
                        g = Instantiate(plus);
                        g.transform.SetParent(kamene.transform);
                        g.transform.SetAsLastSibling();
                        g.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                        naPloche.Add(g);
                    }
                    
                }
            }

            if (stones[i] < 0)
            {
                g = Instantiate(studeny);
                TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (stones[i] == -1) ? "" : stones[i] * -1 + "";
                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
                g.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                g = Instantiate(horuci);
                TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (stones[i] == 1) ? "" : stones[i] + "";
                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
                g.transform.localScale = new Vector3(1, 1, 1);
            }

            index++;
            naPloche.Add(g);
        }
    }

    public string SetUpBoard()
    {
        if(GameManager.instance.level == 1 || GameManager.instance.level == 2)
        {
            GameManager.instance.DeleteStonesFromKotol("kamene");
        }

        else if(GameManager.instance.playerStats.zaporne && GameManager.instance.level == 3)
        {
            GameManager.instance.DeleteStonesFromKotol("kamene2");
        }

        else if(GameManager.instance.level == 3 || GameManager.instance.level == 4)
        {
            for(int i = 1; i <= 3; i++)
            {
                GameManager.instance.DeleteStonesFromKotol("kamene" + i + "i");
            }
        }

        if (GameManager.instance.playerStats.savedEq && GameManager.instance.level == 1)
        {
            SetUpThermo(GameManager.instance.playerStats.pociatocna);
            zatvorky = new Dictionary<int, string>();
            InstantiateStones(GameManager.instance.playerStats.kamene, new List<string>());
            return GameManager.instance.playerStats.GetEquasion(1);
        }

        else if (GameManager.instance.playerStats.savedEq2 && GameManager.instance.level == 2)
        {
            SetUpThermo(GameManager.instance.playerStats.pociatocna2);
            zatvorky = GameManager.instance.playerStats.zatvorky;
            InstantiateStones(GameManager.instance.playerStats.kamene2, GameManager.instance.playerStats.znamienka2);
            return GameManager.instance.playerStats.GetEquasion(2);
        }

        else if (GameManager.instance.playerStats.savedEq3 && GameManager.instance.level == 3)
        {
            if (GameManager.instance.playerStats.zaporne) GameManager.instance.lvl3man.TurnOnButton();
            SetUpThermo(GameManager.instance.playerStats.pociatocna3);
            InstantiateStonesLVL3(GameManager.instance.playerStats.kameneNaPloche, GameManager.instance.playerStats.zaporne);
            SetUpThermoLVL3(GameManager.instance.playerStats.finalna3);
            SetUpFinal(GameManager.instance.playerStats.finalna3);
            SetUpThermo(GameManager.instance.playerStats.pociatocna3);
            SetUpSolutionsNumber(GameManager.instance.playerStats.solutionsGot, GameManager.instance.playerStats.solutionsAll);
            
            GameManager.instance.lvl3man.InstantiateAnswersStart(GameManager.instance.playerStats.solved, GameManager.instance.playerStats.answers);
            return "";
        }

        else if (GameManager.instance.playerStats.savedEq4 && GameManager.instance.level == 4)
        {
            SetUpThermo(GameManager.instance.playerStats.pociatocna4);
            InstantiateStonesLVL3(GameManager.instance.playerStats.kamene4, false);
            SetUpThermoLVL3(GameManager.instance.playerStats.finalna4);
            SetUpFinal(GameManager.instance.playerStats.finalna4);
            SetUpThermo(GameManager.instance.playerStats.pociatocna4);
            SetUpSolutionsNumber(GameManager.instance.playerStats.solutionsGot4, GameManager.instance.playerStats.solutionsAll4);
            
            GameManager.instance.lvl3man.InstantiateAnswersStart(GameManager.instance.playerStats.solved4, GameManager.instance.playerStats.answers4);
            GameManager.instance.lvl3man.ShowButtons();
            return "";
        }

        else
        {
            if (GameManager.instance.level == 3)
            {
                int[] b = GetTaskBounds(GameManager.instance.level);
                thirdLevelGenerator.ThirdLevelEquasion((b[0] == 1) ? true : false, (b[1] == 1) ? true : false, b[2], b[3], b[4], b[5]);
                return "";
            }

            else if(GameManager.instance.level == 4)
            {
                int[] b = GetTaskBounds(GameManager.instance.level);
                thirdLevelGenerator.FourthLevelEquasion((b[0] == 1) ? true : false, b[1], b[2], b[3], b[4]);
                return "";
            }

            else if(GameManager.instance.level == 2)
            {
                int[] b = GetTaskBounds(GameManager.instance.level);
                return firstLevelGenerator.FirstLevelEquasion(b[0], b[1], b[2], (b[3]==1)?true:false, b[4], (b[5] == 1) ? true : false, (b[6] == 1) ? true : false);
            }

            else 
            {
                int[] b = GetTaskBounds(GameManager.instance.level);
                return firstLevelGenerator.FirstLevelEquasion(b[0], b[1], b[2]);
            }
        }          
    }

    private int[] GetTaskBounds(int level)
    {
        if(level == 1)
        {
            int prog = GameManager.instance.playerStats.level_1;
            int x = (int)Math.Ceiling((double)((prog + 1) / 2));
            int min = 1+x;
            int max = 1+(x+3) - (x%3);
            return new int[]{min,max,x+3};
        }

        if(level == 2)
        {
            int prog = GameManager.instance.playerStats.level_2;
            Debug.Log(prog);
            int x = (int)Math.Ceiling((double)((prog + 1) / 2));
            int min = 1+x;
            int max = 1 + (x + 3) - (x % 3);
            int zatvorka = (prog >= 5) ? 1 : 0;
            int pocetzat = x - 1;
            int minus = (prog >= 7) ? 1 : 0;
            int vymen = (prog >= 3) ? 1 : 0;
            int pocet = (prog >= 5) ? x + 2 : x + 3;
            return new int[] { min, max, pocet, zatvorka, pocetzat, minus, vymen};
        }

        if(level == 3)
        {
            int prog = GameManager.instance.playerStats.level_3;
            int[] p = {3,3,4,4,5,3,4,3,5,6};
            int x = (int)Math.Ceiling((double)((prog + 1) / 2));
            int zaporne = (prog >= 5)? 1 : 0;
            if (zaporne==1) GameManager.instance.lvl3man.TurnOnButton();
            else GameManager.instance.lvl3man.TurnOffButton();
            int two = (prog < 2 || (prog > 4 && prog < 8))? 1 : 0;
            return new int[] {zaporne, two, 2, 8, p[prog], p[prog]};
        }

        if(level == 4)
        {
            int prog = GameManager.instance.playerStats.level_4;
            int[] p = { 3, 3, 3, 4, 4, 4, 5, 5, 6, 6 };
            int x = (int)Math.Ceiling((double)((prog + 1) / 2));
            int two = (prog < 2 || (prog > 4 && prog < 7)) ? 1 : 0;
            return new int[] { two, 2, 8, p[prog], p[prog] };
        }

        return new int[] { 5, 10, 5 };
    }

    public int GetAnswer()
    {
        if (GameManager.instance.level == 1) return GameManager.instance.playerStats.finalna;
        else if (GameManager.instance.level == 2) return GameManager.instance.playerStats.finalna2;
        else if (GameManager.instance.level == 3) return GameManager.instance.playerStats.pociatocna3;
        else return GameManager.instance.playerStats.pociatocna4;
    }

    public void InstantiateStonesLVL3(List<int> stones, bool zaporne)
    {
        if (!zaporne)
        {
            kamene = GameObject.Find("kamene");
        }
        else 
        {
            kamene = GameObject.Find("kamene2");
            Image im = kamene.GetComponent<Image>();
            im.color = new Color(im.color.r, im.color.g, im.color.b, 0.5f);
        }

        GameObject g;

        for (int i = 0; i < stones.Count; i++)
        {
            if(!zaporne) kamene = GameObject.Find("kamene"+(i+1)+"i");

            if (stones[i] < 0)
            {
                g = (zaporne)? Instantiate(drag_studeny2):Instantiate(drag_studeny);
                TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (stones[i] == -1) ? "" : stones[i] * -1 + "";
                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
                g.name = "drag_studeny2";
                g.transform.localScale = new Vector3(1, 1, 1);
                naPloche.Add(g);
            }
            else if (stones[i] != 0)
            {
                g = (zaporne)? Instantiate(drag_horuci2) : Instantiate(drag_horuci);
                TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (stones[i] == 1) ? "" : stones[i] + "";
                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
                g.name = "drag_horuci2";
                g.transform.localScale = new Vector3(1, 1, 1);
                naPloche.Add(g);
            }

        }
    }

    public void SetUpFinal(int i)
    {
        Debug.Log("finalna: " + i);
        TextMeshProUGUI t = GameObject.Find("final").GetComponent<TextMeshProUGUI>();
        t.text = i + "";
    }

    public void SetUpSolutionsNumber(int ii, int i)
    {
        TextMeshProUGUI t = GameObject.Find("solutions").GetComponent<TextMeshProUGUI>();
        t.text = ii + "/" + i;
    }

}
