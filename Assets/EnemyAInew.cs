using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAInew : MonoBehaviour {
	public Vector2 direction;
	public Vector2 velSlow;
	public float moveSpeed;
	public float origMoveSpeed;
	public float moveSpeedreplace;
	public bool slowDown;
	private Vector2 velocity;
	public float distance;
	public GameObject player;
	private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (slowDown == true) {
			SlowDown ();
		}
		velocity = rb.velocity;
		enemyMove ();
		enemyVel ();
		distance = Vector2.Distance (rb.position, player.transform.position);
		direction = (new Vector2 (player.transform.position.x, player.transform.position.y) - rb.position).normalized;
	}

	void enemyVel(){
		if (distance > 5) {
			if (velocity.x < 0 && direction.x > 0) {
				//rb.velocity = Vector2.zero;
				Debug.Log("slow");
				slowDown = true;
				velSlow = velocity;
			}
			if (velocity.x > 0 && direction.x < 0) {
				//rb.velocity = Vector2.zero;
				Debug.Log("slow");
				slowDown = true;
				velSlow = velocity;
			}
			if (distance > 35) {
				moveSpeed = moveSpeedreplace;
			} else {
				moveSpeed = origMoveSpeed;
			}
			/*if (velocity.y < -0.8 && direction.y > 0.8) {
				Debug.Log("slow");
				slowDown = true;
				velSlow = velocity;
			}
			if (velocity.y > 0.8 && direction.y < -0.8) {
				Debug.Log("slow");
				slowDown = true;
				velSlow = velocity;
			}*/
		}
			
	}

	void SlowDown(){
		if (rb.velocity.x > velSlow.x * 0.5 ) {
			rb.velocity = new Vector2(rb.velocity.x - 0.2f, rb.velocity.y);
		}
		if (rb.velocity.y > velSlow.y * 0.5) {
			rb.velocity = new Vector2 (rb.velocity.x, rb.velocity.y - 0.2f);
		}
		if (rb.velocity.x <= velSlow.x * 0.5 && rb.velocity.y <= velSlow.y * 0.5) {
			slowDown = false;
		}
	}

	void enemyMove(){
		Vector2 newPos = direction * Time.deltaTime * moveSpeed;
		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 90f;
		rb.rotation = angle;
		rb.AddForce (newPos);
	}
}
