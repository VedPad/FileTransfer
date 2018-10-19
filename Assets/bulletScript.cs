using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour {
	public Vector2 direction;
	public int damage;
	public bool playerBul;
	public GameObject player;
	public SpaceShipMoveMent smm;
	public bool Hit;
	public Animator bulletAnim;
	public CamerShake shk;
	public float distance;
	public float bulletSpeed;
	private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 30f);
		player = GameObject.Find ("SpaceShip");
		smm = player.GetComponent<SpaceShipMoveMent> ();
		shk = Camera.main.GetComponent<CamerShake> ();
		rb = this.GetComponent<Rigidbody2D> ();
		rb.rotation = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
		rb.AddForce (direction * bulletSpeed );
	}
	
	// Update is called once per frame
	void Update () {
		
		if (playerBul) {
			distance = Vector2.Distance (player.transform.position, this.transform.position);
		}
		rb.rotation = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
	}

	void OnTriggerEnter2D(Collider2D other){
		
		if (playerBul == true) {
			if (!Hit) {
				if (other.gameObject.tag == "enemy") {
					//Debug.Log ("Hit");
					rb.velocity = Vector2.zero;
					enemyAI eAI = other.gameObject.GetComponent<enemyAI> ();
					smm.health += Mathf.Round (eAI.health / 7);
                    
					eAI.health -= damage;
                    if(eAI.health <= 0)
                    {
                        smm.score += eAI.maxHealth;
                    }
					//rb.position = other.gameObject.transform.position;
					bulletAnim.SetBool ("hit", true);
					Hit = true;
					if (distance < 10) {
						StartCoroutine (shk.Shake (0.10f, 0.15f));
					}
					Destroy (other.gameObject);
					Destroy (this.gameObject, 3f);
				}
				if (other.gameObject.tag == "deflector") {
					rb.velocity = Vector2.zero;
					bulletAnim.SetBool ("hit", true);
					Hit = true;
					if (distance < 10) {
						StartCoroutine (shk.Shake (0.10f, 0.10f));
					}
					Destroy (this.gameObject, 1f);
				}
			}
		}
			if (!playerBul) {
				if (!Hit) {
					if (other.gameObject.tag == "enemy") {
						//Debug.Log ("Hit");
						rb.velocity = Vector2.zero;
						enemyAI eAI = other.gameObject.GetComponent<enemyAI> ();
						eAI.health -= damage;
						//rb.position = other.gameObject.transform.position;
						bulletAnim.SetBool ("hit", true);
						Hit = true;
						if (distance < 10) {
							StartCoroutine (shk.Shake (0.10f, 0.10f));
						}
						//Destroy (other.gameObject);
						Destroy (this.gameObject,1f);
					}
				if (other.gameObject == player) {
					rb.velocity = Vector2.zero;
					bulletAnim.SetBool ("hit", true);
					Hit = true;
					if (distance < 10) {
						StartCoroutine (shk.Shake (0.10f, 0.10f));
					}
					Destroy (this.gameObject,1f);
				}
				if (other.gameObject.tag == "bullet") {
					rb.velocity = Vector2.zero;
					bulletAnim.SetBool ("hit", true);
					Hit = true;
					Destroy (this.gameObject,1f);
					other.gameObject.GetComponent<Animator> ().SetBool ("hit", true);
					Destroy (other.gameObject,1f);

				}
				if (other.gameObject.tag == "collisionSphere") {
					rb.velocity = Vector2.zero;
					bulletAnim.SetBool ("hit", true);
					Hit = true;
					Destroy (this.gameObject,1f);
				}

				}
			}


		
	}


}
