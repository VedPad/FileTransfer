using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePower : MonoBehaviour {
	public SpaceShipMoveMent smm;
	public bool timeSlow;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (timeSlow) {
			if (smm.timeForce > 10) {
				Time.timeScale = 0.5f;
			} else {
				Time.timeScale = 1f;
			}

		}
        if (Input.GetKeyDown(KeyCode.Space))
        {
			timeSlow = true;
            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
			timeSlow = false;
            Time.timeScale = 1f;
        }
	}
}
