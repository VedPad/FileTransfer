using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    public CharacterController cc;
    public Collider2D c2D;
    public float distToGround;
    public float jumpSpeed;
    private Rigidbody2D rb;
    public bool grounded;
    public float moveSpeed;
    public Vector2 moveDir;
	// Use this for initialization
	void Start () {
        distToGround = c2D.bounds.extents.y;
        rb = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		moveDir =  new Vector2(Input.GetAxisRaw("Horizontal"),0);
        if (moveDir.x > 0)
        {
            transform.localScale = new Vector3(5, 5, 5);
            transform.Translate(Vector2.right * moveSpeed);
        }
        else if (moveDir.x < 0)
        {
            transform.localScale = new Vector3(-5, 5, 5);
            transform.Translate(Vector2.left * moveSpeed);
        }

        if (isGrounded())
        {
            grounded = true;
        }else
        {
            grounded = false;
        }

        if(Input.GetKeyDown(KeyCode.W) && isGrounded())
        {
            rb.velocity = new Vector2(0,jumpSpeed);
        }
        

        
	}

    public bool isGrounded()
    {
        RaycastHit2D hit =  Physics2D.Raycast(transform.position, Vector2.down);

        if(hit.collider != null)
        {
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            if(distance < 0.5)
            {
                return true;
            }else
            {
                return false;
            }
        }else
        {
            return false;
        }
    }
}
