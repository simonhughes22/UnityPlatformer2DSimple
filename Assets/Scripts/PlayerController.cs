using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public int maxHealth = 5;
    public int currentHealth;

    [SerializeField]
    public float speed = 10f;
    // gravity scale is set to 5f
    [SerializeField]
    public float jumpForce = 25f;

    private float moveInput;
    private bool facingRight = true;

    // Components
    private Rigidbody2D rb;
    private BoxCollider2D coll;   

    // Jump Logic
    [SerializeField]
    public float checkRadius = 0.1f;
    [SerializeField]
    public LayerMask whatIsGround;
    // needed as we use same tiles for walls and for ground
    [SerializeField]
    public Transform groundCheck;

    // allow for lag between jump press and jump (if pressed just before landing)
    [SerializeField]
    public float jumpDelaySecs = 0.25f;
    
    private bool isGrounded;
    private float lastJumpTime = -1f;

    [SerializeField]
    public int extraJumps = 0;
	private int currentJumps;

    [SerializeField]
    public float fallMultiplier = 2.5f;
    [SerializeField]
    public float lowJumpMultiplier = 5.0f;
    [SerializeField]
    public float knockBack = 2f;

    // Sounds
    [SerializeField]
    public AudioClip jumpClip;
    [SerializeField]
    public AudioClip hitClip;

    // Game Logic
    [SerializeField]
    GameOverScreen gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

    	currentJumps = extraJumps;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) {
            return;
        }
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
        if (currentHealth <= 0)
        {          
            return;
        }
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
        //TODO - implement this for even better jumping - https://pressstart.vip/tutorials/2019/10/15/104/perfect-jumping-in-unity.html (includes code)

        float timeSinceLastJump = Time.realtimeSinceStartup - lastJumpTime;
        
        // can jump if on ground and jump pressed within small time window
        bool canJump = timeSinceLastJump <= jumpDelaySecs;        

        if (Input.GetButtonDown("Jump") && currentJumps > 0)
        {
            rb.velocity = new Vector2(0, jumpForce); ;
            currentJumps--;
            GameManager.PlaySound(jumpClip);
        }
        else if (isGrounded && canJump)
        {
            rb.velocity = new Vector2(0, jumpForce);
            lastJumpTime = Time.realtimeSinceStartup;
            GameManager.PlaySound(jumpClip);
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

    private void OnDeath()
    {
        gameOverScreen.Show();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // check if jumping on enemy
            if (transform.position.y > other.transform.position.y + 0.1f)
            { // our center must be above enemy's center                 
                // If ground check overlaps enemy then we are jumping on them
                Collider2D[] hits = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.47f); // player has width 1.0f
                foreach (Collider2D col in hits)
                {
                    if (col.gameObject.Equals(other.gameObject))
                    {
                        OnEnemyJumpOn(other);
                        return;
                    }
                }
            }

            OnHit(other);
        }
        else if (other.gameObject.tag == "Projectile")
        {
            OnHit(other);
        }
    }

    private void OnEnemyJumpOn(Collision2D other)
    {
        rb.velocity = new Vector2(rb.velocity.x, knockBack);
        GameManager.PlaySound(jumpClip);
        
        var enemyController = other.gameObject.GetComponent<EnemyBaseBehavior>();        
        enemyController.Kill();        
    }

    private void OnHit(Collision2D other)
    {
        // to the right
        if (other.transform.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(-knockBack, knockBack);
        }
        // to the left
        else
        {
            rb.velocity = new Vector2(knockBack, knockBack);
        }

        GameManager.PlaySound(hitClip);
        currentHealth -= 1;

        if (currentHealth == 0)
        {
            OnDeath();
        }
    }
}
