using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounceBetweenWallsBehavior : EnemyBaseBehavior
{
    public LayerMask ground;
    // If this is too low, it has issues bouncing back and forth for some reason
    public float waitTimeSecs = 1f;
    public float movementSpeed = 5;
    public Transform leftCheck;
    public Transform rightCheck;
    public float checkRadius = 0.1f;

    private bool waiting = false;
    private bool pointingLeft = true;      

    // Update is called once per frame
    void Update()
    {
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
