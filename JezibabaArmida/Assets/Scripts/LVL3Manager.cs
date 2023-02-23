using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LVL3Manager : MonoBehaviour
{
    [SerializeField] GameObject holder;
    [SerializeField] GameObject studeny;
    [SerializeField] GameObject horuci;
    [SerializeField] GameObject solvedScreen;
    [SerializeField] GameObject solved;

    [SerializeField] List<List<GameObject>> odpovede;
    private Dictionary<List<int>, GameObject> holderBook;
    private void Start()
    {
        odpovede = new List<List<GameObject>>();
    }

    public void AddAnswer(List<GameObject> objekt)
    {
        odpovede.Add(objekt);
    }

    public void SetUpAnswers(List<List<int>> objekt)
    {
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

            //holderBook.Add(pole,h);
            //znizit opacity kazdeho holdera na 0
        }
    }

    public void ClearAnswers()
    {
        odpovede.Clear();
    }

    public bool ContainsAnswer(List<GameObject> objekt)
    {
        return odpovede.Contains(objekt);
    }

    public void InstantiateAnswers()
    {

    }
}
