using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{

    public string NAME;
    public Sprite FLAG;
    public float DIF;
    public string TEAM;
    public Button BTN;
    public GameObject Score;


    // Start is called before the first frame update
    void Awake()
    {
        //ShowScore(false);
        BTN.onClick.AddListener(SelectTeam);
    }

    public void ShowScore(bool todo)
    {
       // Debug.Log("ShowScore+"+ todo + "+"+ NAME);
        Score.SetActive(todo);
    }

    public void SelectTeam()
    {
       // Debug.Log(Main.inst.IIHFRATING[NAME]);
        Main.inst.SetTeam(NAME,FLAG,DIF,TEAM);
    }

    public void SetScore(string score, string type = "11")
    {
        //Debug.Log(score);

        if (score != "-")
            Score.SetActive(true);
        else
            Score.SetActive(false); 

        Score.GetComponentInChildren<Text>().text = score.ToString();
        
        switch (type)
        {
            case "11":
            case "12":
            case "21":
            case "22":
            case "51":
            case "52":
                Score.transform.localPosition = new Vector3(62, 17, 0); //right
                break;
            case "41":
            case "42":
            case "31":
            case "32":
            case "61":
            case "62":
                Score.transform.localPosition = new Vector3(-62, 17, 0); //left
                break;
            case "71":
            case "72":
                Score.transform.localPosition = new Vector3(0, -45, 0);//bottom
                break;
        }
        

        
    }
}
