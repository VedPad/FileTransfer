using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsAI : MonoBehaviour {
	public Transform player;
	public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = Vector2.MoveTowards (transform.position, player.position, speed * Time.deltaTime);
	}
}
