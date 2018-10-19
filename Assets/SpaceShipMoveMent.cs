using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class SpaceShipMoveMent : MonoBehaviour {
	public float angle;
	public float boostNumber = 3f;
	public Image timeForceBar;
	public float timeForce = 300f;
	public float maxTimeForce = 300f;
    public float score;
	public float health = 100f;
	public Image healthBar;
	public float maxHealth = 100f;
	public CircleCollider2D cC;
	public CircleCollider2D cC2;
	private GameObject[] enemyArr;
    public CamerShake shk;
    public GameObject cam;
	public GameObject missile;
    public float shakeElapsed;
	public Image boostBar;
	public GameObject expandingShockwave;
	public PostProcessingBehaviour ppb;
	public bool loseTimeForce;
	public enemyTracker eT;
	public float zoomTimer;
	public int closestEnemy;
	private bool aberritionC;
    public bool shake;
	public float shootTime;
	public float fireRate;
	public Vector2 moveDirection;
	public float enemyDist = 1000;
	public bool shooting;
	public bool zooming = false;
	public bool speeding;
	public float aberritionT;
	public Vector2 directionFacing;
	public GameObject projectile;
	public GameObject gun;
	public float speed;
	public float finalAngle;
	public bool zoomReload = false;
	public float zoomReloadTimer;
	public bool timeCoolDown;
	public float coolDownTimer;
	public float ydis;
	public float xdis;
	public Animator anim;
	public Vector3 mousePos;
	public float regSize = 10;
    private Vector3 origPos;
	Rigidbody2D rb;
	private float zoomCooldownTimer;
	public Rigidbody2D cameraRb;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
        origPos = this.transform.position;
		cC = this.GetComponent<CircleCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
        if(health <= 0)
        {
            StartCoroutine(shk.Shake(0.5f, 1f));
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            GameObject[] landMines = GameObject.FindGameObjectsWithTag("landMine");
            GameObject[] bullets = GameObject.FindGameObjectsWithTag("bullet");
            foreach(GameObject enemy in enemies)
            {
                Destroy(enemy);

            }
            foreach(GameObject landMine in landMines)
            {
                Destroy(landMine);

            }
            foreach(GameObject bullet in bullets)
            {
                Destroy(bullet);
            }
            health = 100;
            timeForce = 100;
            score = 0;
            boostNumber = 3;
            rb.velocity = Vector2.zero;
            this.transform.position = origPos;
        }
        if (speeding)
        {
            if(Camera.main.orthographicSize < 15)
            {
                //ppb.profile.motionBlur.enabled = true;
                Camera.main.orthographicSize += Time.deltaTime * Random.Range(50,100);
            }
        }
        if (!speeding)
        {
            if (Camera.main.orthographicSize > 10)
            {
                Camera.main.orthographicSize -= Time.deltaTime * Random.Range(10, 20);
            }
        }
		if (timeForce <= 20) {
			timeCoolDown = true;
		}
		if (timeCoolDown) {
			coolDownTimer++;
		} else {
			coolDownTimer = 0;
		}
		if (coolDownTimer > 90) {
			timeCoolDown = false;

		}
		if (health > 100) {
			health = 100;
		}
		if (timeCoolDown == false && speeding == false && loseTimeForce == false) {
			timeForce++;
		}

		if (timeForce >= maxTimeForce) {
			timeForce = maxTimeForce;
		}
		if (boostNumber == 0) {
			zoomReloadTimer++;
			if (zoomReloadTimer >= 300) {
				boostNumber = 3;
				zoomReloadTimer = 0;
			}
		}

		healthBarDraw ();
		shootTime+= Time.deltaTime;
		directionFacing = new Vector2(xdis,ydis ).normalized;
		ShipRotation ();
		shipMovement ();
		Shoot ();
		closeEnemy ();
		var caset = ppb.profile.chromaticAberration.settings;
		caset.intensity = Mathf.Lerp (0, 1, rb.velocity.magnitude * 3);
        if (shake)
        {
            shakeElapsed+= Time.deltaTime;
        }

		/*if (Input.GetMouseButtonDown (1)) {
			float rotation;
			if (this.transform.rotation.z > 0) {
				rotation = this.transform.rotation.z - 90f;
			} else {
				rotation = this.transform.rotation.z + 180;
			}
			GameObject missile1 = GameObject.Instantiate (missile, gun.transform.position, Quaternion.Euler(this.transform.rotation.x,this.transform.rotation.y,this.transform.rotation.z));
			HomingMissiles hm = missile1.GetComponent<HomingMissiles> ();
			hm.eT = eT;
		}*/

		/*if (Input.GetMouseButtonDown (1)) {
			GameObject shockwave = GameObject.Instantiate (expandingShockwave, this.transform.position, this.transform.rotation);
		}*/
		if (zoomTimer >= 0.2) {
			zoomTimer = 0;
			//Camera.main.orthographicSize = regSize;
		rb.velocity = new Vector2(10 * directionFacing.x,10 * directionFacing.y);
			
			zooming = false;
		}
		if (zooming == true) {
			zoomTimer+= Time.deltaTime;
			ppb.profile.chromaticAberration.enabled = true;
			//if ((Vector2)transform.position == eT.EnemyRBs [closestEnemy].position) {
			//rb.velocity = new Vector2(100 * directionFacing.x,100 * directionFacing.y);
				//foreach (GameObject enemy in enemyArr) {
				/*if (Vector2.Distance (this.transform.position,enemy.transform.position) < 10) {
						Destroy (enemy.gameObject);
						Debug.Log ("enemyShip");
						StartCoroutine(shk.Shake(0.15f, 0.25f));
					}
				}*/
			//}
		}

		
		
		if (Input.GetMouseButtonDown (1)) {
		if (boostNumber > 0 ) {
				boostNumber -= 1;
				
			enemyArr = GameObject.FindGameObjectsWithTag ("enemy");
			StartCoroutine(shk.Shake(0.15f, 0.3f));
			zooming = true;
			enemyDist = Vector2.Distance (eT.EnemyRBs[closestEnemy].position, transform.position);
			//cC.radius = 2.6f;
			//cC2.radius = 2.5f;
			//rb.rotation = Mathf.LerpAngle(rb.rotation,angle,turnSpeed);
			Vector2 target = eT.EnemyRBs [closestEnemy].position;

			Vector2 direction2 = (target - rb.position).normalized;
			float angle = Mathf.Atan2 (direction2.y, direction2.x) * Mathf.Rad2Deg - 90f;
			//rb.rotation = angle;
			rb.AddForce(new Vector2(directionFacing.x * 10000,directionFacing.y  * 10000));
			}
			
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			loseTimeForce = true;
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			loseTimeForce = false;

		}
		if (loseTimeForce) {
			
				timeForce -= 1;
			
		}

	}

	void healthBarDraw(){
		healthBar.fillAmount = health/maxHealth;
		timeForceBar.fillAmount = timeForce / maxTimeForce;
		boostBar.fillAmount = boostNumber / 3;
	}



	void LateUpdate(){
		if (zooming) {
			Camera.main.orthographicSize += 20 * Time.deltaTime;
			ppb.profile.motionBlur.enabled = true;
			var caset = ppb.profile.chromaticAberration.settings;
			caset.intensity = Mathf.Lerp (0, 1, rb.velocity.magnitude * 3);
		}
		if (zoomTimer >= 0.2) {
			Camera.main.orthographicSize = 10;
			aberritionC = true;
			
		}
		if (aberritionC) {
			aberritionT++;
		}
		if (aberritionT >= 9) {
			cC.radius = 0.868f;
			cC2.radius = 0.8f;
			var caset = ppb.profile.chromaticAberration.settings;
			caset.intensity = Mathf.Lerp (0, 1, rb.velocity.magnitude * 3);
			ppb.profile.motionBlur.enabled = false;
		}
		if (aberritionT >= 50) {
		aberritionT = 0;
		aberritionC = false;
			ppb.profile.chromaticAberration.enabled = false;
		}
		
        
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -30);
       
		
	}

	void closeEnemy(){
		for (var i = 0; i < eT.arrayLength; i++) {
			if (Vector2.Distance (eT.EnemyRBs [i].position, transform.position) < enemyDist) {
				closestEnemy = i;
			}
		}
	}

	void Shoot(){
		if (Input.GetMouseButtonDown (0)) {
			shooting = true;
		}
		if (Input.GetMouseButtonUp (0)) {
			shooting = false;
		}

		if (shooting == true) {
			if (shootTime > 1 / fireRate) {
				GameObject bullet = GameObject.Instantiate (projectile, gun.transform.position, this.transform.rotation);
				bulletScript bScript = bullet.GetComponent<bulletScript> ();
				bScript.direction = new Vector2 (directionFacing.x, directionFacing.y);

				shootTime = 0;
			}
		}
	}

	void ShipRotation(){
		mousePos = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
		xdis = mousePos.x - this.transform.position.x;
		ydis = mousePos.y - this.transform.position.y;
		float tan = ydis / xdis;

		angle = Mathf.Atan (tan);

		if (xdis >= 0) {
			if (ydis >= 0) {
				finalAngle = angle;
			}
			if (ydis < 0) {
				finalAngle = angle + (2f * Mathf.PI);
			}
		}
		if (xdis < 0) {
			if (ydis >= 0) {
				finalAngle = angle + (1f * Mathf.PI);
			}
			if (ydis < 0) {
				finalAngle = angle + (1f * Mathf.PI);
			}
		}

		if (zooming == false) {
			this.transform.rotation = Quaternion.Euler (0, 0, finalAngle * Mathf.Rad2Deg);
		}
		

			
	}

	void shipMovement(){
		if (speeding) {
			if (timeForce > 0.5) {
				timeForce -= 0.5f;
				speed = 400;
				anim.SetFloat ("Speed", 4);
			} else {
				speeding = false;
				speed = 100;
				anim.SetFloat ("Speed", 0);
				
			}
			
		}
		if(Input.GetKeyDown(KeyCode.LeftShift)){
			speed = 400;
			anim.SetFloat ("Speed", 4);
			speeding = true;
		}
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			speed = 100;
			anim.SetFloat ("Speed", 0);
			speeding = false;
		}



		moveDirection = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));



		if (speeding == false) {
			
			
			if (moveDirection.x == 0 && moveDirection.y == 0) {
				anim.SetFloat ("Speed", 0);
			} else {
				anim.SetFloat ("Speed", 2);
			}


		}

		Vector3 finalMove = moveDirection * speed * Time.deltaTime;
		rb.AddForce (finalMove);

	}

    void CamShake()
    {
        
        if (shake)
        {
            if(shakeElapsed <= 0.15f)
            {
                StartCoroutine(shk.Shake(0.15f, 0.4f));
            }else
            {
                shake = false;
                shakeElapsed = 0;
            }
        }
    }

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "enemy") {
			Destroy (other.gameObject);
            StartCoroutine(shk.Shake(0.15f, 0.25f));
		if (!zooming ) {
				health -= 5;
			}
			
			/*if (zooming) {
				rb.velocity = Vector2.zero;
			}*/
        }
		if (other.gameObject.tag == "Ebullet") {
			health -= 3;
		}

		if (other.gameObject.tag == "landMine") {
		StartCoroutine(shk.Shake(0.4f,0.45f));
		}
		
	}
}

