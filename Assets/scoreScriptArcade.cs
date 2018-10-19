using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreScriptArcade : MonoBehaviour {
    public Text scoreText;
    public SpaceShipMoveMent smm;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = smm.score.ToString();
	}
}
