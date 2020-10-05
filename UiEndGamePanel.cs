using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiEndGamePanel : MonoBehaviour
{
    public static UiEndGamePanel inst;

    public Text TITLE;
    public Button BUTTON;
    public Button RESTART;
    public Button HELP;

    public Text Team1Name;
    public GameObject Team1Flag;
    public Text Team1Score;
    public Text Team1Attack;
    public Text Team1Pass;
    public Text Team1Penalty;
    public Text Team1GoalsPP;

    public Text Team2Name;
    public GameObject Team2Flag;
    public Text Team2Score;
    public Text Team2Attack;
    public Text Team2Pass;
    public Text Team2Penalty;
    public Text MatchTime;

    public Text Team2GoalsPP;
    void Awake()
    {
        //Debug.Log("UiEndGamePanel");
        if (inst == null) { inst = this; }
        else if (inst != null) { Destroy(gameObject); }

        BUTTON.onClick.AddListener(BtnF);
        RESTART.onClick.AddListener(BtnRest);
        HELP.onClick.AddListener(UiHelp.inst.ShowMe);
        gameObject.SetActive(false);
    }

    public void SetPanel()
    {
        int minutesTimer = Main.inst.SETTINGS_SECONDS / 60;
        int secondsTimer = Main.inst.SETTINGS_SECONDS % 60;


        if (Main.inst.GAME_STATE == Main.gameStates.pause)
            MatchTime.text = "Time: " + Main.inst.MINUTES.ToString("00") + ":" + Main.inst.SECONDS.ToString("00");

        if (Main.inst.GAME_STATE == Main.gameStates.end)
            MatchTime.text = "Match time: " + minutesTimer.ToString("00") + ":" + secondsTimer.ToString("00");

        Team1Name.text = Main.inst.TEAMNAME1;
        Team1Score.text = Main.inst.SCORE1.ToString();
        var tmpTOA1 = Mathf.Round(Main.inst.TIMEONATTACK1 / (Main.inst.TIMEONATTACK1 + Main.inst.TIMEONATTACK2) * 100);
        Team1Attack.text = tmpTOA1.ToString();
        Team1Pass.text = Main.inst.HITSANDPASSES1.ToString();
        Team1Penalty.text = Main.inst.PENALTIES1.ToString();
        Team1GoalsPP.text = Main.inst.PPGOLASB1.ToString() + "/" + Main.inst.PPGOLASM1.ToString();

        Team2Name.text = Main.inst.TEAMNAME2;
        Team2Score.text = Main.inst.SCORE2.ToString();
        var tmpTOA2 = Mathf.Round(Main.inst.TIMEONATTACK2 / (Main.inst.TIMEONATTACK1 + Main.inst.TIMEONATTACK2) * 100);
        Team2Attack.text = tmpTOA2.ToString();
        Team2Pass.text = Main.inst.HITSANDPASSES2.ToString();
        Team2Penalty.text = Main.inst.PENALTIES2.ToString();
        Team2GoalsPP.text = Main.inst.PPGOLASB2.ToString() + "/" + Main.inst.PPGOLASM2.ToString();



    }

    private void BtnF()
    {
        if (Main.inst.GAME_STATE == Main.gameStates.pause) 
        {
            ControlPanel("continue");
        }
        else if(Main.inst.GAME_STATE == Main.gameStates.end)
        {
            if (Saves.inst.CHAMP_IS_CHAMP == 1 && Main.inst.SCORE1 > Main.inst.SCORE2)
                UiChamp.inst.show(true);

            if (Saves.inst.CHAMP_IS_CHAMP == 1 && Main.inst.SCORE1 > Main.inst.SCORE2 && Saves.inst.CHAMP_ETAP == 1) 
                UiPlashka.inst.show(true);

            Main.inst.ReatartGame("end");
        }
    }
    public void show(bool todo)
    {
        gameObject.SetActive(todo);
    }
    private void BtnRest()
    {
        //Main.inst.StopGame();
        Main.inst.PauseGame();
        UiChamp.inst.SetChamp(0);
        Main.inst.ReatartGame();
    }

    public void ControlPanel(string TYPE)
    {
        switch (TYPE)
        {
            case "pause":
                SetPanel();
                TITLE.text = "PAUSE";
                BUTTON.GetComponentInChildren<Text>().text = "CONTINUE";
                gameObject.SetActive(true);
                break;

            case "continue":
                Main.inst.PauseGame();
                gameObject.SetActive(false);
                break;

            case "end":
                SetPanel();
                if (Saves.inst.CHAMP_IS_CHAMP == 1)
                {
                    if (Saves.inst.CHAMP_ETAP != 1) 
                        TITLE.text = "CHAMPIONSHIP 1/"+ Saves.inst.CHAMP_ETAP;
                    else
                        TITLE.text = "CHAMPIONSHIP FINAL";

                    if (Main.inst.SCORE1>Main.inst.SCORE2)
                        BUTTON.GetComponentInChildren<Text>().text = "CONTINUE";
                    else
                        BUTTON.GetComponentInChildren<Text>().text = "END";
                }
                else
                { 
                    TITLE.text = "END";
                    BUTTON.GetComponentInChildren<Text>().text = "NEW GAME";
                    UiChamp.inst.SetChamp(0);
                }
                RESTART.gameObject.SetActive(false);
                gameObject.SetActive(true);
                break;
        }
    }
}
