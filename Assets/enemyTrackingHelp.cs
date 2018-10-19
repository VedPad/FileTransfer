using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTrackingHelp : MonoBehaviour {
	public Rigidbody2D playerRB;
	public float dist;
	public Vector2 heading;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		positionTracker ();
		this.transform.position = playerRB.position + (heading * dist);
	}

	void positionTracker(){
		heading = playerRB.velocity.normalized;
	}
}
