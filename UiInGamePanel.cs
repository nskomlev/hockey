using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UiInGamePanel : MonoBehaviour {

    public static UiInGamePanel inst;

    public Text scoreTeam1;
    public Text scoreTeam2;
    public Text nameTeam1;
    public Text nameTeam2;
    public Text time;
    public Text period;


    public GameObject pp1;
    public GameObject pp2;

    public GameObject flagTeam1;
    public GameObject flagTeam2;

    public Text pp1time;
    public Text pp2time;

    public Button Pause;
    public Button Mute;

    public GameObject CountDown;
    public GameObject OtherText;
    //TIMER
    public int minutesTimer = 0;
    public int secondsTimer = 0;

    public int secondsPP1 = 0;
    public int secondsPP2 = 0;

    //private DateTime timer = new DateTime();
    private DateTime timerNEW;
    private DateTime timer1 = new DateTime();
    private DateTime timer2 = new DateTime();

    private bool triggerOvertime = false;
    // Use this for initialization

    void Awake()
    {
        if (inst == null)        {            inst = this;        }
        else if (inst != null)        {            Destroy(gameObject);        }
        DefaultPanel();
        MuteF(false);
    }

    void DefaultPanel()
    {   
        pp1.SetActive(false);
        pp2.SetActive(false);
    }
    public void SetPanel()
    {
        minutesTimer = Main.inst.SECONDS / 60;
        secondsTimer = Main.inst.SECONDS % 60;

        nameTeam1.text = Main.inst.TEAMNAME1;
        nameTeam2.text = Main.inst.TEAMNAME2;
        scoreTeam1.text = Main.inst.SCORE1.ToString();
        scoreTeam2.text = Main.inst.SCORE2.ToString();
        time.text = minutesTimer.ToString("00") +":" + secondsTimer.ToString("00");
        period.text = Main.inst.PERIOD +"ST";
        pp1time.text = "PP1 "+ Main.inst.PP1.ToString("00");
        pp2time.text = "PP2 "+ Main.inst.PP2.ToString("00");
        Pause.onClick.AddListener(PauseF);
        Mute.onClick.AddListener(()=>MuteF());
        timerNEW = DateTime.Now.AddSeconds(Main.inst.SECONDS);
    }

    void PauseF()
    {
        //Debug.Log("&&&" + Time.timeScale);
        Main.inst.PauseGame();
    }
    void MuteF(bool uptdsndbtn = true)
    {
        if(uptdsndbtn)
        Saves.inst.OPTION_MUTE = (Saves.inst.OPTION_MUTE == true) ? false : true;

        if (Saves.inst.OPTION_MUTE==false) 
        {
            Mute.transform.Find("MuteON").gameObject.SetActive(false);
            Mute.transform.Find("MuteOFF").gameObject.SetActive(true);
        }
        else
        {
            Mute.transform.Find("MuteON").gameObject.SetActive(true);
            Mute.transform.Find("MuteOFF").gameObject.SetActive(false);
        }

        SoundManager.Instance. MuteOn(Saves.inst.OPTION_MUTE);
    }
    public void runCountDown()
    {
        CountDown.GetComponent<Animator>().Play("CountDown");
    }

    public void runOtherText(string text, string color="default", string anim = "default")
    {
        Color TextColor = new Color(1F, 1f, 1f);
        String animName = "OtherText";

        switch (color)
        {
            case "default": TextColor = new Color(1F, 0.7f, 0.1f)            ; break;
            case "blue":    TextColor = new Color(0.0037F, 0.6506904f, 0.7924528f); break;
        }

        switch (anim)
        {
            case "default":     animName = "OtherText"; break;
            case "overtime":    animName = "Overtime"; break;
        }

        Text tmp = OtherText.GetComponentInChildren<Text>();
        tmp.text = text;
        tmp.color = TextColor;
        OtherText.GetComponent<Animator>().Play(animName);
    }

    void Update () {
        //TIMER
       // Debug.Log(timerNEW);
        if (Main.inst.GAME_STATE == Main.gameStates.game)
        {
            if(Main.inst.GAME_PERIOD != Main.gamePeriods.OT)
            {
                // timer = timer.AddSeconds(Time.deltaTime);
                TimeSpan delta = timerNEW - DateTime.Now;

                Main.inst.MINUTES = delta.Minutes;
                Main.inst.SECONDS = delta.Seconds;

                //Main.inst.MINUTES = Main.inst.SECONDS - timer.Minute;
               // Main.inst.SECONDS = Main.inst.SECONDS - timer.Second;
            }

            if (Main.inst.MINUTES == 0 && Main.inst.SECONDS == 0) 
            {
                //TODO: OVERTIME
                if(Main.inst.SCORE1 != Main.inst.SCORE2)
                    GameTimerStop(); //СТОП ИГРА!!!!
                else
                    Main.inst.GAME_PERIOD = Main.gamePeriods.OT;
            }
            //запуск анимации овертайм
            if (Main.inst.GAME_PERIOD == Main.gamePeriods.OT && triggerOvertime == false) 
            {
                triggerOvertime = true;
                runOtherText("OVERTIME", "blue", "overtime");
            }

            time.text = Main.inst.MINUTES.ToString("00") + ":" + Main.inst.SECONDS.ToString("00");
            CheckForPenalty();

            //Debug.Log(Main.inst.MINUTES);
            //Debug.Log(Main.inst.SECONDS);

            if (Main.inst.PP1 > 0)
            {
                timer1 = timer1.AddSeconds(Time.deltaTime);
                Main.inst.PP1 = secondsPP1 - timer1.Second;
            }
            if (Main.inst.PP2 > 0)
            {
                timer2 = timer2.AddSeconds(Time.deltaTime);
                Main.inst.PP2 = secondsPP2 - timer2.Second;
            }
            if (Main.inst.PP2 > 0 || Main.inst.PP1 > 0)
            {
                pp1time.text = "PP: " + Main.inst.PP1;
                pp2time.text = "PP: " + Main.inst.PP2;
            }


            if(Main.inst.CURRENTBONUS != null && Main.inst.CURRENTBONUS.GetComponent<Bonus>().bonusType == "fol")
            {
                if (Main.inst.CURRENTBONUS != null && Main.inst.PP1 == 0 && Main.inst.CURRENTBONUS.GetComponent<Bonus>().bonusTeam == "team1")
                {
                    Debug.Log("Main.inst.PP1 == 0");
                    Main.inst.StopBonus("fol", "team1"); 
                    secondsPP1 = 0; timer1 = new DateTime();
                }
                if (Main.inst.CURRENTBONUS != null && Main.inst.PP2 == 0 && Main.inst.CURRENTBONUS.GetComponent<Bonus>().bonusTeam == "team2")
                {
                    Debug.Log("Main.inst.PP2 == 0");
                    Main.inst.StopBonus("fol", "team2"); 
                    secondsPP2 = 0; timer2 = new DateTime();
                }
            }

            //считаем периоды
            var tmpTotalSec = Main.inst.SETTINGS_SECONDS;
            var tmpCurTotalSec = Main.inst.SECONDS;
            var tmpCutPeriod = tmpCurTotalSec / (tmpTotalSec / 3); //Mathf.Round

            if (tmpCutPeriod >= 2) Main.inst.GAME_PERIOD = Main.gamePeriods.P1;
            else if (tmpCutPeriod >= 1) Main.inst.GAME_PERIOD = Main.gamePeriods.P2;
            else if (tmpCutPeriod > 0) Main.inst.GAME_PERIOD = Main.gamePeriods.P3;
        }
        else
        {
            timerNEW = DateTime.Now.AddSeconds(Main.inst.SECONDS + Main.inst.MINUTES * 60);
        }



        scoreTeam1.text = Main.inst.SCORE1.ToString();
        scoreTeam2.text = Main.inst.SCORE2.ToString();
        period.text = Main.inst.GAME_PERIOD.ToString();
    }

    public void SetPP(string team, int time)
    {
        if (team == "team1")    { secondsPP1 = time; }
        if (team == "team2")    { secondsPP2 = time; }
    }

    private void CheckForPenalty()
    {
        if (Main.inst.PP1 > 0)  { pp1.SetActive(true);  }
        else                    { pp1.SetActive(false); }

        if (Main.inst.PP2 > 0)  { pp2.SetActive(true);  }
        else                    { pp2.SetActive(false); }
    }
    private void GameTimerStop()
    {
        Main.inst.StopGame();
    }
}
