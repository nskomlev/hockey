using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UiControlBtns : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{ 
    public void OnPointerDown(PointerEventData data)
    {
        switch (data.pointerEnter.transform.name)
        {
            case "Team1UpBtn":      Main.inst.Team1UpBtn    = true; break;
            case "Team1DownBtn":    Main.inst.Team1DownBtn  = true; break;
            case "Team2UpBtn":      Main.inst.Team2UpBtn    = true; break;
            case "Team2DownBtn":    Main.inst.Team2DownBtn  = true; break;
        }
        // do any custom "OnPress" behavior here
    }

    public void OnPointerUp(PointerEventData data)
    {
        Debug.Log(data.pointerEnter.transform.name);

        switch (data.pointerEnter.transform.name)
        {
            case "Team1UpBtn":      Main.inst.Team1UpBtn    = false; break;
            case "Team1DownBtn":    Main.inst.Team1DownBtn  = false; break;
            case "Team2UpBtn":      Main.inst.Team2UpBtn    = false; break;
            case "Team2DownBtn":    Main.inst.Team2DownBtn  = false; break;
        }
        // do any custom "OnRelease" behavior here
    }
    public void OnPointerExit(PointerEventData data)
    {
        switch (data.pointerEnter.transform.name)
        {
            case "Team1UpBtn": Main.inst.Team1UpBtn = false; break;
            case "Team1DownBtn": Main.inst.Team1DownBtn = false; break;
            case "Team2UpBtn": Main.inst.Team2UpBtn = false; break;
            case "Team2DownBtn": Main.inst.Team2DownBtn = false; break;
        }
    }
}
