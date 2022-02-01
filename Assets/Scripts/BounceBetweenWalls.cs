using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBetweenWalls : MonoBehaviour
{
    public LayerMask ground;
    public float waitTimeSecs = 1;
    public float movementSpeed = 5;
    public Transform leftCheck;
    public Transform rightCheck;
    public float checkRadius = 0.5f;

    private bool waiting = false;
    private bool pointingLeft = true;
    private BoxCollider2D coll;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // touching left wall
        if (Physics2D.OverlapCircle(leftCheck.position, checkRadius, ground))
        {
            if (!waiting)
            {
                waiting = true;
                StartCoroutine(WaitThenTurn());
            }
        }
        else if (Physics2D.OverlapCircle(rightCheck.position, checkRadius, ground))
        {
            if (!waiting)
            {
                waiting = true;
                StartCoroutine(WaitThenTurn());
            }
        }
        else
        {
            Move();
        }        
    }

    private void Move()
    {
        if (pointingLeft)
        {
            rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        }
    }

    IEnumerator WaitThenTurn()
    {
        yield return new WaitForSeconds(waitTimeSecs);
        // reverse direction
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        pointingLeft = !pointingLeft;
        Move();
        waiting = false;
    }
}
