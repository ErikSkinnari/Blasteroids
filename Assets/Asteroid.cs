using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private int generation;
    public Rigidbody2D rb;
    
    public Asteroid(int gen)
    {
        this.generation = gen;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Blast()
    {
        // Boom
        Debug.Log("Asteroid destroyed!");
    }
}
