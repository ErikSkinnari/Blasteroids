using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileController : MonoBehaviour
{
    public float missileSpeed;

    public Rigidbody2D rb;

    void Start()
    {
        missileSpeed = 10f;
        rb.velocity = transform.up * missileSpeed;        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        Asteroid asteroid = collision.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            // Destroy asteroid and play some kind of animation.
            asteroid.Blast();
        }
        Destroy(gameObject);
    }

}
