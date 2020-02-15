using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public int level;

    private void Start()
    {
        level = 1;
        for (int i = 0; i < level * 2 + 5; i++)
        {
            Vector2 randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
            Instantiate(asteroidPrefab, randomPositionOnScreen, Quaternion.identity);
        }
    }


}
