using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineScript : MonoBehaviour {
	public GameObject explosionEffect;
	public int damage;
	public enemyTracker eT;
	public GameObject player;
	public SpaceShipMoveMent smm;
	// Use this for initialization
	void OnDestroy(){
		GameObject explosion = GameObject.Instantiate (explosionEffect, this.transform.position, this.transform.rotation);
		Destroy (explosion, 5f);
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject == player) {
			smm.health -= damage;
			Destroy (this.gameObject);
		}
        if(other.gameObject.tag == "enemy")
        {
            enemyAI eAI = other.gameObject.GetComponent<enemyAI>();
            eAI.health -= damage;
            Destroy(this.gameObject);
        }

	}

}
