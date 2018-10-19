using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravitationalField : MonoBehaviour {
	public GameObject planetoid;
	public enemyTracker eT;
	public GameObject player;
	public GameObject radiusFinder;
	public float planetMass;
	public Vector2 direction;
	public float pullAmount;
	public bool attracting;
	public Rigidbody2D playerRB;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (Vector2.Distance(player.transform.position, this.transform.position));
		if (Vector2.Distance (player.transform.position, planetoid.transform.position) < Vector2.Distance(planetoid.transform.position,radiusFinder.transform.position)) {
			attracting = true;
		} else {
			attracting = false;
		}
		enemyAttraction ();

		Attraction ();
		direction = (planetoid.transform.position - player.transform.position).normalized;
	}

	void Attraction(){
		if (attracting == true) {
			Vector2 pullForce = direction * planetMass * Time.deltaTime;
			playerRB.AddForce (pullForce);

		}
	}

	void enemyAttraction(){
		foreach (Rigidbody2D enemy in eT.EnemyRBs) {
			if(Vector2.Distance(enemy.position,new Vector2(planetoid.transform.position.x,planetoid.transform.position.y)) < Vector2.Distance(planetoid.transform.position,radiusFinder.transform.position)){
				enemyAI aI = enemy.gameObject.GetComponent<enemyAI> ();
				aI.chasing = false;
				Vector2 directionEnemy = (new Vector2(planetoid.transform.position.x,planetoid.transform.position.y)-enemy.position).normalized;
				Vector2 pullForceEnemy = directionEnemy * planetMass * Time.deltaTime;
				enemy.AddForce(pullForceEnemy);
			}
		}
	}




}
