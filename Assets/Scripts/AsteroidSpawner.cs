using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject _asteroidPrefab;
    public GameObject _smallAsteroidPrefab;
    int numberOfAsteroids;

    public void SpawnAsteroids(int level)
    {
        numberOfAsteroids = level * 2 + 5;
        Debug.Log("Spawn Asteroids. Level #:" + level);
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            Vector2 randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
            Instantiate(_asteroidPrefab, randomPositionOnScreen, Quaternion.identity);
        }

        int smallAsteroids = Random.Range(0, level * 2);

        for (int i = 0; i < smallAsteroids; i++)
        {
            Vector2 randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
            Instantiate(_smallAsteroidPrefab, randomPositionOnScreen, Quaternion.identity);
        }
    }
}
