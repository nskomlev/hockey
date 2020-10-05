using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saves : MonoBehaviour
{
    public static Saves inst;

    [System.NonSerialized]
    public string TEAMNAME1;
    [System.NonSerialized]
    public string TEAMNAME2;
    [System.NonSerialized]
    public string STATA_GAMEPLAYS;
    [System.NonSerialized]
    public string STATA_GAMEWINS;
    public int CHAMP_ETAP=4;
    public int CHAMP_IS_CHAMP=0;
    public string CHAMP_CUR_PLAYER_SOPER = "";
    public string CHAMP_CUR_PLAYER_TEAM = "";

    public bool OPTION_MUTE = false;
    public bool PLAY_AFTER_HELP = false;

    public float OPTION_HUBSPEED = 2.0F;
    public int OPTION_TIMEPERIOD = 60;

    public float OPTION_MIN_ANG = 1.5f;
    public float OPTION_MAX_ANG = 2.5f;

    public int ADS_COUNTER = 0;

    public string[] CHAMP_CAHMPSCORE=
    {
        "NNN&-&NNN&-",
        "NNN&-&NNN&-",
        "NNN&-&NNN&-",
        "NNN&-&NNN&-",
        "NNN&-&NNN&-",
        "NNN&-&NNN&-",
        "NNN&-&NNN&-"
    };

    void Awake()
    {
        if (inst == null) { inst = this; }
        else if (inst != null) { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);
    }

    public void SaveTeams(string name1, string name2)
    {
        PlayerPrefs.SetString("TEAMNAME1", name1);
        PlayerPrefs.SetString("TEAMNAME2", name2);
    }
    public void LoadOptions()
    {
        OPTION_MUTE         =(PlayerPrefs.GetInt("OPTION_MUTE", 1)==1)?(true) :(false);
        OPTION_HUBSPEED     = PlayerPrefs.GetFloat("OPTION_HUBSPEED", 2.0F);
        OPTION_TIMEPERIOD   = PlayerPrefs.GetInt("OPTION_TIMEPERIOD",60);
        Main.inst.SETTINGS_SECONDS = OPTION_TIMEPERIOD;
        UiOptions.inst.SetCurOptSlider();
    }
    public string LoadTeam(int teamNum)
    {
        string loadedteamname = "RUS";

        switch (teamNum)
        {
            case 1: loadedteamname = PlayerPrefs.GetString("TEAMNAME1", "RUS"); break;
            case 2: loadedteamname = PlayerPrefs.GetString("TEAMNAME2", "CAN"); break;
        }

        return loadedteamname;
    }
}
