using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shockWaveScript : MonoBehaviour {
	private SpriteRenderer sR;
	public float opacity = 1f;
	// Use this for initialization
	void Start () {
		sR = this.GetComponent<SpriteRenderer> ();
		Destroy (this.gameObject, 10f);
	}
	
	// Update is called once per frame
	void Update () {
		opacity -= 0.01f;
		this.transform.localScale = new Vector3 (this.transform.localScale.x + this.transform.localScale.x/5.5f, this.transform.localScale.y + this.transform.localScale.y/5.5f, this.transform.localScale.z);
		sR.color = new Color (1f, 1f, 1f, opacity);
	}
}
