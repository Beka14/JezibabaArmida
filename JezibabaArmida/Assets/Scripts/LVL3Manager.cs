using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class LVL3Manager : MonoBehaviour
{
    [SerializeField] GameObject holder;
    [SerializeField] GameObject studeny;
    [SerializeField] GameObject horuci;
    [SerializeField] GameObject solvedScreen;
    [SerializeField] GameObject solved;
    [SerializeField] GameObject reset_btn;
    [SerializeField] GameObject infine_btn;
    [SerializeField] GameObject noSolutions_btn;

    [SerializeField] public List<List<int>> odpovede;
    private Dictionary<int[], GameObject> holderBook;
    private void Start()
    {
        odpovede = new List<List<int>>();
        holderBook = new Dictionary<int[], GameObject>();
    }

    public void ShowButtons()
    {
        if (GameManager.instance.level == 3) return;
        infine_btn.SetActive(true);
        noSolutions_btn.SetActive(true);
    }

    void ResetButtons()
    {
        CancelInvoke();
        infine_btn.SetActive(false);
        noSolutions_btn.SetActive(false);
        Invoke("ShowButtons", 15f);
    }

    public void TurnOnButton()
    {
        reset_btn.SetActive(true);
        Image i = GameObject.Find("kamene2").GetComponent<Image>();
        i.color = new Color(i.color.r,i.color.g,i.color.b,0.5f);
    }

    public void TurnOffButton()
    {
        reset_btn.SetActive(false);
        Image i = GameObject.Find("kamene2").GetComponent<Image>();
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0f);
    }

    public void ResetStones()
    {
        foreach (int[] i in holderBook.Keys)
        {
            holderBook[i].SetActive(false);
        }

        GameManager.instance.DeleteStonesFromKotol("kamene2");

        if(GameManager.instance.level == 3)
        {
            GameManager.instance.playerStats.answers = new List<List<int>>();
            odpovede = new List<List<int>>();
            GameManager.instance.boardScript.InstantiateStonesLVL3(GameManager.instance.playerStats.kamene3, true);
            GameManager.instance.playerStats.solutionsGot = 0;
            GameManager.instance.boardScript.SetUpSolutionsNumber(0, GameManager.instance.playerStats.solutionsAll);
            GameManager.instance.playerStats.kameneNaPloche = new List<int>();
            GameManager.instance.playerStats.kameneNaPloche.AddRange(GameManager.instance.playerStats.kamene3);
        }
        else
        {
            GameManager.instance.playerStats.answers4 = new List<List<int>>();
            odpovede = new List<List<int>>();
            GameManager.instance.boardScript.InstantiateStonesLVL3(GameManager.instance.playerStats.kamene4, false);
            GameManager.instance.playerStats.solutionsGot4 = 0;
            GameManager.instance.boardScript.SetUpSolutionsNumber(0, GameManager.instance.playerStats.solutionsAll4);
            GameManager.instance.playerStats.kamene4 = new List<int>();
        }
    }

    public void SetUpAnswers(List<List<int>> objekt, List<List<int>> odpov)
    {
        ResetButtons();
        odpovede = new List<List<int>>();
        holderBook = new Dictionary<int[], GameObject>();
        ClearAnswers();
        foreach(List<int> o in objekt)
        {
            List<int> pole = new List<int>();
            //inst holder, setup parent
            GameObject h = Instantiate(holder);
            h.transform.SetParent(solved.transform);
            h.transform.SetAsLastSibling();
            h.name = "holder";
            h.transform.localScale = new Vector3(1, 1, 1);
            foreach (int i in o)
            {
                //setup stones parent holder
                pole.Add(i);
                GameObject g;
                if(i < 0)
                {
                    g = Instantiate(studeny);
                    TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                    v.text = (i == -1) ? "" : i * -1 + "";
                    g.transform.SetParent(h.transform);
                    g.transform.SetAsLastSibling();
                    g.transform.localScale = new Vector3(1, 1, 1);
                    g.name = "studeny";
                }
                else
                {
                    g = Instantiate(horuci);
                    TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                    v.text = (i == 1) ? "" : i + "";
                    g.transform.SetParent(h.transform);
                    g.transform.SetAsLastSibling();
                    g.transform.localScale = new Vector3(1, 1, 1);
                    g.name = "studeny";
                }
            }

            int[] p = pole.ToArray();
            Array.Sort(p);
            holderBook.Add(p, h);
            Debug.Log(string.Join(",", p));
            //znizit opacity kazdeho holdera na 0
            h.SetActive(false);

            if(GameManager.instance.level==3) GameManager.instance.playerStats.solved.Add(p.ToList<int>());
            else GameManager.instance.playerStats.solved4.Add(p.ToList<int>());
        }
        
        if(GameManager.instance.level == 3) GameManager.instance.playerStats.answers = odpovede;
        else GameManager.instance.playerStats.answers4 = odpovede;
    }

    IEnumerator InstantiateAnswers(List<List<int>> objekt, List<List<int>> odpov)
    {
        yield return new WaitForEndOfFrame();
        holderBook = new Dictionary<int[], GameObject>();
        ;
        foreach (List<int> o in objekt)
        {
            List<int> pole = new List<int>();
            //inst holder, setup parent
            GameObject h = Instantiate(holder);
            h.transform.SetParent(solved.transform);
            h.transform.SetAsLastSibling();
            h.name = "holder";
            h.transform.localScale = new Vector3(1, 1, 1);
            foreach (int i in o)
            {
                //setup stones parent holder
                pole.Add(i);
                GameObject g;
                if (i < 0)
                {
                    g = Instantiate(studeny);
                    TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                    v.text = (i == -1) ? "" : i * -1 + "";
                    g.transform.SetParent(h.transform);
                    g.transform.SetAsLastSibling();
                    g.transform.localScale = new Vector3(1, 1, 1);
                    g.name = "studeny";
                }
                else
                {
                    g = Instantiate(horuci);
                    TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                    v.text = (i == 1) ? "" : i + "";
                    g.transform.SetParent(h.transform);
                    g.transform.SetAsLastSibling();
                    g.transform.localScale = new Vector3(1, 1, 1);
                    g.name = "studeny";
                }
            }

            int[] p = pole.ToArray();
            Array.Sort(p);
            holderBook.Add(p, h);
            h.SetActive(false);
        }

        odpovede = new List<List<int>>();

        foreach (List<int> saved in odpov)
        {
            ContainsAnswer(saved);
        }
    }

    public void InstantiateAnswersStart(List<List<int>> objekt, List<List<int>> odpov)
    {
        StartCoroutine(InstantiateAnswers(objekt, odpov));
    }

    public void ClearAnswers()
    {
        GameObject s = solved;
        for (var i = s.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(s.transform.GetChild(i).gameObject);
        }
    }

    public bool ContainsAnswer(List<int> odpoved)
    {
        int[] p = odpoved.ToArray();
        Array.Sort(p);
        bool obsahuje = false;

        foreach (List<int> i in odpovede)                                           
        {
            if (string.Join(",", i) == string.Join(",", p)) obsahuje = true;
        }

        foreach (int[] k in holderBook.Keys)                              
        {
            if ((string.Join(",", p) == string.Join(",", k)) && !obsahuje)
            {
                holderBook[k].SetActive(true);
                odpovede.Add(k.ToList<int>());
                if(GameManager.instance.level == 3 && !GameManager.instance.playerStats.answers.Contains(odpoved)) GameManager.instance.playerStats.answers.Add(odpoved);
                else if (GameManager.instance.level == 4 && !GameManager.instance.playerStats.answers4.Contains(odpoved)) GameManager.instance.playerStats.answers4.Add(odpoved);
                return true;
            }
        }

        return false;
    }

}
