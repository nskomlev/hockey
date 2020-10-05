using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiOptions : MonoBehaviour
{
    public Slider sliderPeriodTime;
    public Slider sliderHubSpeed;
    public Text textPeriodTime;
    public Text textHubSpeed;
    public static UiOptions inst;
    public Button EXIT;

    void Awake()
    {
        //Debug.Log("UiEndGamePanel");
        if (inst == null) { inst = this; }
        else if (inst != null) { Destroy(gameObject); }

        EXIT.onClick.AddListener(()=>show(false));
        show(false);
    }

    public void show(bool todo)
    {
        gameObject.SetActive(todo);
    }

    // Start is called before the first frame update
    void Start()
    {
        sliderPeriodTime.onValueChanged.AddListener(delegate { FsliderPeriodTime(); });
        sliderHubSpeed.onValueChanged.AddListener(delegate { FsliderHubSpeed(); });
    }
    public void FsliderPeriodTime()
    {
        textPeriodTime.text = (sliderPeriodTime.value * 30).ToString() + " seconds";
        Saves.inst.OPTION_TIMEPERIOD = (int)sliderPeriodTime.value * 30;
        PlayerPrefs.SetInt("OPTION_TIMEPERIOD", Saves.inst.OPTION_TIMEPERIOD);
        Saves.inst.LoadOptions();
    }
    public void FsliderHubSpeed()
    {
        string tmpSpeed = "hard";
        float tmpSpeedF = 2f;
        float tmpOPTION_MIN_ANG = 1.5f;
        float tmpOPTION_MAX_ANG = 2.5f;

        switch (sliderHubSpeed.value.ToString())
        {
            case "1":
                tmpSpeed = "easy";
                tmpSpeedF = 1.5f;
                tmpOPTION_MIN_ANG = 2.7f;
                tmpOPTION_MAX_ANG = 3.5f;
                break; //2.5f, 3.5f
            case "2":
                tmpSpeed = "hard";
                tmpSpeedF = 2f;
                tmpOPTION_MIN_ANG = 1.7f;
                tmpOPTION_MAX_ANG = 2.5f;
                break; //1.5f, 2.5f
        }

        textHubSpeed.text = tmpSpeed;
        Saves.inst.OPTION_HUBSPEED = tmpSpeedF;
        Saves.inst.OPTION_MAX_ANG = tmpOPTION_MAX_ANG;
        Saves.inst.OPTION_MIN_ANG = tmpOPTION_MIN_ANG;
        PlayerPrefs.SetFloat("OPTION_HUBSPEED", Saves.inst.OPTION_HUBSPEED);
        PlayerPrefs.SetFloat("OPTION_MIN_ANG", Saves.inst.OPTION_MIN_ANG);
        PlayerPrefs.SetFloat("OPTION_MAX_ANG", Saves.inst.OPTION_MAX_ANG);
        Saves.inst.LoadOptions();
    }
    public void SetCurOptSlider()
    {
        sliderPeriodTime.value = Saves.inst.OPTION_TIMEPERIOD / 30;
        var tmpSpeed = 2;
        var tmpSpeedName = "hard";
        switch (Saves.inst.OPTION_HUBSPEED.ToString())
        {
            case "1.7":
                tmpSpeed = 1;
                tmpSpeedName = "easy";

                break;
            case "2":
                tmpSpeed = 2;
                tmpSpeedName = "hard";

                break;
        }
        sliderHubSpeed.value = tmpSpeed;

        textHubSpeed.text = tmpSpeedName;
        textPeriodTime.text = Saves.inst.OPTION_TIMEPERIOD + " seconds";
    }

}
