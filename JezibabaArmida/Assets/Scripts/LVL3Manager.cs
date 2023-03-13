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

    [SerializeField] public List<List<int>> odpovede;
    private Dictionary<int[], GameObject> holderBook;
    private void Start()
    {
        odpovede = new List<List<int>>();
        holderBook = new Dictionary<int[], GameObject>();
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
        Debug.Log("reset nijee este bye");
    }

    public void SetUpAnswers(List<List<int>> objekt, List<List<int>> odpov)
    {
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

            //foreach (List<int> saved in odpov)
            //{
            // ContainsAnswer(saved);
            //}

            GameManager.instance.playerStats.solved.Add(p.ToList<int>());
        }
        
        GameManager.instance.playerStats.answers = odpovede;
    }

    IEnumerator InstantiateAnswers(List<List<int>> objekt, List<List<int>> odpov)
    {
        yield return new WaitForEndOfFrame();
        holderBook = new Dictionary<int[], GameObject>();
        //ClearAnswers();
        //Debug.Log(objekt.Count() + " odppved count: " + odpov.Count());
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
            //Debug.Log(string.Join(",", p));
            //znizit opacity kazdeho holdera na 0
            h.SetActive(false);
        }

        //odpovede = odpov;
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

        foreach (List<int> i in odpovede)                                           //TODO OPRAVIT aby nebol for loop
        {
            if (string.Join(",", i) == string.Join(",", p)) obsahuje = true;
        }

        foreach (int[] k in holderBook.Keys)                                 //TODO OPRAVIT aby nebol for loop
        {
            if ((string.Join(",", p) == string.Join(",", k)) && !obsahuje)
            {
                //Debug.Log("NACHADZA SA TU KLUC");
                holderBook[k].SetActive(true);
                odpovede.Add(k.ToList<int>());
                //Debug.Log(string.Join(" -- ", k.ToList<int>()));
                if(!GameManager.instance.playerStats.answers.Contains(odpoved))GameManager.instance.playerStats.answers.Add(odpoved);
                return true;
            }
        }
        /*
        if (holderBook.ContainsKey(p))
        {
            Debug.Log("NACHADZA SA TU KLUC");
            Image x = holderBook[p].GetComponent<Image>();
            x.color = new Color(x.color.r, x.color.g, x.color.b, 1);
            return true;
        }
        */
        return false;
    }
}
