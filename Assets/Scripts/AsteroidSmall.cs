using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSmall : MonoBehaviour
{
    private int generation;
    public Rigidbody2D rb;
    float rotation;
    float rotationSpeedMin = -10f;
    float rotationSpeedMax = 10f;

    private void Start()
    {
        rotation = Random.Range(rotationSpeedMin, rotationSpeedMax);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotation * Time.deltaTime, Space.Self);
    }

    public void Blast()
    {
        Destroy(gameObject);
    }
}
