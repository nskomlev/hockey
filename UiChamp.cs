using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiChamp : MonoBehaviour
{
    public static UiChamp inst;

    public Button Startchamp;
    public Button toMenu;
    public List<string> champTeams = new List<string>();
    public GameObject playerIcon;

    void Awake()
    {
        if (inst == null) { inst = this; }
        else if (inst != null) { Destroy(gameObject); }

        if (Saves.inst.CHAMP_IS_CHAMP == 0)
        {
            Saves.inst.CHAMP_IS_CHAMP = 0;
            Saves.inst.CHAMP_ETAP = 4;
            show(false);
        }
        else //ЕСЛИ ЧЕМПИОНАТ 
        {
            
            
            if (Saves.inst.CHAMP_IS_CHAMP == 1 && Saves.inst.CHAMP_ETAP == 0) 
            {
                Startchamp.gameObject.SetActive(false);
                //TODO: YOUWIN SOUND AND OTHER!!!!
            }

            switch (Saves.inst.CHAMP_ETAP)
            {
                case 4: playerIcon.GetComponent<RectTransform>().localPosition = new Vector2(-387, 171); break;
                case 2: playerIcon.GetComponent<RectTransform>().localPosition = new Vector2(-252, 73); break;
                case 1: playerIcon.GetComponent<RectTransform>().localPosition = new Vector2(-110, 32); break;
                case 0: playerIcon.GetComponent<RectTransform>().localPosition = new Vector2(-110, 32); break;
            }

            DrawChampTable();
            show(true);
        }

        

        //DontDestroyOnLoad(gameObject);
        //CreatChampTeams("RUS");
    }
    // Start is called before the first frame update
    void Start()
    {
        Startchamp.onClick.AddListener(StartChamp);
        toMenu.onClick.AddListener(ToMenu);
    }


    public void show(bool todo)
    {
        Debug.Log("CHAMP SHOW "+ todo);
        gameObject.SetActive(todo);
    }
    public void SetChamp(int todo)
    {

        //Debug.Log("SetChamp:"+todo);

        Saves.inst.CHAMP_IS_CHAMP= todo;

        if (todo == 0)
        {
            Saves.inst.CHAMP_ETAP =4;
            Saves.inst.CHAMP_CAHMPSCORE.SetValue("NNN&-&NNN&-", 0);
            Saves.inst.CHAMP_CAHMPSCORE.SetValue("NNN&-&NNN&-", 1);
            Saves.inst.CHAMP_CAHMPSCORE.SetValue("NNN&-&NNN&-", 2);
            Saves.inst.CHAMP_CAHMPSCORE.SetValue("NNN&-&NNN&-", 3);
            Saves.inst.CHAMP_CAHMPSCORE.SetValue("NNN&-&NNN&-", 4);
            Saves.inst.CHAMP_CAHMPSCORE.SetValue("NNN&-&NNN&-", 5);
            Saves.inst.CHAMP_CAHMPSCORE.SetValue("NNN&-&NNN&-", 6);

        }
    }
    public int GetChamp()
    {
        return Saves.inst.CHAMP_IS_CHAMP;//PlayerPrefs.GetInt("IS_CHAMP");
    }
    public void setPlayerTeam()
    {
        CreatChampTeams(Main.inst.TEAMNAME1);
    }
    public void ToMenu()
    {
        show(false);
    }
    public void StartChamp() //кнопка начать чемпионат
    {
        Main.inst.team1.GetComponent<Team>().AICONTROL = false;
        Main.inst.team2.GetComponent<Team>().AICONTROL = true;

        var tmplvl = Main.inst.ConvertRatingToLvl(Main.inst.GetTeamRating(Saves.inst.CHAMP_CUR_PLAYER_SOPER).ToString());
        Main.inst.SetTeam(Saves.inst.CHAMP_CUR_PLAYER_SOPER, Resources.Load<Sprite>("Flags/" + Saves.inst.CHAMP_CUR_PLAYER_SOPER), float.Parse(tmplvl), "team2");

        UiMainMenu.inst.StartGame();

        if (Saves.inst.CHAMP_IS_CHAMP == 0)
        {
            Saves.inst.CHAMP_ETAP = 4;
            Saves.inst.CHAMP_IS_CHAMP = 1;
        }

        show(false);
        Debug.Log("START CHAMP");
    }
    public void NextLevel()
    {
        Debug.Log("NextLevel");
        //ДЕЛАЕМ ОЧКИ

        string[] ETAP4_WINTEAMS = { "NNN", "NNN", "NNN", "NNN" };
        string[] ETAP2_WINTEAMS = { "NNN", "NNN" };

        for (int ii = 0; ii < Saves.inst.CHAMP_CAHMPSCORE.Length; ii++)
        {
            int impGame = ii + 1;

            string[] words = Saves.inst.CHAMP_CAHMPSCORE[ii].Split(new char[] { '&' });

            var tmpTeam1 = words[0];
           // var tmpScore1 = words[1];
            var tmpTeam2 = words[2];
          //  var tmpScore2 = words[3];

            if (Saves.inst.CHAMP_ETAP == 4)
            {

                if (impGame <= 4) //ЧЕТВЕРТЬ ФИНАЛ
                {

                    if (impGame == 1)//КОМАНДА ИГРОКА
                    {
                        Saves.inst.CHAMP_CAHMPSCORE[ii] = tmpTeam1 + "&" + Main.inst.SCORE1 + "&" + tmpTeam2 + "&" + Main.inst.SCORE2;
                        ETAP4_WINTEAMS[ii] = tmpTeam1;
                    }
                    if (impGame > 1)
                    {
                        int[] tmpSimResult = simulateGame(tmpTeam1, tmpTeam2);

                        if (tmpSimResult[0] > tmpSimResult[1])
                            ETAP4_WINTEAMS[ii] = tmpTeam1;
                        else
                            ETAP4_WINTEAMS[ii] = tmpTeam2;

                        Saves.inst.CHAMP_CAHMPSCORE[ii] = tmpTeam1 + "&" + tmpSimResult[0].ToString() + "&" + tmpTeam2 + "&" + tmpSimResult[1].ToString();
                    }
                    //string tmp2 = 
                    // "NNN&-&NNN&-",
                }
                else if (impGame >= 5 && impGame <= 6)
                {
                    //Debug.Log("ETAP == 4 impGame >= 5 && impGame <= 6");

                    if (impGame == 5)//КОМАНДА ИГРОКА
                    {
                        Saves.inst.CHAMP_CAHMPSCORE[ii] = ETAP4_WINTEAMS[0] + "&" + "-" + "&" + ETAP4_WINTEAMS[1] + "&" + "-";

                        //Назнаяаем соперника
                        Saves.inst.CHAMP_CUR_PLAYER_SOPER = ETAP4_WINTEAMS[1];
                    }
                    if (impGame > 5)
                    {
                        Saves.inst.CHAMP_CAHMPSCORE[ii] = ETAP4_WINTEAMS[2] + "&" + "-" + "&" + ETAP4_WINTEAMS[3] + "&" + "-";
                    }
                }
            }

            if (Saves.inst.CHAMP_ETAP == 2)
            {

                

                if (impGame >= 5 && impGame <= 6)
                {
                    //Debug.Log("ETAP == 2 impGame >= 5 && impGame <= 6");

                    if (impGame == 5)//КОМАНДА ИГРОКА
                    {
                        Saves.inst.CHAMP_CAHMPSCORE[ii] = tmpTeam1 + "&" + Main.inst.SCORE1 + "&" + tmpTeam2 + "&" + Main.inst.SCORE2;
                        ETAP2_WINTEAMS[0] = tmpTeam1;
                    }
                    if (impGame > 5)
                    {
                        int[] tmpSimResult = simulateGame(tmpTeam1, tmpTeam2);

                        if (tmpSimResult[0] > tmpSimResult[1])
                            ETAP2_WINTEAMS[1] = tmpTeam1;
                        else
                            ETAP2_WINTEAMS[1] = tmpTeam2;


                        Saves.inst.CHAMP_CAHMPSCORE[ii] = tmpTeam1 + "&" + tmpSimResult[0].ToString() + "&" + tmpTeam2 + "&" + tmpSimResult[1].ToString();
                    }
                }
                else if (impGame == 7)
                {
                    Saves.inst.CHAMP_CAHMPSCORE[ii] = ETAP2_WINTEAMS[0] + "&" + "-" + "&" + ETAP2_WINTEAMS[1] + "&" + "-";
                    //Назнаяаем соперника
                    Saves.inst.CHAMP_CUR_PLAYER_SOPER = ETAP2_WINTEAMS[1];
                }
            }

            if (Saves.inst.CHAMP_ETAP == 1)
            {
                if (impGame == 7)
                    Saves.inst.CHAMP_CAHMPSCORE[ii] = tmpTeam1 + "&" + Main.inst.SCORE1 + "&" + tmpTeam2 + "&" + Main.inst.SCORE2;

            }
        }
        //чел выиграл
        if (Saves.inst.CHAMP_ETAP == 4) Saves.inst.CHAMP_ETAP = 2;
        else if (Saves.inst.CHAMP_ETAP == 2) Saves.inst.CHAMP_ETAP = 1;
        else if (Saves.inst.CHAMP_ETAP == 1) Saves.inst.CHAMP_ETAP = 0;
        //РИСУЕМ ТАБЛИЦУ
        // DrawChampTable();
    }

    private int[] simulateGame(string tmpTeam1, string tmpTeam2)
    {
        //Debug.Log("SIMULATE = " + tmpTeam1 + "--" + tmpTeam2);

        //throw new NotImplementedException();
        float tmpLevel1 = Main.inst.IIHFRATING[tmpTeam1];
        float tmpLevel2 = Main.inst.IIHFRATING[tmpTeam2];
        int tmpScore1 = 0;
        int tmpScore2 = 0;

        //Debug.Log("SIMULATE = " + tmpLevel1 + "--" + tmpLevel2);

        //string teamToWin = "team1";

        float tmpPrc = 100;

        if (tmpLevel1 >= tmpLevel2)
        { tmpPrc = tmpLevel2 / tmpLevel1 * 100;
            //teamToWin = "team1";
        }
        else
        { tmpPrc = tmpLevel1 / tmpLevel2 * 100;
            //teamToWin = "team2";
        }

        //Debug.Log("SIMULATE = " + tmpPrc + "%");


        if (!v((int)tmpPrc))
        {
            tmpScore1 = UnityEngine.Random.Range(2, 6);
            tmpScore2 = tmpScore1 - UnityEngine.Random.Range(1, tmpScore1);
        }
        else
        {
            tmpScore2 = UnityEngine.Random.Range(2, 6);
            tmpScore1 = tmpScore2 - UnityEngine.Random.Range(1, tmpScore2);
        }
        
        int[] rt = { tmpScore1, tmpScore2 };

        //Debug.Log("SIMULATE = " + tmpScore1 + "--" + tmpScore2);

        return rt;
    }

    public bool v(int percent)
    {
        System.Random rand = new System.Random((int)DateTime.Now.Ticks);
        int res = rand.Next(101);
        if (res <= percent) return true; else return false;
    }

    public void CreatChampTeams(string playerTeam = "RUS") //ТОЛЬКО ОДИН РАЗ ДО НАЧЛА ЧАМПИОНТАА
    {
        //Debug.Log("CreatChampTeams1");
        Saves.inst.CHAMP_CUR_PLAYER_TEAM = playerTeam;

        //Создаем первую четверку коменад ЕСЛИ массив чемпионата пустой
        if (Saves.inst.CHAMP_CAHMPSCORE[0].Split(new char[] { '&' })[0]=="NNN")
        {
            //Debug.Log("CreatChampTeams2");
            reshuffle(Main.inst.FLAGSCORE);

            champTeams.Add(playerTeam);
            int tmpiiiiiii = 0;
            for (int I = 0; I < Main.inst.FLAGSCORE.Length; I++)
            {

                string[] words = Main.inst.FLAGSCORE[I].Split(new char[] { ',' });
                //var tmpDif = words[0];
                var tmpName = words[1];
                if (tmpName != playerTeam)
                {
                    champTeams.Add(tmpName);
                    tmpiiiiiii++;
                }
                if (tmpiiiiiii > 6)
                    break;

                if (tmpiiiiiii==1)
                {
                    Saves.inst.CHAMP_CUR_PLAYER_SOPER = tmpName;
                }

               // Debug.Log(tmpName);
            }

            int tmpiiii = 0;
            for (int I = 0; I < Saves.inst.CHAMP_ETAP; I++)
            {

                Saves.inst.CHAMP_CAHMPSCORE[I] = champTeams[tmpiiii] + "&-&" + champTeams[tmpiiii+1] + "&-";
               // Debug.Log(Saves.inst.CHAMP_CAHMPSCORE[I]);
                tmpiiii += 2;
            }


        }//ЕСЛИ НЕ ПУСТОЦ ТО НАДО ЗАМЕНИТЬ КОМАНДУ НА НАОВУЮ 
        else if(Saves.inst.CHAMP_CAHMPSCORE[0].Split(new char[] { '&' })[0]!= playerTeam) 
        {

            Saves.inst.CHAMP_CAHMPSCORE[0] = playerTeam + "&-&" + Saves.inst.CHAMP_CAHMPSCORE[0].Split(new char[] { '&' })[2] + "&-";
        }

        DrawChampTable();
    }
    public void DrawChampTable()
    {
        //Debug.Log("DRAWTABLE");

        for (int ii=0; ii< Saves.inst.CHAMP_CAHMPSCORE.Length;ii++)
        {
            string[] words = Saves.inst.CHAMP_CAHMPSCORE[ii].Split(new char[] { '&' });

            var tmpTeam1 = words[0];
            var tmpScore1 = words[1];
            var tmpTeam2 = words[2];
            var tmpScore2 = words[3];
            var tmpLevel1 = "";
            var tmpLevel2 = "";

            if (tmpTeam1 != "NNN") 
                tmpLevel1 = "LVL " + Main.inst.ConvertRatingToLvl(Main.inst.GetTeamRating(tmpTeam1).ToString());
            if (tmpTeam2 != "NNN")
                tmpLevel2 = "LVL " + Main.inst.ConvertRatingToLvl(Main.inst.GetTeamRating(tmpTeam2).ToString());

            GameObject tmpObj1 = null;
            GameObject tmpObj2 = null;

            var tmpPrefName1 = (ii + 1).ToString() + (1).ToString();
            var tmpPrefName2 = (ii + 1).ToString() + (2).ToString();

            tmpObj1 = GameObject.Find(tmpPrefName1);
            tmpObj2 = GameObject.Find(tmpPrefName2);

            //tmpObj1.GetComponent<Flag>().ShowScore(false);
            //tmpObj2.GetComponent<Flag>().ShowScore(false);

            Transform tmpFlagImg1 = tmpObj1.transform.Find("Flag");
            Transform tmpFlagText1 = tmpObj1.transform.Find("Dif");

            Transform tmpFlagImg2 = tmpObj2.transform.Find("Flag");
            Transform tmpFlagText2 = tmpObj2.transform.Find("Dif");

            tmpFlagImg1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Flags/" + tmpTeam1);
            tmpFlagText1.GetComponent<Text>().text = tmpLevel1;
            tmpFlagImg2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Flags/" + tmpTeam2);
            tmpFlagText2.GetComponent<Text>().text = tmpLevel2;

            tmpObj1.GetComponent<Flag>().SetScore(tmpScore1, tmpPrefName1);
            tmpObj2.GetComponent<Flag>().SetScore(tmpScore2, tmpPrefName2);
        }
    }

    //for shuffle number from array
    void reshuffle(string[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++)
        {
            string tmp = texts[t];
            int r = UnityEngine.Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }

}
