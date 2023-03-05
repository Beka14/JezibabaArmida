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
    NumberSlider numberSlider;

    int answer;

    List<int> vsetky = new List<int>();
    List<GameObject> naPloche = new List<GameObject>();
    List<string> znamienka = new List<string>();
    Dictionary<int, string> zatvorky = new Dictionary<int, string>();

    private void Start()
    {
        //Debug.Log("*************boardSCRIPT START**************");
        kamene = GameObject.Find("kamene");
        thermometer = GameObject.Find("Slider");
        numberSlider = GameObject.Find("Numbers").GetComponent<NumberSlider>();
        if(TryGetComponent<Thermometer>(out Thermometer thermoScript)) thermoScript = thermometer.GetComponent<Thermometer>();
        //GetComponent<GameManager>().Init();
    }
    
    public string FirstLevelEquasion(int pocet_kamenov)
    {
        List<int> list = new List<int>();
        List<int> kamene = new List<int>();
        int max = 48;
        int min = 11;
        bool ok = false;

        vsetky = new List<int>();
        znamienka = new List<string>();
        zatvorky = new Dictionary<int, string>();

        int pociatocna = Random.Range(min, max + 1);
        int studenyKamen = Random.Range(-5,-1); //studenyKamen = Random.Range(1, 5) * -1;
        int horuciKamen = Random.Range(1, 5);
        int vysledna = Random.Range(min, max + 1);

        while (!ok)
        {
            pociatocna = Random.Range(min, max+1);
            studenyKamen = Random.Range(-5,-1); //studenyKamen = Random.Range(1, 5) * -1;
            horuciKamen = Random.Range(1, 5);
            vysledna = Random.Range(min, max + 1);

            for(int x=0; x < 1000; x++)
            {
                for(int y=0; y < 1000; y++)
                {
                    if ((pociatocna + (horuciKamen * x) + (studenyKamen * y) == vysledna) && x + y <= pocet_kamenov && x != 0 && y != 0)
                    {
                        kamene = new List<int>();
                        ok = true;
                        for (int i = 0; i < x; i++) kamene.Add(horuciKamen);
                        for (int j = 0; j < y; j++) kamene.Add(studenyKamen);
                        //Debug.Log("------------ " + string.Join(",", kamene));
                        answer = vysledna;
                        break;
                    }
                }
            }
        }

        int r = Random.Range(1,3);
        List<int> pole = new List<int>();
        if (r % 2 == 0 && kamene.Count() > 3)
        {
            //zatvorka
            int z = Random.Range(2, kamene.Count()-1);
            for(int i = z; i >=0 ; i--)                      //for(int i = 0; i < z; i++)
            {
                int x = Random.Range(0, kamene.Count() -1);
                pole.Add(kamene[x]);        //problem??
                kamene.RemoveAt(x);
            }
        }

        string tmp;
        if (GameManager.instance.level == 1) tmp = Stringify(pole, kamene);
        else
        {
            answer = pociatocna;
            tmp = Stringify_lvl2(pole, kamene);
        } 

        //TODO 

        SetUpThermo(pociatocna);
        InstantiateStones(vsetky, znamienka);
        if(numberSlider == null) numberSlider = GameObject.Find("Numbers").GetComponent<NumberSlider>();
        numberSlider.SetMinMax(vysledna);

        //

        // SAVE EQ

        if(GameManager.instance.level == 1)
        {
            GameManager.instance.playerStats.kamene = vsetky;
            GameManager.instance.playerStats.pociatocna = pociatocna;
            GameManager.instance.playerStats.rovnica = tmp;
            GameManager.instance.playerStats.finalna = vysledna;
            GameManager.instance.playerStats.savedEq = true;
            GameManager.instance.playerStats.level = GameManager.instance.level;
        }

        else if (GameManager.instance.level == 2) 
        {
            GameManager.instance.playerStats.kamene2 = vsetky;
            GameManager.instance.playerStats.znamienka2 = znamienka;
            GameManager.instance.playerStats.zatvorky = zatvorky;
            GameManager.instance.playerStats.pociatocna2 = pociatocna;
            GameManager.instance.playerStats.rovnica2 = tmp;
            GameManager.instance.playerStats.finalna2 = answer;
            GameManager.instance.playerStats.savedEq2 = true;
            GameManager.instance.playerStats.level = GameManager.instance.level;
        }

        //

        Debug.Log("POROVNANIE: poc: " + vysledna + " answ: " + answer);
        return pociatocna + "" + tmp + " = " + answer; 
    }

    string Stringify(List<int> a, List<int> b)
    {
        //Debug.Log("------------2 " + string.Join(",", a) + " // " + string.Join(",", b));
        string sb = "";
        int r = Random.Range(0,b.Count());
        for(int i=0;i<r;i++)
        {
            sb += (b[i] < 0) ? " - " + -1*b[i] : " + "+b[i];
            vsetky.Add(b[i]);
        }

        if(a.Count() != 0)
        {
            int z = Random.Range(1,3);
            if (z % 2 == 0)
            {
                sb += " + (";
            }
            else sb += " + (";
            for (int i = 0; i < a.Count(); i++)
            {
                sb += (a[i] < 0) ? "-" + -1 * a[i] : "+" + a[i];        //TODO druhy level: sb.append((a.get(i) < 0) ? "-" + (-1*a.get(i)): "+" + a.get(i));
                vsetky.Add(a[i]);
            }
            sb += ")";
        }

        for (int i = r; i < b.Count(); i++)  //i < b.Count-r
        {
            sb += (b[i] < 0) ? " - " + -1 * b[i] : " + " + b[i];
            vsetky.Add(b[i]);
        }

        //Debug.Log("------------3 " + string.Join(",", vsetky));
        return sb;
    }



    string Stringify_lvl2(List<int> a, List<int> b)
    {
        int index = 0;
        string sb = "";
        int r = Random.Range(1, b.Count()-1);
        for (int i = 0; i < r; i++)
        {
            sb += (b[i] < 0) ? "-" + -1 * b[i] : "+" + b[i];
            vsetky.Add(b[i]);
            answer += b[i];
            znamienka.Add((b[i] < 0) ? "+" : "+");
            index++;
        }

        if (a.Count() != 0)
        {
            int x = 0;
            int p = Random.Range(1, 3);
            if (p % 2 == 0)
            {
                sb += "-(";
                zatvorky.Add(index,"-(");
                for (int i = 0; i < a.Count(); i++)
                {
                    int z = Random.Range(1, 4);
                    if (z % 3 == 0)
                    {
                        sb += (a[i] < 0) ? "-" + -1 * a[i] : "-" + a[i];
                        vsetky.Add((a[i] < 0) ? -1 * a[i] : -1 * a[i]);
                        x += a[i];
                        znamienka.Add("-");
                        index++;
                    }
                    else
                    {
                        sb += (a[i] < 0) ? "-" + -1 * a[i] : "+" + a[i];
                        vsetky.Add(a[i]);
                        x += a[i];
                        znamienka.Add((a[i] < 0) ? "+" : "+");
                        index++;
                    }

                }
                Debug.Log("x: " + x);
                answer += -1 * x;
                sb += ")";
                zatvorky.Add(index,")");
            }
            else
            {
                sb += "+(";
                zatvorky.Add(index, "+(");
                for (int i = 0; i < a.Count(); i++)
                {
                    int z = Random.Range(1, 4);
                    if (z % 3 == 0)
                    {
                        sb += (a[i] < 0) ? "-" + -1 * a[i] : "-" + a[i];
                        vsetky.Add((a[i] < 0) ? -1 * a[i] : -1 * a[i]);
                        znamienka.Add("-");
                        answer += a[i];
                        index++;
                    }
                    else
                    {
                        sb += (a[i] < 0) ? "-" + -1 * a[i] : "+" + a[i];
                        vsetky.Add(a[i]);
                        znamienka.Add((a[i] < 0) ? "+" : "+");
                        answer += a[i];
                        index++;
                    }
                }
                sb += ")";
                zatvorky.Add(index, ")");
            }
            Debug.Log("------------3 " + string.Join(",", zatvorky));
            /*
            for (int i = 0; i < a.Count(); i++)
            {
                int z = Random.Range(1, 4);
                if (z % 3 == 0)
                {
                    sb += (a[i] < 0) ? "-" + -1 * a[i] : "-" + a[i];
                    vsetky.Add((a[i] < 0) ? -1 * a[i] : -1 * a[i]);
                    znamienka.Add("-");
                }
                else {
                    sb += (a[i] < 0) ? "-" + -1 * a[i] : "+" + a[i];
                    vsetky.Add(a[i]);
                    znamienka.Add((a[i] < 0) ? "+" : "+");
                }   
            }
            sb += ")";
            */
        }

        for (int i = r; i < b.Count(); i++)  //i < b.Count-r
        {
            int z = Random.Range(1, 4);
            if (z % 3 == 0)
            {
                sb += (b[i] < 0) ? "-" + -1 * b[i] : "-" + b[i];
                vsetky.Add((b[i] < 0) ? -1 * b[i] : -1 * b[i]);
                answer += b[i];
                znamienka.Add("-");
            }
            else
            {
                sb += (b[i] < 0) ? "-" + -1 * b[i] : "+" + b[i];
                vsetky.Add(b[i]);
                answer += b[i];
                znamienka.Add((b[i] < 0) ? "+" : "+");
            }
        }

        //Debug.Log("------------3 " + string.Join(",", vsetky));
        return sb;
    }

    public void SetUpThermo(int value)
    {
        kamene = GameObject.Find("kamene");
        thermometer = GameObject.Find("Slider");
        thermoScript = thermometer.GetComponent<Thermometer>();
        //Debug.Log(thermometer.name);
        thermoScript.SetValue(value);
    }

    public void SetUpThermoLVL3(int value)
    {
        GameObject thermometer2 = GameObject.Find("Slider2");
        Thermometer thermo = thermometer2.GetComponent<Thermometer>();
        thermo.SetValue(value);
    }

    public void InstantiateStones(List<int> stones, List<string> znam)     
    {
        int index = 0;
        kamene = GameObject.Find("kamene");
        if(GameManager.instance.level == 1) kamene.GetComponent<GridLayoutGroup>().spacing = new Vector2(15,0);
        else kamene.GetComponent<GridLayoutGroup>().spacing = new Vector2(0, 0);
        GameObject g;

        for(int i = 0; i < stones.Count; i++)
        {

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

            if (zatvorky.ContainsKey(index))
            {
                if (zatvorky[index] == "+(")
                {
                    g = Instantiate(plusPar);
                    g.transform.localScale = new Vector3(1, 1, 1);
                }

                else if (zatvorky[index] == "-(") g = Instantiate(minusPar);

                else g = Instantiate(endPar);

                g.transform.SetParent(kamene.transform);
                g.transform.SetAsLastSibling();
                g.transform.localScale = new Vector3(1, 1, 1);
                naPloche.Add(g);
            }

            if (znam.Count() != 0 && i+1 < znam.Count())
            {
                //Debug.Log("------------ " + string.Join(",", znamienka));
                if (znam[i+1] == "-")
                {
                    g = Instantiate(minus);
                    g.transform.SetParent(kamene.transform);
                    g.transform.SetAsLastSibling();
                    g.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                }
                else
                {
                    g = Instantiate(plus);
                    g.transform.SetParent(kamene.transform);
                    g.transform.SetAsLastSibling();
                    g.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                }

                naPloche.Add(g);
            }

            //naPloche.Add(g);
            //g.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public string SetUpBoard(int x)
    {
        /*
        for (int i=0; i<naPloche.Count; i++)
        {
            Debug.Log("------REMOVE------ " + string.Join(",", naPloche));
            //GameObject a = naPloche[0];
            Destroy(naPloche[0]);
        }
        //Debug.Log(naPloche.Count);
        naPloche = new List<GameObject>();
        */
        

        GameObject g = GameObject.Find("kamene");
        for (var i = g.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(g.transform.GetChild(i).gameObject);
        }

        if(GameManager.instance.playerStats.zaporne)
        {
            GameObject gg = GameObject.Find("kamene2");
            for (var i = gg.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(gg.transform.GetChild(i).gameObject);
            }
        }

        ////////// SAVED EQ

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
            /*
            if(GameManager.instance.playerStats.level_3 == 2)
            {
                GameManager.instance.playerStats.savedEq3 = false;
                ThirdLevelEquasion(true);
                return "";
            }
            */

            SetUpThermo(GameManager.instance.playerStats.pociatocna3);
            InstantiateStonesLVL3(GameManager.instance.playerStats.kameneNaPloche, GameManager.instance.playerStats.zaporne);
            //SetUpFinal(GameManager.instance.playerStats.finalna3);
            SetUpThermoLVL3(GameManager.instance.playerStats.finalna3);
            SetUpFinal(GameManager.instance.playerStats.finalna3);
            SetUpThermo(GameManager.instance.playerStats.pociatocna3);
            SetUpSolutionsNumber(GameManager.instance.playerStats.solutionsGot, GameManager.instance.playerStats.solutionsAll);
            //TODO naspat dat solutions do lvl3managera
            GameManager.instance.lvl3man.InstantiateAnswersStart(GameManager.instance.playerStats.solved, GameManager.instance.playerStats.answers);

            //GameManager.instance.lvl3man.SetUpAnswers(GameManager.instance.playerStats.solved, GameManager.instance.playerStats.answers);
            return "";
        }

        else
        {
            if (GameManager.instance.level == 3) 
            {
                //GameManager.instance.lvl3man.ClearAnswers();
                if (GameManager.instance.playerStats.level_3 == 1)
                {
                    ThirdLevelEquasion(true);
                    return "";
                }

                ThirdLevelEquasion();
                return "";
            }

            else return FirstLevelEquasion(x);
        }
       
        ////////// SAVED EQ
        //return FirstLevelEquasion(x);           
    }

    public int GetAnswer()
    {
        if(GameManager.instance.level == 1) return GameManager.instance.playerStats.finalna;
        else if(GameManager.instance.level == 2) return GameManager.instance.playerStats.finalna2;
        else return GameManager.instance.playerStats.pociatocna3;
    }



    public void ThirdLevelEquasion(bool zaporne = false)
    {
        //zaporne = true;                     //TODO po debugu vymazat
        List<int> list = new List<int>();
        List<int> kamene = new List<int>();
        List<List<int>> solved = new List<List<int>>();
        int max = 80;
        int min = 0;
        bool ok = false;

        int pociatocna = Random.Range(min, max + 1);
        int prvy = Random.Range(2, 8);
        int druhy = Random.Range(2, 8);
        int treti = Random.Range(2, 8);
        int vysledna = Random.Range(pociatocna, pociatocna + 20);

        if(!zaporne) while (!ok)
        {
            solved = new List<List<int>>();
            pociatocna = Random.Range(min, max + 1);
            prvy = Random.Range(2, 8);
            while (prvy == druhy) druhy = Random.Range(2, 8);
            while (treti == druhy || treti == prvy) treti = Random.Range(2, 8);
            while (vysledna == pociatocna) vysledna = Random.Range(pociatocna, pociatocna + 20);    //TODO ZAPORNE CISLO

            Debug.Log(pociatocna + " + " + prvy + " + " + druhy + " + " + treti + " = " + vysledna);

            int target = vysledna - pociatocna;
            List<List<int>> result = CombinationSum(new int[]{prvy,druhy,treti}, target, false);
            if (result.Count() < 3 || result.Count > 6)
            {
                continue;
            }
            else if (result.Count() >= 3 && result.Count() <= 6)
            {
                ok = true;
                solved = result;
                kamene = new List<int> { prvy, druhy, treti };
            }
        }

        else while (!ok)
        {
            solved = new List<List<int>>();
            pociatocna = Random.Range(min, max + 1);
            prvy = Random.Range(2, 9);                                                          //TODO skusit s 2 kamenmi
            while (prvy == druhy || druhy == -1 || druhy == 1 || druhy == 0) druhy = Random.Range(-5, 8);
            while (treti == druhy || treti == prvy) treti = -1 * Random.Range(2, 8);
            while (vysledna == pociatocna) vysledna = Random.Range(pociatocna, pociatocna + 20);    //TODO ZAPORNE CISLO

            Debug.Log(pociatocna + " + " + prvy + " + " + druhy + " + " + treti + " = " + vysledna);

            int target = vysledna - pociatocna;
            List<List<int>> result = CombinationSum(new int[] { prvy, druhy, treti }, target, true);         //druhy
            if (result.Count() < 2 || result.Count > 6)
            {
                continue;
            }
            else if (result.Count() >= 2 && result.Count() <= 6)
            {
                ok = true;
                solved = result;
                kamene = MakeStones(result);
            }
        }

        Debug.Log("------------ " + solved.Count());
        // SOLVED INSTANTIOATE
        if (GameManager.instance.lvl3man == null) GameManager.instance.lvl3man = GameObject.Find("LVL3Manager").GetComponent<LVL3Manager>();
        GameManager.instance.lvl3man.SetUpAnswers(solved, new List<List<int>>());
        //

        //TODO 

        SetUpThermoLVL3(vysledna);
        SetUpFinal(vysledna);
        SetUpThermo(pociatocna);
        SetUpSolutionsNumber(0,solved.Count());
        InstantiateStonesLVL3(kamene, zaporne);

        //

        // SAVE EQ

        GameManager.instance.playerStats.kamene3 = kamene;
        GameManager.instance.playerStats.kameneNaPloche = kamene;
        GameManager.instance.playerStats.solved = solved;
        GameManager.instance.playerStats.finalna3 = vysledna;
        GameManager.instance.playerStats.pociatocna3 = pociatocna;
        GameManager.instance.playerStats.savedEq3 = true;
        GameManager.instance.playerStats.zaporne = zaporne;
        GameManager.instance.playerStats.answers = new List<List<int>>();
        GameManager.instance.playerStats.solutionsGot = 0;
        GameManager.instance.playerStats.solutionsAll = solved.Count();
        GameManager.instance.playerStats.level = GameManager.instance.level;

        //

        //Debug.Log("POROVNANIE: poc: " + vysledna + " answ: " + answer);
        //return pociatocna + "" + tmp + " = " + answer;
    }

    List<int> MakeStones(List<List<int>> s)
    {
        List<int> k = new List<int>();
        foreach(List<int> v in s)
        {
            foreach(int w in v) k.Add(w);   
        }

        for(int i=0; i < k.Count; i++)
        {
            int tmp = k[i];
            int ri = Random.Range(i, k.Count);
            k[i] = k[ri];
            k[ri] = tmp;
        }

        return k;
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
        }

        GameObject g;

        for (int i = 0; i < stones.Count; i++)
        {

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

    void SetUpFinal(int i)
    {
        Debug.Log("finalna: " + i);
        TextMeshProUGUI t = GameObject.Find("final").GetComponent<TextMeshProUGUI>();
        t.text = i + "";
    }

    void SetUpSolutionsNumber(int ii, int i)
    {
        TextMeshProUGUI t = GameObject.Find("solutions").GetComponent<TextMeshProUGUI>();
        t.text = ii + "/" + i;
    }

    bool HasRealSolution(int a, double a1, double a2, double a3, double b)
    {
        double[][] matrix;
        if(a3 != 0) matrix = new double[][] { new double[] { a1,a2,a3,b - a}, new double[] { 0,0,0,0}, new double[]{0,0,0,0}};
        else{
            // Create the augmented matrix without the third row
            matrix = new double[][] { new double[] { a1, a2, b - a }, new double[] { 0, 0, 0 } };
        }

        // Reduce the matrix to row echelon form
        for (int j = 0; j < 3; j++)
        {
            // Find a non-zero entry in the jth column
            int i = j;
            while (i < matrix.Count() && matrix[i][j] == 0)
            {
                i++;
            }

            if (i == matrix.Count() && matrix[i - 1][j] == 0)
            {
                // The matrix is already in row echelon form
                break;
            }

            if (i != j)
            {
                // Swap rows i and j
                double[] temp = matrix[i];
                matrix[i] = matrix[j];
                matrix[j] = temp;
            }

            // Divide the first row by the pivot element
            double pivot = matrix[j][j];
            matrix[j][j] = 1;
            for (int k = j + 1; k < 4; k++)
            {
                matrix[j][k] /= pivot;
            }

            // Subtract a multiple of the first row from each subsequent row
            for (i = j + 1; i < matrix.Count(); i++)
            {
                double factor = matrix[i][j];
                matrix[i][j] = 0;
                for (int k = j + 1; k < 4; k++)
                {
                    matrix[i][k] -= factor * matrix[j][k];
                }
            }
        }

        // Determine the rank of the matrix
        int rank = 0;
        for (int i = 0; i < matrix.Count(); i++)
        {
            bool nonZero = false;
            for (int j = 0; j < 4; j++)
            {
                if (Math.Abs(matrix[i][j]) > 1e-9)
                {
                    nonZero = true;
                    break;
                }
            }
            if (nonZero)
            {
                rank++;
            }
        }

        // Return true if the rank is 1, indicating a solution exists
        return rank == 1;
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
