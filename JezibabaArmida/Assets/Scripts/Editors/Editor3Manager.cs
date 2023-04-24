using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Editor3Manager : MonoBehaviour
{
    [SerializeField] GameObject studeny;
    [SerializeField] GameObject horuci;
    [SerializeField] GameObject slider2;

    Slider slider;
    TextMeshProUGUI value;
    LVL3Manager lvl3Manager;

    public bool stvrty = false;
    public bool valid = false;
    public bool infine = false;
    public bool zero = false;
    public int pocet_kamenov;
    public bool prvy_slot = false;
    public bool druhy_slot = false;

    public int prvy_kamen = 0;
    public int druhy_kamen = 0;

    List<List<int>> solutions = new List<List<int>>();
    int poc = 0;
    int vysledna = 0;

    private void Start()
    {
        if (GameObject.Find("infine_btn") != null) stvrty = true;
        lvl3Manager = GameObject.Find("LVL3Manager").GetComponent<LVL3Manager>();
        pocet_kamenov = 0;
        GameObject kamene2 = GameObject.Find("kamene2");
        slider = GameObject.Find("SliderEditor").GetComponent<Slider>();
        for (int i = 2; i < 10; i++)
        {
            GameObject h = Instantiate(horuci);
            TextMeshProUGUI v = h.transform.Find("value").GetComponent<TextMeshProUGUI>();
            v.text = (i == 1) ? "" : i + "";
            h.transform.SetParent(kamene2.transform);
            h.transform.SetAsLastSibling();
            h.name = "horuci";
            h.transform.localScale = new Vector3(1, 1, 1);
            if (stvrty)
            {
                GameObject s = Instantiate(studeny);
                v = s.transform.Find("value").GetComponent<TextMeshProUGUI>();
                v.text = (i == 1) ? "" : i + "";
                s.transform.SetParent(kamene2.transform);
                s.transform.SetAsLastSibling();
                s.name = "studeny";
                s.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void CheckTask()
    {
        poc = (int)slider.value;
        vysledna = 0;
        int target = vysledna - poc;
        solutions = new List<List<int>>();
        int i = 0;
        if(prvy_kamen == druhy_kamen)
        {
            StartCoroutine(ChangeColor(true));
            SetSolutions(0);
            return;
        }
        while (solutions.Count < 3 || solutions.Count > 5)
        {
            vysledna = UnityEngine.Random.Range(poc + 1, poc + (Math.Max(prvy_kamen, druhy_kamen) * 6));
            target = vysledna - poc;
            if(prvy_kamen < 0 || druhy_kamen < 0 && stvrty)
            {
                solutions = CombinationSum(new int[] { prvy_kamen, druhy_kamen }, target, true);
            }
            else solutions = CombinationSum(new int[] { prvy_kamen, druhy_kamen}, target, false);
            i++;
            if (i >= 60) break;
        }
        if(i != 60)
        {
            if (prvy_kamen < 0 || druhy_kamen < 0)
            {
                infine = true;
                SetSolutions(solutions.Count);
                return;
            }
            lvl3Manager.ClearAnswers();
            lvl3Manager.InstantiateAnswersStart(solutions,solutions);
            StartCoroutine(ChangeColor(false));
            SetSolutions(solutions.Count);
            valid = true;
        }
        else
        {
            if (stvrty)
            {
                zero = true;
            }
            else
            {
                StartCoroutine(ChangeColor(true));
                SetSolutions(0);
            }
        }
    }

    public void SubmitTask()
    {
        List<int> kamene = new List<int> { prvy_kamen, druhy_kamen };

        if (stvrty)
        {
            GameManager.instance.playerStats.kamene4 = new List<int>();
            GameManager.instance.playerStats.kamene4.AddRange(kamene);
            GameManager.instance.playerStats.kameneNaPloche4 = kamene;
            GameManager.instance.playerStats.solved4 = solutions;
            GameManager.instance.playerStats.finalna4 = vysledna;
            GameManager.instance.playerStats.pociatocna4 = poc;
            GameManager.instance.playerStats.savedEq4 = true;
            GameManager.instance.playerStats.savedEditor4 = true;
            GameManager.instance.playerStats.answers4 = new List<List<int>>();
            GameManager.instance.playerStats.solutionsGot4 = 0;
            GameManager.instance.playerStats.infine = (infine)? true: false;
            GameManager.instance.playerStats.noSolutions = (zero) ? true : false;
            GameManager.instance.playerStats.solutionsAll4 = (GameManager.instance.playerStats.noSolutions) ? UnityEngine.Random.Range(3, 5) : solutions.Count;

            SceneManager.LoadScene(4);
        }
        else
        {
            GameManager.instance.playerStats.kamene3 = new List<int>();
            GameManager.instance.playerStats.kamene3.AddRange(kamene);
            GameManager.instance.playerStats.kameneNaPloche = kamene;
            GameManager.instance.playerStats.solved = solutions;
            GameManager.instance.playerStats.finalna3 = vysledna;
            GameManager.instance.playerStats.pociatocna3 = poc;
            GameManager.instance.playerStats.savedEditor3 = true;
            GameManager.instance.playerStats.savedEq3 = true;
            GameManager.instance.playerStats.zaporne = false;
            GameManager.instance.playerStats.answers = new List<List<int>>();
            GameManager.instance.playerStats.solutionsGot = 0;
            GameManager.instance.playerStats.solutionsAll = solutions.Count;

            SceneManager.LoadScene(3);
        }
    }

    private void Update()
    {
        if(prvy_slot && druhy_slot && valid)
        {
            GameObject.Find("next_btn").GetComponent<Button>().interactable = true;
            slider2.SetActive(true);
            SetSlider2Value(vysledna);
        }
        else if(prvy_slot && druhy_slot && infine)
        {
            GameObject.Find("next_btn").GetComponent<Button>().interactable = true;
            ChangeColorButton(true,true);
            slider2.SetActive(true);
            SetSlider2Value(vysledna);
        }
        else if (prvy_slot && druhy_slot && zero)
        {
            GameObject.Find("next_btn").GetComponent<Button>().interactable = true;
            ChangeColorButton(false,true);
            slider2.SetActive(true);
            SetSlider2Value(vysledna);
        }
        else if(GameObject.Find("next_btn").GetComponent<Button>().interactable == true)
        {
            GameObject.Find("next_btn").GetComponent<Button>().interactable = false;
            slider2.SetActive(false);
            infine = false;
            zero = false;
            ChangeColorButton(false, false);
            ChangeColorButton(true, false);
        }
    }

    private void SetSlider2Value(int vysledna)
    {
        slider2.GetComponent<Slider>().value = vysledna;
        GameObject.Find("final2").GetComponent<TextMeshProUGUI>().text = vysledna + "";
    }

    public void SetSolutions(int count)
    {
        if (count == 0) 
        {
            zero = false;
            infine = false;
            valid = false;
            lvl3Manager.ClearAnswers();
        }
        GameObject.Find("solutions").GetComponent<TextMeshProUGUI>().text = "0/" + count;
    }
    IEnumerator ChangeColor(bool bad)
    {
        TextMeshProUGUI t = GameObject.Find("solutions").GetComponent<TextMeshProUGUI>();
        if (bad) t.color = Color.red; else t.color = Color.green;
        yield return new WaitForSeconds(1.4f);
        t.color = Color.white;
    }

    void ChangeColorButton(bool inf, bool on)
    {
        Image t = (inf)? GameObject.Find("infine_btn").GetComponent<Image>(): GameObject.Find("zero_btn").GetComponent<Image>();
        t.color = (on)? Color.green:Color.grey;
    }

    public static List<List<int>> CombinationSum(int[] nums, int target, bool depth)
    {
        List<List<int>> result = new List<List<int>>();
        Array.Sort(nums);
        Backtrack(nums, target, 0, new List<int>(), result, 0, depth);
        return result;
    }

    private static void Backtrack(int[] nums, int target, int start, List<int> list, List<List<int>> result, int depth, bool d)
    {
        if (target == 0)
        {
            result.Add(new List<int>(list));
        }
        if (depth >= 10 && d) return;
        else if (target > 0)
        {
            for (int i = start; i < nums.Length && nums[i] <= target; i++)
            {
                if (nums[i] < 0 && (depth >= 10 && d))    //&& depth >= 8
                {
                    continue;
                }
                list.Add(nums[i]);
                Backtrack(nums, target - nums[i], i, list, result, depth + 1, d);
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
