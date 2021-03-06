using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolBehavior : EnemyBaseBehavior
{
    [SerializeField]
    public float speed;
    [SerializeField]
    public Transform[] patrolPoints;
    [SerializeField]
    public float waitTimeSecs;
    [SerializeField]
    public float minDistance = 1;     

    private int currentPointIndex = 0;
    private bool waiting = false;   

    // Update is called once per frame
    void Update()
    {        
        var distance = Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position);
        if (distance >= minDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, speed * Time.deltaTime);
        }
        else
        {
            if (!waiting) {
                waiting = true;
                StartCoroutine(Wait());
            }            
        }
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTimeSecs);
        currentPointIndex++;
        if (currentPointIndex >= patrolPoints.Length) {
            currentPointIndex = 0;
        }
        waiting = false;
    }
    
}
