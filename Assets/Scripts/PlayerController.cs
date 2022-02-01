using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float jumpForce;
	public float moveInput;
    
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpDelaySecs = 0.5f;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
	private bool facingRight = true;
    private bool isGrounded;
    private float lastJumpTime = -1f;

	public int extraJumps;
	private int currentJumps;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;
    public float knockBack = 2f;

    public int hits = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    	currentJumps = extraJumps;
    }

    // Update is called once per frame
    void Update()
    {
    	if(isGrounded){
    		currentJumps = extraJumps;
            lastJumpTime = -1f;
    	}

        float timeSinceLastJump = Time.realtimeSinceStartup - lastJumpTime;
        bool canJump = timeSinceLastJump >= jumpDelaySecs;

        if(Input.GetButtonDown("Jump") && currentJumps > 0 && canJump){
        	//rb.velocity = Vector2.up * jumpForce;

            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            lastJumpTime = Time.realtimeSinceStartup;
        	currentJumps--;

            hits += 1;
            Debug.Log("Jump 1: " + hits);
        }
        else if(isGrounded && Input.GetButtonDown("Jump") && currentJumps == 0 && canJump){
	        //rb.velocity = Vector2.up * jumpForce;

            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            lastJumpTime = Time.realtimeSinceStartup;
            Debug.Log("Jump 2: " + Time.realtimeSinceStartup);
        }


        //Better jump logic in Unity(from video)
        // falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            Debug.Log("Falling: " + Time.realtimeSinceStartup);
        }
        // rising
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            Debug.Log("Not Jumping: " + Time.realtimeSinceStartup);
        }
    }
    
    void FixedUpdate()
    {       	
        isGrounded = coll.IsTouchingLayers(whatIsGround);
    
    	moveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(Mathf.Round(moveInput * speed), Mathf.Round(rb.velocity.y));
    	
    	if(!facingRight && moveInput > 0){    	
    		Flip();
    	}
    	else if(facingRight && moveInput < 0){
    		Flip();
    	}
    }
    
    void Flip(){
    	facingRight = !facingRight;
    	Vector3 scaler = transform.localScale;
    	scaler.x *= -1;
    	transform.localScale = scaler;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // to the right
            if (other.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(-knockBack, knockBack);
            }
            else
            {
                rb.velocity = new Vector2(-knockBack, knockBack);
            }
        }
    }
}
