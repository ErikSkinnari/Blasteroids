using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Wrappable
{
    public Rigidbody2D rb;
    public GameObject AsteroidSmallPrefab, Explosion;
    internal float rotation;
    internal float rotationSpeed = 10f;
    internal float velocityValue = 30f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rotation = Random.Range(-rotationSpeed, rotationSpeed);
        rb.AddRelativeForce(new Vector2(Random.Range(-velocityValue, velocityValue), Random.Range(-velocityValue, velocityValue)));
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotation * Time.deltaTime, Space.Self);
        transform.position = WrappingBehaviour.WrappingUpdate(this);
    }

    public virtual void Blast()
    {
        int randomNumber = Random.Range(1, 5);
        for (int i = 0; i < randomNumber; i++)
        {
            Instantiate(AsteroidSmallPrefab, gameObject.transform.position, gameObject.transform.rotation);            
        }

        FindObjectOfType<AudioManager>().Play("hit");

        var explosionAnimation = Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(explosionAnimation, explosionAnimation.GetComponent<ParticleSystem>().main.duration);

        Destroy(gameObject);
    }
}
