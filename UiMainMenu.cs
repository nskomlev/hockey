using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMainMenu : MonoBehaviour {

    public static UiMainMenu inst;

    public Button Startgame;
    public Button toChamp;
    public Button toOptions;

    public Button Help;
    public Dropdown Drop1;
    public Dropdown Drop2;
    public Transform ScrollContentP1;
    public Transform ScrollContentP2;

    public GameObject Forma1;
    public GameObject Forma2;
    public Text TeamName1;
    public Text TeamName2;
    public Text TeamLevel1;
    public Text TeamLevel2;

    public Image ChampFlag;

    void Awake()
    {
        if (inst == null) { inst = this; }
        else if (inst != null) { Destroy(gameObject); }

        Startgame.onClick.AddListener(StartGame);
        toChamp.onClick.AddListener(StartChamp);
        toOptions.onClick.AddListener(ToOptions);

        Help.onClick.AddListener(UiHelp.inst.ShowMe);
        Drop1.onValueChanged.AddListener(delegate {
            DropdownValueChanged(Drop1);
        });
        Drop2.onValueChanged.AddListener(delegate {
            DropdownValueChanged(Drop2);
        });
    }

    void DropdownValueChanged(Dropdown change)
    {
       Debug.Log(change.value);
        if (change == Drop1) 
        {
            if (change.value == 0) Main.inst.team1.GetComponent<Team>().AICONTROL = false;
            if (change.value == 1) Main.inst.team1.GetComponent<Team>().AICONTROL = true;
        }
        if (change == Drop2)
        {
            if (change.value == 1) Main.inst.team2.GetComponent<Team>().AICONTROL = false;
            if (change.value == 0) Main.inst.team2.GetComponent<Team>().AICONTROL = true;
        }
    }

    
    void ToOptions()
    {
        UiOptions.inst.show(true);
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        Main.inst.StartGame();
        Main.inst.EventController("buttonClick");
    }
    void StartChamp()
    {
        UiChamp.inst.show(true);
        UiChamp.inst.setPlayerTeam();
    }
    public void show(bool todo)
    {
        gameObject.SetActive(todo);
    }
    public void CreateFlags()
    {
        //TODO: приводим сложность флагов к балу 100
        //float TOPDIFF = float.Parse(Main.inst.FLAGSCORE[0].Split(new char[] { ',' })[0]);
        //Debug.Log(TOPDIFF);

        for (var I=0;I < Main.inst.FLAGSCORE.Length; I++)
        {

            string[] words = Main.inst.FLAGSCORE[I].Split(new char[] { ',' });
            Main.inst.IIHFRATING.Add(words[1], float.Parse(words[0]));
           // Debug.Log(words[1]+ float.Parse(words[0]));
            var tmpDif= words[0];
            var tmpName= words[1];

            var tmpFlag = Instantiate(Main.inst.FlagsPrefeb, new Vector3(0, 0, 0), Quaternion.identity, ScrollContentP1.transform);
            var tmpFlag2 = Instantiate(Main.inst.FlagsPrefeb, new Vector3(0, 0, 0), Quaternion.identity, ScrollContentP2.transform);

            tmpFlag.transform.localPosition = new Vector3(8, -I * 60 - 20, 0);
            tmpFlag2.transform.localPosition = new Vector3(8, -I * 60 - 20, 0);

            var tmpFlagImg = tmpFlag.transform.Find("Flag");
            var tmpFlagText = tmpFlag.transform.Find("Dif");
            var tmpFlagImg2 = tmpFlag2.transform.Find("Flag");
            var tmpFlagText2 = tmpFlag2.transform.Find("Dif");

            string tmpDIF100 = Main.inst.ConvertRatingToLvl(tmpDif);
            //tmpDIF100 = Mathf.Round(float.Parse(tmpDif) / TOPDIFF * 100);

            tmpFlagText.GetComponent<Text>().text = "LVL " + tmpDIF100.ToString();
            tmpFlagText2.GetComponent<Text>().text = "LVL " + tmpDIF100.ToString();
            //tmpFlagImg.GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
            tmpFlagImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Flags/" + tmpName);
            tmpFlagImg2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Flags/" + tmpName);

            tmpFlag.GetComponent<Flag>().DIF = float.Parse(tmpDIF100);
            tmpFlag2.GetComponent<Flag>().DIF = float.Parse(tmpDIF100);

            tmpFlag.GetComponent<Flag>().NAME = tmpName;
            tmpFlag2.GetComponent<Flag>().NAME = tmpName;

            tmpFlag.GetComponent<Flag>().FLAG = Resources.Load<Sprite>("Flags/" + tmpName);
            tmpFlag2.GetComponent<Flag>().FLAG = Resources.Load<Sprite>("Flags/" + tmpName);

            tmpFlag.GetComponent<Flag>().TEAM = "team1";
            tmpFlag2.GetComponent<Flag>().TEAM = "team2";


            tmpFlag.GetComponent<Flag>().ShowScore(false);
            tmpFlag2.GetComponent<Flag>().ShowScore(false);

            if (tmpFlag.GetComponent<Flag>().NAME == Saves.inst.LoadTeam(1)) 
            {
                tmpFlag.GetComponent<Flag>().SelectTeam();
            }
            if (tmpFlag.GetComponent<Flag>().NAME == Saves.inst.LoadTeam(2))
            {
                tmpFlag2.GetComponent<Flag>().SelectTeam();
            }
        }

        RectTransform rt = ScrollContentP1.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(0, Main.inst.FLAGSCORE.Length*60+30);
        RectTransform rt2 = ScrollContentP2.GetComponent<RectTransform>();
        rt2.sizeDelta = new Vector2(0, Main.inst.FLAGSCORE.Length * 60 + 30);
    }

}
