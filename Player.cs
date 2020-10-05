using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool ISSUPER = false;
    public bool ISPENALTY = false;
    public string POSITION = "";
    public int NUMBER = 0;
    public GameObject FORMA;
    public GameObject HEAD;
    // Use this for initialization
    void Start () {

        gameObject.transform.Find("super").gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Main.inst.GAME_STATE == Main.gameStates.stop && POSITION != "PP")
        {
            if (tag == "team1")
                transform.rotation = Quaternion.Euler(0, 0, -90); GetComponent<Animator>().SetBool("run", true);
            if (tag == "team2")
                transform.rotation = Quaternion.Euler(0, 0, 90); GetComponent<Animator>().SetBool("run", true);
        }

        if (Main.inst.GAME_STATE == Main.gameStates.game)
        {
            if (!ISPENALTY && POSITION != "PP")
            {
                if (tag == "team1")
                {
                    

                    if (Main.inst.team1.GetComponent<Team>().MOVE == new Vector3(0, 1, 0))
                    { transform.rotation = Quaternion.Euler(0, 0, 0); GetComponent<Animator>().SetBool("run", false); }
                    else if (Main.inst.team1.GetComponent<Team>().MOVE == new Vector3(0, -1, 0))
                    { transform.rotation = Quaternion.Euler(0, 0, 180); GetComponent<Animator>().SetBool("run", false); }
                    else
                    { transform.rotation = Quaternion.Euler(0, 0, -90); GetComponent<Animator>().SetBool("run", true); }

                }
                if (tag == "team2")
                {
                    if (Main.inst.team2.GetComponent<Team>().MOVE == new Vector3(0, 1, 0))
                    { transform.rotation = Quaternion.Euler(0, 0, 0); GetComponent<Animator>().SetBool("run", false); }
                    else if (Main.inst.team2.GetComponent<Team>().MOVE == new Vector3(0, -1, 0))
                    { transform.rotation = Quaternion.Euler(0, 0, 180); GetComponent<Animator>().SetBool("run", false); }
                    else
                    { transform.rotation = Quaternion.Euler(0, 0, 90); GetComponent<Animator>().SetBool("run", true); }
                }
            }
            /*
            if (!ISPENALTY && POSITION != "PP")
            {
                gameObject.SetActive(true);
            }
            else if (ISPENALTY && POSITION != "PP")
            {
                gameObject.SetActive(false);
            }*/
        }
    }
    public void PlayerToConsole()
    {
        Debug.Log(NUMBER);
    }
    public void SetSuper(bool ToDo)
    {
        ISSUPER = ToDo;
        gameObject.transform.Find("super").gameObject.SetActive(ISSUPER);
    }
    public void SetPenalty(bool ToDo,bool ToShow)
    {
        ISPENALTY = ToDo;
        gameObject.SetActive(ToShow);
    }
}
