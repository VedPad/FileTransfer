using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissiles : MonoBehaviour {
	public Vector2 target;
	public float hitAmount;
	public float hitMax;
	public Rigidbody2D rb;

	Vector2 lastPos;
	float threshold = 0.0f;
	public float turnSpeed = 200f;
	public Vector2 offset;
	public enemyTracker eT;
	public int closestEnemy;
	public float speed = 5f;
	public float enemyDist;
	public float timer;
	void Start(){
		lastPos = this.transform.position;
		rb = this.GetComponent<Rigidbody2D> ();
	}

	void Update(){
		if (hitAmount >= hitMax) {
			Destroy (this.gameObject);
		}

		offset = new Vector2(this.transform.position.x,this.transform.position.y) - lastPos;
		if (offset.x == threshold) {
			enemyDist = 1000f;
		}
		timer++;
		if (timer >= 2) {
			lastPos = new Vector2 (this.transform.position.x, this.transform.position.y);
			timer = 0;
		}
		closeEnemy ();
		/*enemyDist = Vector2.Distance (eT.EnemyRBs[closestEnemy].position, transform.position);


		transform.position = Vector2.MoveTowards (transform.position, target, speed * Time.deltaTime);
		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 90f;
		rb.rotation = Mathf.LerpAngle(rb.rotation,angle,turnSpeed);*/
	}

	void FixedUpdate(){
		
		target = eT.EnemyRBs [closestEnemy].position;
		Vector2 direction = (target - rb.position).normalized;

		float rotateAmount = Vector3.Cross (direction, transform.right).z;
		rb.angularVelocity = -rotateAmount * turnSpeed;
		rb.velocity = transform.right * speed;
	}

	void closeEnemy(){
		for (var i = 0; i < eT.arrayLength; i++) {
			if (Vector2.Distance (eT.EnemyRBs [i].position, transform.position) < enemyDist) {
				closestEnemy = i;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "enemy") {
			Destroy (other.gameObject);
			hitAmount += 1;
		}
	}

}