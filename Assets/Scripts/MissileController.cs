using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public delegate void AsteroidHitter();
    public static event AsteroidHitter AsteroidHit;
    private float missileSpeed = 10f;
    public Camera mainCamera;
    public ObjectPooler ObjectPooler;
    public Rigidbody2D rb;

    void FixedUpdate()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            ReturnMissileToPool();
        }
    }

    public void OnEnable() => Fire();

    public void Fire()
    {
        Debug.Log("Firing Missile");
        rb.velocity = transform.up * missileSpeed;
        
    }

    private void ReturnMissileToPool()
    {
        ObjectPooler.ReturnGameObject(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If collission is with ship, return
        if(collision.GetComponent<PlayerBehaviour>() != null) return;

        if (collision.GetComponent<Asteroid>() != null)
        {
            collision.GetComponent<Asteroid>().Blast();
            AsteroidHit?.Invoke();
        }

        if (collision.GetComponent<AsteroidSmall>() != null)
        {
            collision.GetComponent<AsteroidSmall>().Blast();
            AsteroidHit?.Invoke();
        }

        ReturnMissileToPool();
    }

}
