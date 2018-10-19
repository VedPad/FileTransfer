using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour {
	public float moveSpeed;
	public float repelRangeGrav;
	public int health;
    public int maxHealth = 10;
	public GameObject[] gravities;
	public GameObject explosionDeath;
	public float turnSpeed = .1f;
	public float shootTimer;
	public GameObject projectile;
	public Vector2 newPosTotal;
    public float explosionForce;
    public bool ExplodePlayer;
	public bool Force;
	public float rateOfFire;
	public enemyTracker eT;
	public float shootDistance;
	public bool gravF;
    private float explodeDistance;
	private Vector2 direction1;
	public CamerShake shk;
	public bool isKamikaze;
	public float repelRange;
	public bool chasing = true;
	public float repelAmount;
	public float repelAmountGrav;
    public bool isExploder;
    public bool exploded;
	public bool isLandMine;
	public GameObject landMine;
    public float projectileAmount;
	public bool isShooter;
	public float strafeSpeed;
	public GameObject player;
	private Rigidbody2D rb;
	private Vector3 velocity;
	// Use this for initialization
	void Start () {
		//eT = GameObject.Find ("TheEnemyBrain").GetComponent<enemyTracker> ();
		//player = GameObject.Find ("SpaceShip");
		rb = this.GetComponent<Rigidbody2D> ();
		gravities = GameObject.FindGameObjectsWithTag ("gravity");
		eT.EnemyRBs.Add (rb);
		eT.arrayLength += 1;
		explodeDistance = Random.Range (2.5f, 7f);
	}

	void OnDestroy(){
		eT.EnemyRBs.Remove (rb);
        SpaceShipMoveMent smm = player.GetComponent<SpaceShipMoveMent>();
        
		eT.arrayLength -= 1;
		GameObject explosion = GameObject.Instantiate (explosionDeath, this.transform.position, this.transform.rotation);
		Destroy (explosion,3f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float distance = Vector2.Distance (rb.position, player.transform.position);
		shootTimer++;
		direction1 = (new Vector2(player.transform.position.x,player.transform.position.y) - rb.position).normalized;
        if (isExploder)
        {
            if(distance > explodeDistance)
            {
                float angle = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg - 90f;
                rb.rotation = Mathf.LerpAngle(rb.rotation, angle, turnSpeed);
                newPosTotal = moveRegular(direction1);
                rb.MovePosition(newPosTotal);
            }else
            {
                Explode();
                
            }
            
        }

		if (isLandMine) {
			if(distance > explodeDistance)
			{
				float angle = Mathf.Atan2(direction1.y, direction1.x) * Mathf.Rad2Deg - 90f;
				rb.rotation = Mathf.LerpAngle(rb.rotation, angle, turnSpeed);
				newPosTotal = moveRegular(direction1);
				rb.MovePosition(newPosTotal);
			}else
			{
				doLandMine ();

			}
		}
            
		if (chasing == true) {
			if (isShooter) {
				
				float angle = Mathf.Atan2 (direction1.y, direction1.x) * Mathf.Rad2Deg - 90f;
				rb.rotation = angle;

					if (distance > shootDistance ) {
						newPosTotal = moveRegular (direction1);
						Force = false;
					} else if (distance <= (shootDistance)) {
						newPosTotal = moveStrafing (direction1);

						Force = true;
					}

				if (distance <= shootDistance) {
					Shoot ();
				}
				if (Force == true) {
					newPosTotal -= rb.position;
					rb.AddForce (newPosTotal, ForceMode2D.Force);
				} else {
					rb.MovePosition (newPosTotal);
				}
					
				

			}
			if (isKamikaze) {
				float angle = Mathf.Atan2 (direction1.y, direction1.x) * Mathf.Rad2Deg - 90f;
				rb.rotation = Mathf.LerpAngle (rb.rotation, angle, turnSpeed);
				newPosTotal = moveRegular(direction1);
				rb.MovePosition (newPosTotal);
			}

		}
		if (health <= 0) {
			
			Destroy (this.gameObject);

		}
			
	}

    public void Explode()
    {
        float bulletAngle = 0;
        float increment = (2 * Mathf.PI) / projectileAmount;
        for (int i = 0; i < projectileAmount; i++)
        {
            float x = Mathf.Cos(bulletAngle) * 1;
            float y = Mathf.Sin(bulletAngle) * 1;
            GameObject bul = GameObject.Instantiate(projectile, new Vector3(transform.position.x + x, transform.position.y + y, -5), Quaternion.Euler(0,0,bulletAngle));
            Rigidbody2D bulRB = bul.GetComponent<Rigidbody2D>();
            if (ExplodePlayer)
            {
                enemyAI buleAI = bul.GetComponent<enemyAI>();
                buleAI.eT = eT;
                buleAI.player = player;
            }
            bulRB.AddForce(new Vector2(x * explosionForce, y * explosionForce));
            bulletAngle += increment;
            if (!ExplodePlayer)
            {
                Destroy(bul, 20f);
            }
        }
        Destroy(this.gameObject);
    }

	public void doLandMine(){
		GameObject mine = GameObject.Instantiate (landMine, new Vector3(this.transform.position.x,this.transform.position.y,5), this.transform.rotation);
		LandMineScript lMS = mine.GetComponent<LandMineScript> ();
		lMS.eT = eT;
		lMS.smm = player.GetComponent<SpaceShipMoveMent> ();
		lMS.player = player;
		Destroy (mine, 30f);
		StartCoroutine(shk.Shake(0.25f,0.35f));
		Destroy (this.gameObject);

	}

	public void Shoot(){
		if (shootTimer >= 60/rateOfFire) {
			
			GameObject bullet = GameObject.Instantiate (projectile, this.transform.position, this.transform.rotation);
			bulletScript bScript = bullet.GetComponent<bulletScript> ();
			bScript.direction = new Vector2 (direction1.x, direction1.y);
			shootTimer = 0;
		}
	}

	Vector2 moveStrafing(Vector2 direction){
		Vector2 newPos = transform.position + transform.right * Time.deltaTime * strafeSpeed;
		return newPos;
	}

	Vector2 moveRegular(Vector2 direction){
		bool gRepelled = false;
		Vector2 repelForce = Vector2.zero;
		Vector2 repelForce2 = Vector2.zero;
		foreach (Rigidbody2D enemy in eT.EnemyRBs) {
			if (enemy == rb) {
				continue;
			}
			if (Vector2.Distance (enemy.position, rb.position) <= repelRange) {
				Vector2 repelDir = (rb.position - enemy.position).normalized;
				repelForce += repelDir;
			}
		}
		if (gRepelled == false) {
			foreach (GameObject gravity1 in gravities) {
				gravitationalField gF = gravity1.GetComponent<gravitationalField> ();
				if (Vector2.Distance (rb.position, new Vector2 (gravity1.transform.position.x, gravity1.transform.position.y)) <= Vector2.Distance (new Vector2 (gravity1.transform.position.x, gravity1.transform.position.y), new Vector2 (gF.radiusFinder.transform.position.x, gF.radiusFinder.transform.position.y)) + repelRangeGrav) {
					gravF = true;
					Vector2 repelDir = (rb.position - new Vector2 (gravity1.transform.position.x, gravity1.transform.position.y)).normalized;
					repelForce2 += repelDir;
					gRepelled = true;
				} else {
					gravF = false;
				}
			}
		}


		Vector2 newPos = transform.position + transform.up * Time.deltaTime * moveSpeed;
		newPos += repelForce * Time.deltaTime * repelAmount;
		newPos += repelForce2 * Time.deltaTime * repelAmountGrav;
		if (gravF == true) {
			newPos -= (new Vector2(transform.position.x,transform.position.y) + new Vector2(transform.up.x,transform.up.y) * Time.deltaTime * moveSpeed);
		}
		return newPos;
	}


}
