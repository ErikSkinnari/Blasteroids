using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Wrappable
{
    public Rigidbody2D rb;
    public GameObject AsteroidSmallPrefab;
    readonly float velocityValue = 30f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(new Vector2(Random.Range(-velocityValue, velocityValue), Random.Range(-velocityValue, velocityValue)));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = WrappingBehaviour.WrappingUpdate(this);
    }

    public void Blast()
    {
        // Boom
        Debug.Log("Asteroid destroyed!");
        for (int i = 0; i < 3; i++)
        {
            Instantiate(AsteroidSmallPrefab, gameObject.transform.position, gameObject.transform.rotation);            
        }
        
        Destroy(gameObject);
    }
}
