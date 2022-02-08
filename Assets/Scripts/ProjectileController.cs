using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < GameManager.Instance.bottomOfScreenYVal)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Tile") {
            Destroy(gameObject);
        }
    }
}