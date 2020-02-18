using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSmall : Asteroid
{
    private void Awake()
    {
        velocityValue = 80f;
    }

    public override void Blast()
    {
        FindObjectOfType<AudioManager>().Play("hit");
        var explosionAnimation = Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(explosionAnimation, explosionAnimation.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }
}
