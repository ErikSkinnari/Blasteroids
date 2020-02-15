using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSmall : Wrappable
{
    private int generation;
    public Rigidbody2D rb;
    float rotation;
    float rotationSpeedMin = -10f;
    float rotationSpeedMax = 10f;
    readonly float velocityValue = 40f;

    private void Start()
    {
        rotation = Random.Range(rotationSpeedMin, rotationSpeedMax);
        //gameObject.GetComponent<Rigidbody2D>()
        rb.AddRelativeForce(new Vector2(Random.Range(-velocityValue, velocityValue), Random.Range(-velocityValue, velocityValue)));
        //camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotation * Time.deltaTime, Space.Self);
        transform.position = WrappingBehaviour.WrappingUpdate(this);
    }

    public void Blast()
    {
        Destroy(gameObject);
    }
}
