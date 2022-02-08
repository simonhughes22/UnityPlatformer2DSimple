using UnityEngine;
using System.Collections;

public class EnemyBaseBehavior : MonoBehaviour
{
    private bool alive = true;
    protected Rigidbody2D rb;
    protected BoxCollider2D coll;

    [SerializeField]
    public float dropSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void Kill() {        
        alive = false;
        coll.isTrigger = true;
        rb.velocity = new Vector2(0, dropSpeed);
    }

    void OnBecameInvisible()
    {
        if (!alive) { 
            Destroy(gameObject);
        }
    }
}
