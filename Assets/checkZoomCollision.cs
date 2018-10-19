using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkZoomCollision : MonoBehaviour {
	public SpaceShipMoveMent sMM;
	public CircleCollider2D cC;
	public bool zoomCollider;
	public float colliderTimer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (sMM.zooming == true) {
			zoomCollider = true;
		}
		if (zoomCollider == true) {
			colliderTimer++;
			cC.radius = 0.5f;
		}

		if (colliderTimer > 20) {
			zoomCollider = false;
			colliderTimer = 0;
			cC.radius = 0f;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "enemy") {
			
			
				Destroy (other.gameObject);


		}
	}
}
