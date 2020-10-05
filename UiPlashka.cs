using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPlashka : MonoBehaviour
{
    public static UiPlashka inst;
    public Button BUTTON;
    public Image ChampFlag;

    void Awake()
    { 
        //Debug.Log("UiEndGamePanel");
        if (inst == null) { inst = this; }
        else if (inst != null) { Destroy(gameObject); }

        BUTTON.onClick.AddListener(()=> show(false));

        //ПОБЕДА В ЧМ
        if (Saves.inst.CHAMP_IS_CHAMP == 1 && Main.inst.SCORE1 > Main.inst.SCORE2 && Saves.inst.CHAMP_ETAP == 0)
        {
            UiPlashka.inst.show(true);
            Main.inst.EventController("winChamp");
        }
        else
        {
            UiPlashka.inst.show(false);
        }
    }

    public void show(bool todo) 
    {
        gameObject.SetActive(todo);
    }


}
