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

    [SerializeField] List<GameObject> odpovede;
    private Dictionary<int[], GameObject> holderBook;
    private void Start()
    {
        odpovede = new List<GameObject>();
        holderBook = new Dictionary<int[], GameObject>();
    }

    public void SetUpAnswers(List<List<int>> objekt, List<List<int>> odpov)
    {
        odpovede = new List<GameObject>();
        ClearAnswers();
        foreach(List<int> o in objekt)
        {
            List<int> pole = new List<int>();
            //inst holder, setup parent
            GameObject h = Instantiate(holder);
            h.transform.SetParent(solved.transform);
            h.transform.SetAsLastSibling();
            h.name = "holder";
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
                    g.name = "studeny";
                }
                else
                {
                    g = Instantiate(horuci);
                    TextMeshProUGUI v = g.transform.Find("value").GetComponent<TextMeshProUGUI>();
                    v.text = (i == 1) ? "" : i + "";
                    g.transform.SetParent(h.transform);
                    g.transform.SetAsLastSibling();
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

        }
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
        //Debug.Log(string.Join(",", p));

        foreach (int[] k in holderBook.Keys)                                 //TODO OPRAVIT aby nebol for loop
        {
            if ((string.Join(",", p) == string.Join(",", k)) && !odpovede.Contains(holderBook[k]))
            {
                Debug.Log("NACHADZA SA TU KLUC");
                holderBook[k].SetActive(true);
                odpovede.Add(holderBook[k]); 
                if(!GameManager.instance.playerStats.answers.Contains(odpoved)) GameManager.instance.playerStats.answers.Add(odpoved);
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
