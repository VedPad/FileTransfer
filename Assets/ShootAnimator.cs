using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAnimator : MonoBehaviour {
	public Animator anim;
	public float shootAmount; 
	public bool shot;
	public Vector2 direction;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		direction = (Camera.main.ScreenToWorldPoint (Input.mousePosition) - this.transform.position).normalized;
		//changeFacing ();
		if (Input.GetMouseButtonDown (0)) {
			if (shot == false) {
				anim.SetBool ("shooting", true);
				shot = true;
			}

		}
		if (shot == true) {
			shootAmount+= Time.deltaTime;
		}
		if (shootAmount > 0.3) {
			shot = false;
			shootAmount = 0;
			anim.SetBool ("shooting", false);
		}
	}

	void changeFacing(){
		if (direction.x >= 0) {
			this.transform.localScale = new Vector3(5,5,5);
		} else {
			this.transform.localScale = new Vector3(-5,5,5);
		}
	}
}
