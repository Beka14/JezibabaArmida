using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlaygroundManager : MonoBehaviour
{
    public static PlaygroundManager instance = null;
    [SerializeField] GameObject kotol;
    Kotol kotolScript;
    Thermometer thermometer;

    List<GameObject> kamene = new List<GameObject>();
    List<GameObject> skryteKamene = new List<GameObject>();

    int kameneMax = 0;
    int kameneCount = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        kotolScript = GetComponent<Kotol>();
        thermometer = GetComponent<Thermometer>();

        RectTransform rc = kotol.GetComponent<RectTransform>();
        GridLayoutGroup glg = kotol.GetComponent<GridLayoutGroup>();
        kameneMax = Mathf.FloorToInt(rc.rect.width / (glg.cellSize.x + glg.spacing.x)) * Mathf.FloorToInt(rc.rect.height / (glg.cellSize.y + glg.spacing.y));
        Debug.Log(kameneMax);
        //kameneMax = 6;
    }
    
    public void AddStone(GameObject kamen)     
    {
        if(kameneCount >= kameneMax)
        {
            kamen.SetActive(false);
            skryteKamene.Add(kamen);
        }
        kameneCount++;
        thermometer.ChangeValue((kamen.name == "studeny")? -1:1);
        //Debug.Log(kameneCount);
    }

    public void RemoveStone(GameObject kamen)     
    {
        if (kameneCount >= kameneMax && skryteKamene.Count > 0)
        {
            skryteKamene[0].SetActive(true);
            skryteKamene.RemoveAt(0);
            Debug.Log("activujm");
        }
        kameneCount--;
        thermometer.ChangeValue((kamen.name == "studeny") ? 1 : -1);
        //Debug.Log(kameneCount);
    }

    public int GetThermoValue()
    {
        //Debug.Log(thermometer.GetValue());
        return thermometer.GetValue();
    }
    
}
