using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public delegate void AsteroidHitter();
    public static event AsteroidHitter AsteroidHit;
    public float missileSpeed;
    public Camera mainCamera;
    public ObjectPooler ObjectPooler;

    public Rigidbody2D rb;

    void Start()
    {
        missileSpeed = 10f;
    }

    void FixedUpdate()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            ReturnMissileToPool();
        }
    }

    public void OnEnable()
    {
        rb.velocity = transform.up * missileSpeed;
        FindObjectOfType<AudioManager>().Play("shot");
        Debug.Log("vel " + rb.velocity);
    }

    public void OnDisable() 
    {
        Debug.Log("disabling ");
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
        }

        if (collision.GetComponent<AsteroidSmall>() != null)
        {
            collision.GetComponent<AsteroidSmall>().Blast();
        }

        AsteroidHit?.Invoke();

        ReturnMissileToPool();
    }

}
