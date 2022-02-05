using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
	public float speed = 10f;
    // gravity scale is set to 5f
	public float jumpForce = 25f;
	public float moveInput;
    private bool facingRight = true;

    // Components
    private Rigidbody2D rb;
    private BoxCollider2D coll;    

    public float checkRadius = 0.1f;
    public LayerMask whatIsGround;
    // needed as we use same tiles for walls and for ground
    public Transform groundCheck;

    // allow for lag between jump press and jump (if pressed just before landing)
    public float jumpDelaySecs = 0.25f;
    
    private bool isGrounded;
    private float lastJumpTime = -1f;

	public int extraJumps = 0;
	public int currentJumps;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 5.0f;
    public float knockBack = 2f;
   
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
        SetJumpState();
        HorizontalMovement();
    }

    private void SetJumpState()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (isGrounded)
        {
            currentJumps = extraJumps;
        }

        if (Input.GetButtonDown("Jump"))
        {
            lastJumpTime = Time.realtimeSinceStartup;
        }
    }

    void FixedUpdate()
    {       
        VerticalMovement();
    }

    private void HorizontalMovement()
    {
        moveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(Mathf.Round(moveInput * speed), Mathf.Round(rb.velocity.y));
        if (!facingRight && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput < 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // inside FixedUpdate as working with RigidBody and Physics engine
    private void VerticalMovement()
    {
        float timeSinceLastJump = Time.realtimeSinceStartup - lastJumpTime;
        
        // can jump if on ground and jump pressed within small time window
        bool canJump = timeSinceLastJump <= jumpDelaySecs;        

        if (Input.GetButtonDown("Jump") && currentJumps > 0)
        {
            rb.velocity = new Vector2(0, jumpForce); ;
            currentJumps--;
        }
        else if (isGrounded && canJump)
        {
            rb.velocity = new Vector2(0, jumpForce);
            lastJumpTime = Time.realtimeSinceStartup;
        }

        //Better jump logic in Unity(from video)
        // falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // rising
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // if jumping and not holding jump 
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
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
                rb.velocity = new Vector2(knockBack, knockBack);
            }
        }
    }
}
