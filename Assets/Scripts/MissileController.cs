using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float missileSpeed;
    public Camera mainCamera;

    public Rigidbody2D rb;

    void Start()
    {
        missileSpeed = 10f;
        rb.velocity = transform.up * missileSpeed;        
    }

    void Update()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If collission is with ship, return
        if(collision.GetComponent<PlayerBehaviour>() != null) return;

        Debug.Log(collision.name);
        Asteroid asteroid = collision.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            // Destroy asteroid and play some kind of animation.
            // TODO Animation
            asteroid.Blast();
        }
        
        Destroy(gameObject);
    }

}
