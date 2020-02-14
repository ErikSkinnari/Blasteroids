using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private int generation;
    public Rigidbody2D rb;
    public GameObject AsteroidSmallPrefab;
    float smallSpeed = 20f;
    
    // Update is called once per frame
    void Update()
    {

    }

    public void Blast()
    {
        // Boom
        Debug.Log("Asteroid destroyed!");
        for (int i = 0; i < 3; i++)
        {
            GameObject smallAsteroid = Instantiate(AsteroidSmallPrefab, gameObject.transform.position, gameObject.transform.rotation);
            smallAsteroid.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(Random.Range(-smallSpeed, smallSpeed), Random.Range(-smallSpeed, smallSpeed)));
        }
        
        Destroy(gameObject);
    }
}
