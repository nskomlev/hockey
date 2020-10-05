using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour {


    public Sprite fol;
    public Sprite sup;

    public string bonusType = "fol";
    public string bonusTeam = "";

    // Use this for initialization
    void Awake () {

        float tmpRnd = Random.Range(0,10);
        Sprite tmpSprite = fol;

       // Debug.Log(tmpRnd);

        if (tmpRnd > 4.5F)
        {
            tmpSprite = fol;
            bonusType = "fol";
        }
        else
        {
            tmpSprite = sup;
            bonusType = "sup";
        }

        GetComponent<SpriteRenderer>().sprite = tmpSprite;
        Main.inst.LASTBONUS = gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
