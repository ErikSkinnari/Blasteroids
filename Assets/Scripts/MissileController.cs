﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public delegate void AsteroidHitter();
    public static event AsteroidHitter AsteroidHit;
    public float missileSpeed;
    public Camera mainCamera;

    public Rigidbody2D rb;

    void Start()
    {
        missileSpeed = 10f;
        rb.velocity = transform.up * missileSpeed;
        FindObjectOfType<AudioManager>().Play("shot");
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

        if (collision.GetComponent<Asteroid>() != null)
        {
            collision.GetComponent<Asteroid>().Blast();
        }

        if (collision.GetComponent<AsteroidSmall>() != null)
        {
            collision.GetComponent<AsteroidSmall>().Blast();
        }

        AsteroidHit?.Invoke();

        Destroy(gameObject);
    }

}
