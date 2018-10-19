using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScriptTest : MonoBehaviour {
	public float spawnChance;
	public GameObject enemyPrefab;
	public enemyTracker eT;
	public float spawnAmount;
	public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		spawnChance = Mathf.Round (Random.Range (0, 1000));
		if (spawnChance > spawnAmount) {
			GameObject justSpawned = GameObject.Instantiate (enemyPrefab, transform.position, transform.rotation);
			enemyAI eAI = justSpawned.GetComponent<enemyAI> ();
			eAI.player = player;
			eAI.shk = Camera.main.GetComponent<CamerShake> ();
			eAI.eT = eT;
		}
	}
}
