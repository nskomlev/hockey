using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour {

    public string NAME;
    public string TYPE;
    public bool AICONTROL;
    public Color COLOR;
    public Sprite FORMA;
    public float SPEED;
    public float DIF;
    public List<GameObject> PLAYERS = new List<GameObject>(); //все игроки

    public Vector3 MOVE = new Vector3(0, 0, 0);

    public float STAT_1;
    public float STAT_2;
    public float STAT_3;
    public float STAT_4;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
