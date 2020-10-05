using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour {


    //public float hubforce = 2.2f;
    public static Hub inst;

    void Awake()
    {
        if (inst == null)
        { inst = this; }
        else if (inst != null)
        { Destroy(gameObject); }
    }

	// Update is called once per frame
	void Update () {
        if (Main.inst.GAME_STATE == Main.gameStates.game)
        {
            if (this.gameObject.transform.position.x>0)
            {
                Main.inst.TIMEONATTACK1++;
            }
            if (this.gameObject.transform.position.x < 0)
            {
                Main.inst.TIMEONATTACK2++;
            }
        }
	}
    public void StopHub()
    {
       //Debug.Log("StopHub");
        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
    public void SuperHub(string team)
    {
        var vorota2 = new Vector3(-7.2F,0,0) - gameObject.transform.position;
        var vorota1 = new Vector3(7.2F, 0, 0) - gameObject.transform.position;

        if (team == "team1")
            gameObject.GetComponent<Rigidbody2D>().velocity = vorota1 * Saves.inst.OPTION_HUBSPEED * 2;
        else
            gameObject.GetComponent<Rigidbody2D>().velocity = vorota2 * Saves.inst.OPTION_HUBSPEED * 2;
    }
    public void PushHub()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(RandomSign() * 2.0f, RandomSign() * 1F) * Saves.inst.OPTION_HUBSPEED;
    }
    public int RandomSign() {
        return Random.value< .5? 1 : -1;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "vorota1shtanga" || collision.gameObject.tag == "vorota2shtanga")
        {
            Main.inst.EventController("shtanga");
        }
        if (collision.gameObject.tag == "poleborders")
        {
            //Debug.Log("poleborders");
            Main.inst.EventController("hubBort");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        float tmpY = 0;

        //Debug.Log(collision.gameObject.tag);

        //Проверка на окончание бонуса СУПЕР УДАР
        if ((collision.gameObject.tag == "team1" || collision.gameObject.tag == "team2") && collision.gameObject.transform.GetComponent<Player>().ISSUPER == true)
        {
            Main.inst.StopBonus("sup", collision.gameObject.tag, collision.gameObject);
        }
        // УДАР ПО ШАЙБЕ от ИГРОКА
        else if (collision.gameObject.tag == "team2")
        {
            if (Main.inst.team2.GetComponent<Team>().MOVE == new Vector3(0, -1, 0))
                tmpY = Random.Range(-1 * Saves.inst.OPTION_MAX_ANG, -1 * Saves.inst.OPTION_MIN_ANG);
            else if (Main.inst.team2.GetComponent<Team>().MOVE == new Vector3(0, 1, 0)) 
                tmpY = Random.Range(1 * Saves.inst.OPTION_MIN_ANG, 1 * Saves.inst.OPTION_MAX_ANG);
            else
                tmpY = Random.Range(-0.5f, 0.5f);

            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-4.0f, tmpY) * Saves.inst.OPTION_HUBSPEED;
            //STATA
            Main.inst.HITSANDPASSES2++;
            Main.inst.EventController("shot");
        }
        else if (collision.gameObject.tag == "team1")
        {
            if (Main.inst.team1.GetComponent<Team>().MOVE == new Vector3(0, -1, 0))
                tmpY = Random.Range(-1*Saves.inst.OPTION_MIN_ANG, -1*Saves.inst.OPTION_MAX_ANG);
            else if (Main.inst.team1.GetComponent<Team>().MOVE == new Vector3(0, 1, 0))
                tmpY = Random.Range(1 * Saves.inst.OPTION_MAX_ANG, 1 * Saves.inst.OPTION_MIN_ANG);
            else
                tmpY = Random.Range(-0.5f, 0.5f);

            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(4.0f, tmpY) * Saves.inst.OPTION_HUBSPEED;
            //STATA
            Main.inst.HITSANDPASSES1++;
            Main.inst.EventController("shot");
        }
        //Определяем последнего игрока досконувшегося до шайбы КРОМЕ голкипера
        if (collision.gameObject.tag == "team2" || collision.gameObject.tag == "team1")
        {
            
            //Послдений игрок в каждой из команд, кроме ГОЛКИПЕРА
            if(collision.gameObject.transform.GetComponent<Player>().POSITION != "GK") {
                if (collision.gameObject.tag == "team1")
                    Main.inst.tmpLASTPLAYERt1 = collision.gameObject;
                if (collision.gameObject.tag == "team2")
                    Main.inst.tmpLASTPLAYERt2 = collision.gameObject;
            }

            //Если не ГОЛКИПЕР делаем ПОСЛДЕДНЕГО ИГОРОКА 
            if (collision.gameObject.transform.GetComponent<Player>().POSITION != "GK")
            {
                Main.inst.LASTPLAYER = collision.gameObject;
            }
            else //Если ЖЕ ГОЛКИПЕР делаем ПОСЛДЕДНЕГО ИГОРОКА равным последним игроком из командв ГОЛКИПЕРА
            if (collision.gameObject.transform.GetComponent<Player>().POSITION == "GK")
            {
                if (collision.gameObject.tag == "team1") 
                {
                    Main.inst.LASTPLAYER = Main.inst.tmpLASTPLAYERt1;
                }
                if (collision.gameObject.tag == "team2")
                {
                    Main.inst.LASTPLAYER = Main.inst.tmpLASTPLAYERt2;
                }
            }

        }

        if (collision.gameObject.tag == "vorota1goal" || collision.gameObject.tag == "vorota2goal")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = gameObject.GetComponent<Rigidbody2D>().velocity*0.02F;
            Main.inst.Goal(collision.gameObject.tag);
        }

        if (collision.gameObject.tag == "bonus")
        {
            Debug.Log("BONUS");
            Main.inst.StartBonus(collision.gameObject);
            Main.inst.CURRENTBONUS = Main.inst.LASTBONUS;
            collision.gameObject.SetActive(false);
        }
    }
}
