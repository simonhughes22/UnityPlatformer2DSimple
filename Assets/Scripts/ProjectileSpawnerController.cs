using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawnerController: MonoBehaviour
{
    [SerializeField]
    public float[] spawnSchedule;

    [SerializeField]
    public GameObject projectile;

    [SerializeField]
    public float verticalSpeed = 20f;

    [SerializeField]
    public float horizontalSpeed = 10f;

    private int currentIndex = -1;
    private bool waiting = false;
    private bool facingRight = true;
    private Vector2 lastPosition = Vector2.zero;

    private void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float diff = transform.position.x - lastPosition.x;
        if (diff > 0)
        {
            facingRight = true;
        }
        else if (diff < 0)
        {
            facingRight = false;
        }
        if(!waiting)
        {
            StartCoroutine(WaitThenFire());
        }
        lastPosition = transform.position;
    }

    IEnumerator WaitThenFire()
    {
        waiting = true;
        currentIndex++;
        if (currentIndex >= spawnSchedule.Length)
        {
            currentIndex = 0;
        }
        yield return new WaitForSeconds(spawnSchedule[currentIndex]);

        GameObject projectileObject = Instantiate(projectile, transform.position, Quaternion.identity);
        var projRb = projectileObject.GetComponent<Rigidbody2D>();
        if (facingRight)
        {
            projRb.velocity = new Vector2(horizontalSpeed, verticalSpeed);
        }
        else
        {
            projRb.velocity = new Vector2(-horizontalSpeed, verticalSpeed);
        }
        waiting = false;
    }
}
