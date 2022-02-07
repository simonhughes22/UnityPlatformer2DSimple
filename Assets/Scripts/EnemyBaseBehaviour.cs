using UnityEngine;
using System.Collections;

public class EnemyBaseBehavior : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
