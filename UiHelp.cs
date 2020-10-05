using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHelp : MonoBehaviour
{
    public static UiHelp inst;
    public Button BUTTON;

    void Awake()
    {
        //Debug.Log("UiEndGamePanel");
        if (inst == null) { inst = this; }
        else if (inst != null) { Destroy(gameObject); }

        BUTTON.onClick.AddListener(HideMe);
        gameObject.SetActive(false);
    }

    public void HideMe()
    {
        if (Saves.inst.PLAY_AFTER_HELP == true)
        {
            Main.inst.RealStartGame();
        }
        gameObject.SetActive(false);
    }

    public void ShowMe()
    {
        gameObject.SetActive(true);
    }
}
