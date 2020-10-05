using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.Advertisements; 


//TODO: Опять не правльно рассчитваются случайные игры
//TODO: Прорисовка чепионата посл есмены коеманды после уже случившегос ячемпионата, вроде так
//TODO: Звук ОХ при щтанге 
//TODO: Добавить авторы
//TODO: Онлайн?
//TODO: Ачифки !!
/*
 * 3 гола подряд
 * Выиграл за 1 секунду до конца игры
 * Выиграл чемпионат
 * Отбил супер удар
 * 100 заброшенных шайб
 * 100 игр сыграно (завершены)
 * 
*/
//TODO: статситика 
/*
    Всего игр
    Побед
    Всего врменни в игре
  */

public class Main : MonoBehaviour {

    public static Main inst;

    public GameObject team1;
    public GameObject team2;
    public GameObject PlayerPrefeb;
    public GameObject BonusPrefeb;
    public GameObject FlagsPrefeb;
    public GameObject HubPrefeb;  

    public float teamspeed = 0.1f; 

    public Vector2[] playersPoses; //заданы в редакторе
    public List<GameObject> list = new List<GameObject>(); //все игроки

    private Color clr1 = new Color(0f, 0.35f, 1f, 1f);
    private Color clr2 = new Color(1f, 0f, 0f, 1f);

    [System.NonSerialized]
    public int SETTINGS_MINUTES = 1;   //заданы в редакторе
    public int SETTINGS_SECONDS = 30; //заданы в редакторе

    // GAME
    public int MINUTES;
    public int SECONDS;
    public int  PERIOD;
    public int SCORE1;
    public int SCORE2;
    public int PP1;
    public int PP2;
    public string TEAMNAME1;
    public string TEAMNAME2;

    public float TIMEONATTACK1;
    public float TIMEONATTACK2;
    public int HITSANDPASSES1;
    public int HITSANDPASSES2;
    public int PENALTIES1;
    public int PENALTIES2;
    public int PPGOLASB1;
    public int PPGOLASB2;
    public int PPGOLASM1;
    public int PPGOLASM2;

    public Dictionary<string, float> IIHFRATING = new Dictionary<string, float>();

    public string[] FLAGSCORE =
    {
"3990,CAN",
"3945,SWE",
"3930,RUS",
"3765,USA",
"3765,FIN",
"3740,CZE",
"3590,CHE",
"3575,DEU",
"3270,NOR",
"3245,SVK",
"3225,LVA",
"3130,DNK",
"3115,FRA",
"3085,BLR",
"2915,SVN",
"2835,KOR",
"2820,AUT",
"2760,KAZ",
"2725,ITA",
"2595,HUN",
"2565,POL",
"2465,GBR",
"2410,JPN",
"2340,UKR",
"2220,LTU",
"2155,EST",
"2115,HRV",
"2060,NLD",
"2010,ROU",
"1830,SRB",
"1700,ESP",
"1660,ISL",
"1610,CHN",
"1450,ISR",
"1440,MEX",
"1280,AUS",
"1250,BEL",
"1210,BGR",
"1030,NZL",
"990,GEO",
"925,PRK",
"790,LUX",
"770,TUR",
"655,ZAF",
"600,HKG",
"425,TWN",
"365,ARE",
"360,BIH",
"200,TKM",
"140,KWT",
    };

    private GameObject PP1player;
    private GameObject PP2player;

    private GameObject PP1REALplayer;
    private GameObject PP2REALplayer;

    public GameObject LASTPLAYER = null;
    public GameObject LASTBONUS = null;
    public GameObject CURRENTBONUS = null;

    public GameObject tmpLASTPLAYERt1 = null;
    public GameObject tmpLASTPLAYERt2 = null;

    public bool Team1UpBtn = false;
    public bool Team1DownBtn = false;
    public bool Team2UpBtn = false;
    public bool Team2DownBtn = false;

    //AI
    private GameObject closestPL;
    private float deltaAI1=0;
    private float deltaAI2=0;

    //BONUS
    private bool BONUSACTIVE = false;

    public enum gameStates { menu,begin,game,stop,pause,end };
    public gameStates GAME_STATE = gameStates.begin;

    public enum gamePeriods { P1, P2, P3, OT };
    public gamePeriods GAME_PERIOD = gamePeriods.P1;

    public GameObject SUD;
    // Use this for initialization

    void Awake()
    {
        if (inst == null) { inst = this; }
        else if (inst != null) { Destroy(gameObject); }

        Saves.inst.LoadOptions();

        GAME_STATE = gameStates.menu;
        GAME_PERIOD = gamePeriods.P1;

        Admob.inst.loadBannerAd();// ADS load
    }
    void OnApplicationQuit()
    {
        //UiChamp.inst.SetChamp(0);
    }


    void Start()
    {
        UiMainMenu.inst.CreateFlags();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Android close icon or back button tapped.
            Application.Quit();
        }

        if (GAME_STATE == gameStates.game)
        {
            Control();
            CreateBonus();
        }
    }

    public void EventController(string EventName,string EventVar="default")
    {
        switch (EventName)
        {
            case "goal":
                                    Team1UpBtn = false;
                                    Team1DownBtn = false;
                                    Team2UpBtn = false;
                                    Team2DownBtn = false;
                                    SoundManager.Instance.Play(SoundManager.Instance.goal);
                                    break;//++
            case "shot":            SoundManager.Instance.Play(SoundManager.Instance.shot); break;
            case "shtanga":         SoundManager.Instance.Play(SoundManager.Instance.shtanga); break;//++
            case "bonusSuperCatch": SoundManager.Instance.Play(SoundManager.Instance.bonusSuperCatch); break;//++
            case "bonusFallCatch":  SoundManager.Instance.Play(SoundManager.Instance.svistok); break;//++
            case "hubBort":         SoundManager.Instance.Play(SoundManager.Instance.hubBort); break;//++
            case "restartGame":
                                    UiEndGamePanel.inst.show(false);
                                    UiMainMenu.inst.show(true);
                                    SoundManager.Instance.StopMusic(SoundManager.Instance.tolpa);
                                    Saves.inst.ADS_COUNTER++; Admob.inst.showInterstitialAd();
                break;//++
            case "endGame":         SoundManager.Instance.Play(SoundManager.Instance.endGame);
                                    SoundManager.Instance.StopMusic(SoundManager.Instance.tolpa);
                break;//++
            case "startgGame":      SoundManager.Instance.PlayMusic(SoundManager.Instance.tolpa);
                Saves.inst.SaveTeams(team1.GetComponent<Team>().NAME, team2.GetComponent<Team>().NAME);
                break;
            case "bonusFallDver":   SoundManager.Instance.Play(SoundManager.Instance.bonusFallDver); break;//++
            case "buttonClick":     SoundManager.Instance.Play(SoundManager.Instance.shot); break; //++
            case "winChamp":        SoundManager.Instance.Play(SoundManager.Instance.newlevel); break; //++
        }
    }

  
    public void ReatartGame(string TYPE="stop")
    {
        GAME_STATE = gameStates.stop;

        Debug.Log("restartGame:"+ TYPE);

        if (TYPE== "stop" && Saves.inst.CHAMP_IS_CHAMP == 0 ) //если чемпионат и прерывание то обнуляем
        {
            UiChamp.inst.SetChamp(0);
        }

        if (TYPE == "end" && Saves.inst.CHAMP_IS_CHAMP == 1) 
        {

            if (SCORE1 > SCORE2)
            {
                UiChamp.inst.NextLevel();
            }
            else
            {
                UiChamp.inst.SetChamp(0);
                //TODO: прогиграл
            }
        }

        EventController("restartGame");
        UiMainMenu.inst = null;
        UiInGamePanel.inst = null;
        UiEndGamePanel.inst = null;
        UiChamp.inst = null;
        UiPlashka.inst = null;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
       /* */
    }


    public void StartGame()
    {

        if(PlayerPrefs.GetInt("SHOW_HELP", 1)==1)
        {
            Saves.inst.PLAY_AFTER_HELP = true;
            UiHelp.inst.ShowMe();
        }
        else
        {
            RealStartGame();
        }

    }

    public void RealStartGame()
    {
        Saves.inst.PLAY_AFTER_HELP = false;
        PlayerPrefs.SetInt("SHOW_HELP", 0);
        SUD = GameObject.Find("sud");
        CreatePlayers();
        SetGameSettings();
        UiInGamePanel.inst.runCountDown();
        ShowSudAnimation();
        StartCoroutine(restartAfterGoal());
        EventController("startgGame");
    }


        private void SetGameSettings()
    {
        MINUTES = SETTINGS_SECONDS / 60;
        SECONDS = SETTINGS_SECONDS % 60;
        PERIOD = 1;
        SCORE1 = 0;
        SCORE2 = 0;
        PP1 = 0;
        PP2 = 0;

        TIMEONATTACK1 = 0;
        TIMEONATTACK2 = 0;
        HITSANDPASSES1 = 0;
        HITSANDPASSES2 = 0;
        PENALTIES1 = 0;
        PENALTIES2 = 0;
        PPGOLASB1 = 0;
        PPGOLASB2 = 0;
        PPGOLASM1 = 0;
        PPGOLASM2 = 0;

        UiInGamePanel.inst.SetPanel();
        UiEndGamePanel.inst.SetPanel();
        //сложность - скорость
        //время
        //кто управляет
    }
    public void SetTeam(string NAME, Sprite FLAG, float DIF, string TEAM)
    {
        //Debug.Log("setteam " + NAME + TEAM);

        if (TEAM == "team1")
        {
            UiMainMenu.inst.TeamName1.text = NAME;
            UiMainMenu.inst.TeamLevel1.text = "LVL "+ DIF.ToString();
            team1.GetComponent<Team>().NAME = NAME;
            team1.GetComponent<Team>().TYPE = TEAM;
            team1.GetComponent<Team>().COLOR = clr1;
            team1.GetComponent<Team>().AICONTROL = false;
            team1.GetComponent<Team>().FORMA = FLAG;
            team1.GetComponent<Team>().DIF = DIF;
            team1.GetComponent<Team>().SPEED = teamspeed;
            UiMainMenu.inst.Forma1.GetComponent<Image>().sprite = FLAG;
            UiInGamePanel.inst.flagTeam1.GetComponent<Image>().sprite = FLAG;
            UiEndGamePanel.inst.Team1Flag.GetComponent<Image>().sprite = FLAG;
            UiMainMenu.inst.ChampFlag.sprite = FLAG;
            UiPlashka.inst.ChampFlag.sprite = FLAG;
            TEAMNAME1 = NAME;
        }
        else
        {
            UiMainMenu.inst.TeamName2.text = NAME;
            UiMainMenu.inst.TeamLevel2.text = "LVL " + DIF.ToString();
            team2.GetComponent<Team>().NAME = NAME;
            team2.GetComponent<Team>().TYPE = TEAM;
            team2.GetComponent<Team>().COLOR = clr2;
            team2.GetComponent<Team>().AICONTROL = true;
            team2.GetComponent<Team>().FORMA = FLAG;
            team2.GetComponent<Team>().DIF = DIF;
            team2.GetComponent<Team>().SPEED = teamspeed;
            UiMainMenu.inst.Forma2.GetComponent<Image>().sprite = FLAG;
            UiInGamePanel.inst.flagTeam2.GetComponent<Image>().sprite = FLAG;
            UiEndGamePanel.inst.Team2Flag.GetComponent<Image>().sprite = FLAG;
            TEAMNAME2 = NAME;
        }
    }

    public void PauseGame()
    {
       // Debug.Log("^^^" + Time.timeScale);

        if (GAME_STATE == gameStates.game)
        {
            // Debug.Log("__"+Time.timeScale);
            GAME_STATE = gameStates.pause;
            Time.timeScale = 0;
            UiEndGamePanel.inst.ControlPanel("pause");

        }
        else if (GAME_STATE == gameStates.pause)
        {
            // Debug.Log("&&" + Time.timeScale);
            GAME_STATE = gameStates.game;
            Time.timeScale = 1;
            //UiEndGamePanel.inst.ControlPanel("continue");
        }

        Team1UpBtn = false;
        Team1DownBtn = false;
        Team2UpBtn = false;
        Team2DownBtn = false;

    }
    public void Goal(string goalteam)
    {
        GAME_STATE = gameStates.stop;
        ShowSudAnimation();

        EventController("goal");

        if (goalteam == "vorota1goal")
        {
            SCORE2++;
            if(CURRENTBONUS!=null)
            {
                if(CURRENTBONUS.GetComponent<Bonus>().bonusType == "fol")
                {
                    if (CURRENTBONUS.GetComponent<Bonus>().bonusTeam == "team1")
                    {
                        PPGOLASB2++;
                        PPGOLASM1++;
                    }
                    if (CURRENTBONUS.GetComponent<Bonus>().bonusTeam == "team2")
                    {
                        PPGOLASM2++;
                        PPGOLASB1++;
                    }
                }
            }
        }
        else if(goalteam == "vorota2goal")
        {
            SCORE1++;

            if (CURRENTBONUS != null)
            {
                if (CURRENTBONUS.GetComponent<Bonus>().bonusType == "fol")
                {
                    if (CURRENTBONUS.GetComponent<Bonus>().bonusTeam == "team1")
                    {
                        PPGOLASM2++;
                        PPGOLASB1++;
                    }
                    if (CURRENTBONUS.GetComponent<Bonus>().bonusTeam == "team2")
                    {
                        PPGOLASB2++;
                        PPGOLASM1++;
                    }
                }
            }
        }
        UiInGamePanel.inst.runOtherText("GOAL!!!");
       // UiInGamePanel.inst.runOtherText("OVERTIME", "blue", "overtime");
        StartCoroutine(restartAfterGoal());
    }

    public IEnumerator startGameDelay()
    {
        yield return new WaitForSeconds(1F);
        GAME_STATE = gameStates.game;
        Hub.inst.PushHub();
    }
    public IEnumerator restartAfterGoal()
    {
        yield return new WaitForSeconds(2F);
        Hub.inst.StopHub();
        team1.transform.position = new Vector3(-1.3F, 0.2F, 0);
        team2.transform.position = new Vector3(-1.3F, 0.2F, 0);
        StartCoroutine(restartAfterGoal2());
    }
    public IEnumerator restartAfterGoal2()
    {
        yield return new WaitForSeconds(1F);
        GAME_STATE = gameStates.game;
        Hub.inst.PushHub();
    }
    public IEnumerator loadingDelay()
    {
        yield return new WaitForSeconds(1F);
        UiPlashka.inst.show(false);
    }


    public void StopGame()//endgame
    {
        GAME_STATE = gameStates.end;
        Hub.inst.StopHub();

       // Advertisement.Show();

        UiEndGamePanel.inst.ControlPanel("end");
        EventController("endGame");
    }

    private void CreateBonus()
    {
        if (SECONDS % 15 == 0 && BONUSACTIVE==false)
        {
            int tmp = (int)Mathf.Round(Random.Range(0F, 2F));
            float tmpF = Random.Range(-3.2F, 3.2F);

            Vector3 pos = new Vector3(-2.8F, 0, 0);

            switch (tmp)
            {
               // case 0: pos = new Vector3(0, 0, 0); break;
                case 0: pos = new Vector3(-2.8F, tmpF, 0); break;
                case 1: pos = new Vector3(2.8F, tmpF, 0); break;
            }

            Instantiate(BonusPrefeb, pos, Quaternion.identity);
            BONUSACTIVE = true;
        }

       // TODO: Условия на поялвение бонса
    }
    public void StartBonus(GameObject bonusGO)
    {
        Debug.Log("StartBonus");
        if (bonusGO.GetComponent<Bonus>().bonusType == "fol")
        {
            if (LASTPLAYER != null)
            {
                LASTPLAYER.SetActive(false);
                if (LASTPLAYER.gameObject.tag == "team1")
                {
                    PP1 = 10;
                    UiInGamePanel.inst.SetPP("team1", PP1);
                    PP1player.gameObject.GetComponent<Player>().SetPenalty(true, true);
                    PP1REALplayer = LASTPLAYER;
                    bonusGO.GetComponent<Bonus>().bonusTeam = "team1";
                    Debug.Log("StartBonus1");
                    PENALTIES1++;//STATA
                }
                if (LASTPLAYER.gameObject.tag == "team2")
                {
                    PP2 = 10;
                    UiInGamePanel.inst.SetPP("team2", PP2);
                    PP2player.gameObject.GetComponent<Player>().SetPenalty(true, true);
                    PP2REALplayer = LASTPLAYER;
                    bonusGO.GetComponent<Bonus>().bonusTeam = "team2";
                    Debug.Log("StartBonus2");
                    PENALTIES2++;//STATA
                }
                ShowSudAnimation();
                EventController("bonusFallDver");
                //BONUSACTIVE = true;
            }
            //TODO: Удалить последнего игрока
        }
        else 
        if (bonusGO.GetComponent<Bonus>().bonusType == "sup")
        {
            if (LASTPLAYER != null)
            {
                if (LASTPLAYER.gameObject.GetComponent<Player>().tag == "team1")
                    list[0].gameObject.GetComponent<Player>().SetSuper(true);
                else
                    list[6].gameObject.GetComponent<Player>().SetSuper(true);

                EventController("bonusSuperCatch");
                //TODO: временно
                //BONUSACTIVE = true;
            }
            //TODO: Дат ьсупер способность случанйому игроку
        }
    }
    public void StopBonus(string type, string team, GameObject GO = null)
    {
        Debug.Log("StopBonus");
        if (type == "fol")
        {
                if (team == "team1")
                {
                    //if(PP1REALplayer!=null)
                    //    PP1REALplayer.SetActive(true);
                    PP1REALplayer.gameObject.GetComponent<Player>().SetPenalty(false, true);
                    PP1player.gameObject.GetComponent<Player>().SetPenalty(false, false);
                    
                }
                if (team == "team2")
                {
                    //if (PP2REALplayer != null)
                    //    PP2REALplayer.SetActive(true);
                    PP2REALplayer.gameObject.GetComponent<Player>().SetPenalty(false, true);
                    PP2player.gameObject.GetComponent<Player>().SetPenalty(false, false);
                }
                EventController("bonusFallDver");
        }
        else if (type == "sup")
        {
            GO.GetComponent<Player>().SetSuper(false);
            Hub.inst.SuperHub(team);
            //Шайба летит в сторону ворот из класса щайба
        }

        BONUSACTIVE = false;
        Destroy(CURRENTBONUS);
        CURRENTBONUS = null;
        //LASTBONUS = null;
    }
    public void ShowSudAnimation()
    {
        SUD.GetComponent<Animator>().Play("Sud");
    }

    private void Control()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {

                // PLAYER1
                if (touch.position.x < Screen.width / 2 && touch.position.y > Screen.height / 2)
                { Team1UpBtn = true; }// Debug.Log("LEFTTOP");
               // else
               // { Team1UpBtn = false;}

                if (touch.position.x < Screen.width / 2 && touch.position.y < Screen.height / 2)
                { Team1DownBtn = true; }// Debug.Log("LEFTBOTTOM");
               // else
               // {Team1DownBtn = false;}

               
                //PLAYER2
                if (touch.position.x > Screen.width / 2 && touch.position.y < Screen.height / 2)
                {
                    if (team2.GetComponent<Team>().AICONTROL == true) //если игрок 2 AI
                    {   Team1UpBtn = true; }    //Debug.Log("LEFTTOP");
                    else                                            //если игрок 2 НЕ AI
                    {   Team2DownBtn = true; } // Debug.Log("RIGHTBOTTOM");
                }
               /* else
                {
                    if (team2.GetComponent<Team>().AICONTROL == true)
                    {Team1UpBtn = false; } // Debug.Log("LEFTTOP");
                    else
                    Team2DownBtn = false;  // Debug.Log("RIGHTBOTTOM");
                }*/

                if (touch.position.x > Screen.width / 2 && touch.position.y > Screen.height / 2)
                {Team2UpBtn = true; }// Debug.Log("RIGHTTOP");
               // else
               // {Team2UpBtn = false;}

            }
            else
            {
                
                if (touch.position.x < Screen.width / 2)
                { Team1UpBtn = false; Team1DownBtn = false; } // TODO: добавить если игрок только один
                else
                {
                    Team2DownBtn = false;
                    Team2UpBtn = false;

                    if (team2.GetComponent<Team>().AICONTROL == true) //если игрок 2 AI
                    { Team1UpBtn = false; }    //Debug.Log("LEFTTOP");
                }
                
            }
        }
        

        //Debug.Log(Time.deltaTime);

        deltaAI1 += 0.01F; //Time.deltaTime;
        
        //TEAM1
        if (team1.GetComponent<Team>().AICONTROL== false)
        {
            team1.GetComponent<Team>().MOVE = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W) || Team1UpBtn)
                team1.GetComponent<Team>().MOVE = new Vector3(0, 1, 0);
            if (Input.GetKey(KeyCode.S) || Team1DownBtn)
                team1.GetComponent<Team>().MOVE = new Vector3(0, -1, 0);
        }
        else if(deltaAI1>0.1) //0.5
        {
            //Debug.Log(deltaAI1);

            deltaAI1 = 0; //TODO: AI - ЗАДЕРЖКА
            closestPL = team1.GetComponent<Team>().PLAYERS[1];

            foreach (GameObject PL in team1.GetComponent<Team>().PLAYERS)
            {
                //PL.GetComponent<Player>().SetSuper(false);
                if (Vector3.Distance(PL.gameObject.transform.position, HubPrefeb.transform.position) <
                    Vector3.Distance(closestPL.gameObject.transform.position, HubPrefeb.transform.position))
                    {
                        closestPL = PL;
                    }
            }
            //ДВИГАЕМ AI
            if (closestPL.gameObject.transform.position.y  > HubPrefeb.transform.position.y - 0.1) // TODO: подумать про вариант когда шайба на одном уровне и он стоит ровно
                team1.GetComponent<Team>().MOVE = new Vector3(0, -1, 0);
            else if(closestPL.gameObject.transform.position.y  <= HubPrefeb.transform.position.y + 0.1) //
                team1.GetComponent<Team>().MOVE = new Vector3(0, 1, 0);
            else
                team1.GetComponent<Team>().MOVE = new Vector3(0, 0, 0);
        }
        float tmpDifftoSpeedConverrter1 = ((-0.056F - 0.03F * (team1.GetComponent<Team>().DIF / 100)) / -0.086F)* team1.GetComponent<Team>().SPEED;//  ;

        if (team1.transform.position.y + team1.GetComponent<Team>().MOVE.y * teamspeed >= -3.88 
            && team1.transform.position.y + team1.GetComponent<Team>().MOVE.y * teamspeed <= 3.88)
            team1.transform.position += team1.GetComponent<Team>().MOVE * tmpDifftoSpeedConverrter1; //*Time.deltaTime;

       // tmpTime+=0.01F;
        deltaAI2 += 0.01F;//Time.deltaTime;

        //TEAM2
        if (team2.GetComponent<Team>().AICONTROL==false)
        {
            team2.GetComponent<Team>().MOVE = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.UpArrow) || Team2UpBtn)
                team2.GetComponent<Team>().MOVE = new Vector3(0, 1, 0);
            if (Input.GetKey(KeyCode.DownArrow) || Team2DownBtn)
                team2.GetComponent<Team>().MOVE = new Vector3(0, -1, 0);
        }
        else if (deltaAI2 > 0.1) //0.5  0.1-0.2 возможные колебания
        {
            //Debug.Log(deltaAI2);

            deltaAI2 = 0; //TODO: AI - ЗАДЕРЖКА
            closestPL = team2.GetComponent<Team>().PLAYERS[1];

            foreach (GameObject PL in team2.GetComponent<Team>().PLAYERS)
            {
                //PL.GetComponent<Player>().SetSuper(false);
                if (Vector3.Distance(PL.gameObject.transform.position, HubPrefeb.transform.position) <
                    Vector3.Distance(closestPL.gameObject.transform.position, HubPrefeb.transform.position))
                {
                    closestPL = PL;
                }
            }
            //ДВИГАЕМ AI
            if (closestPL.gameObject.transform.position.y > HubPrefeb.transform.position.y - 0.1) //
                team2.GetComponent<Team>().MOVE = new Vector3(0, -1, 0);
            else if (closestPL.gameObject.transform.position.y <= HubPrefeb.transform.position.y + 0.1) //
                team2.GetComponent<Team>().MOVE = new Vector3(0, 1, 0);
            else
            team1.GetComponent<Team>().MOVE = new Vector3(0, 0, 0);
        }

        float tmpDifftoSpeedConverrter2 = ((-0.056F - 0.03F * (team2.GetComponent<Team>().DIF / 100)) / -0.086F)* team2.GetComponent<Team>().SPEED;
        //(team2.GetComponent<Team>().SPEED * team2.GetComponent<Team>().DIF / 100); // 0.03x−0.86y+0.056=0  x=(-0.056+0.86y)/0.03 y=(-0.056-0.03x)/(-0.086)

        if (team2.transform.position.y + team2.GetComponent<Team>().MOVE.y * teamspeed >= -3.88
        && team2.transform.position.y + team2.GetComponent<Team>().MOVE.y * teamspeed <= 3.88)
            team2.transform.position += team2.GetComponent<Team>().MOVE * tmpDifftoSpeedConverrter2; //*Time.deltaTime; 

    }
    public string ConvertRatingToLvl(string rating)
    {
        float TOPDIFF = 3990; //TODO: решить с макимальным
        float tmpDIF100 = Mathf.Round(float.Parse(rating) / TOPDIFF * 100);
        //Debug.Log(tmpDIF100);
        return tmpDIF100.ToString();
    }
    public float GetTeamRating(string teamName)
    {
        return IIHFRATING[teamName];
    }
    private void CreatePlayers()
    {
        for (int i = 0; i < playersPoses.Length; i++)
        {
           // Debug.Log(playersPoses[i]);

            GameObject tmpPlayer;

            //TEAM2
            if (i < 6)
            {
                tmpPlayer = Instantiate(PlayerPrefeb, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                tmpPlayer.transform.parent = team2.transform;
                tmpPlayer.transform.rotation = Quaternion.Euler(0, 0, 90);
                tmpPlayer.tag = "team2";
                //tmpPlayer.transform.Find("forma").GetComponent<SpriteRenderer>().color = team2.GetComponent<Team>().COLOR; // Set to opaque black
                tmpPlayer.transform.GetComponent<Player>().FORMA.GetComponent<SpriteRenderer>().sprite = team2.GetComponent<Team>().FORMA;
                tmpPlayer.transform.GetComponent<Player>().HEAD.GetComponent<SpriteRenderer>().sprite = team2.GetComponent<Team>().FORMA;
                if (i == 5) tmpPlayer.transform.GetComponent<Player>().POSITION = "GK";
                else if(i == 0 || i == 1) tmpPlayer.transform.GetComponent<Player>().POSITION = "AT";
                else tmpPlayer.transform.GetComponent<Player>().POSITION = "DF";
                team2.GetComponent<Team>().PLAYERS.Add(tmpPlayer);
            }//TEAM1
            else if(i >= 6 && i < 12 )
            {
                tmpPlayer = Instantiate(PlayerPrefeb, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                tmpPlayer.transform.parent = team1.transform;
                tmpPlayer.transform.rotation = Quaternion.Euler(0, 0, -90);
                tmpPlayer.tag = "team1";
                //tmpPlayer.transform.Find("forma").GetComponent<SpriteRenderer>().color = team1.GetComponent<Team>().COLOR; // Set to opaque black
                tmpPlayer.transform.GetComponent<Player>().FORMA.GetComponent<SpriteRenderer>().sprite = team1.GetComponent<Team>().FORMA;
                tmpPlayer.transform.GetComponent<Player>().HEAD.GetComponent<SpriteRenderer>().sprite = team1.GetComponent<Team>().FORMA;
                if (i == 11) tmpPlayer.transform.GetComponent<Player>().POSITION = "GK";
                else if (i == 6 || i == 7) tmpPlayer.transform.GetComponent<Player>().POSITION = "AT";
                else tmpPlayer.transform.GetComponent<Player>().POSITION = "DF";
                team1.GetComponent<Team>().PLAYERS.Add(tmpPlayer);
            }
            else if(i == 12) //team2 PP 2.3 -4.5
            {
                tmpPlayer = Instantiate(PlayerPrefeb, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                tmpPlayer.tag = "team2";
                //tmpPlayer.transform.Find("forma").GetComponent<SpriteRenderer>().color = team2.GetComponent<Team>().COLOR; // Set to opaque black
                tmpPlayer.transform.GetComponent<Player>().FORMA.GetComponent<SpriteRenderer>().sprite = team2.GetComponent<Team>().FORMA;
                tmpPlayer.transform.GetComponent<Player>().HEAD.GetComponent<SpriteRenderer>().sprite = team2.GetComponent<Team>().FORMA;
                tmpPlayer.transform.GetComponent<Player>().POSITION = "PP";
                PP2player = tmpPlayer;
                tmpPlayer.transform.GetComponent<Player>().FORMA.gameObject.GetComponentInParent<SpriteMask>().frontSortingLayerID = SortingLayer.NameToID("Bonus");
                tmpPlayer.transform.GetComponent<Player>().FORMA.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Bonus";
                tmpPlayer.transform.GetComponent<Player>().FORMA.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 90;
                tmpPlayer.SetActive(false);

                SpriteRenderer[] allChildren2 = tmpPlayer.transform.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer child in allChildren2)
                {
                    child.sortingLayerName = "Bonus";
                    // do whatever with child transform here
                }
            }
            else //team1 PP -2.3 -4.5
            {
                tmpPlayer = Instantiate(PlayerPrefeb, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                tmpPlayer.tag = "team1";
                //tmpPlayer.transform.Find("forma").GetComponent<SpriteRenderer>().color = team1.GetComponent<Team>().COLOR; // Set to opaque black
                tmpPlayer.transform.GetComponent<Player>().FORMA.GetComponent<SpriteRenderer>().sprite = team1.GetComponent<Team>().FORMA;
                tmpPlayer.transform.GetComponent<Player>().HEAD.GetComponent<SpriteRenderer>().sprite = team1.GetComponent<Team>().FORMA;
                tmpPlayer.transform.GetComponent<Player>().POSITION = "PP";
                PP1player = tmpPlayer;
                tmpPlayer.transform.GetComponent<Player>().FORMA.gameObject.GetComponentInParent<SpriteMask>().frontSortingLayerID = SortingLayer.NameToID("Bonus");
                tmpPlayer.transform.GetComponent<Player>().FORMA.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Bonus";
                tmpPlayer.transform.GetComponent<Player>().FORMA.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 90;
                tmpPlayer.SetActive(false);

                SpriteRenderer[] allChildren1 = tmpPlayer.transform.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer child in allChildren1)
                {
                    child.sortingLayerName = "Bonus";
                    // do whatever with child transform here
                }
            }
            tmpPlayer.transform.GetComponent<Player>().NUMBER = i;
            tmpPlayer.transform.localPosition = playersPoses[i];
            list.Add(tmpPlayer);
        }
    }
}


public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rnd = new System.Random();
        for (var i = 0; i < list.Count; i++)
            list.Swap(i, rnd.Next(i, list.Count));
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}