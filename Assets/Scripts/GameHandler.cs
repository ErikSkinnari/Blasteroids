using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public int _playerLives, _asteroidShot, _levelNumber;
    public GameObject Player, live1, live2, live3, AsteroidPrefab, AsteroidPrefabSmall;
    public GameObject spawner;
    public Text asteroidCounter, timeCounter, gameOver, levelComplete;
    public float playTimeCounter;
    public string playerName;
    private bool levelCleared, gameEnded, levelTransition;

    private void OnEnable()
    {
        gameOver.gameObject.SetActive(false);
        levelComplete.gameObject.SetActive(false);
        PlayerBehaviour.PlayerHit += PlayerDamage;
        MissileController.AsteroidHit += AsteroidHit;
    }

    void Start()
    {
        playTimeCounter = 0;
        _asteroidShot = 0;
        _playerLives = 3;
        _levelNumber = 0;
        timeCounter.text = playTimeCounter.ToString("F2");
        Player = Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);
        Debug.Log("sTARTUP METHOD");
        SetupLevel();
        Debug.Log("Game setup");
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!gameEnded && !levelCleared)
        {
            Asteroid a = (Asteroid)FindObjectOfType(typeof(Asteroid));
            if (a)
            {
                Debug.Log("Asteroid object found: " + a.name);
            }
            else
            {
                levelCleared = true;
                levelTransition = true;
                Debug.Log("No Asteroid object could be found");
            }
            playTimeCounter += Time.deltaTime;
            TimerUpdate();
        }

        if(levelCleared)
        {
            if(levelTransition)
            {
                levelTransition = false;
                LevelComplete();
            }
        }

        if(gameEnded)
        {
            GameOver();
        }
    }

    // Disable controls, show message and then start next level
    void LevelComplete()
    {
        DisablePlayerControls();
        StartCoroutine(CompleteMessage());             
    }

    IEnumerator CompleteMessage()
    {
        levelComplete.text = "Level " + _levelNumber + " Complete!";
        levelComplete.enabled = true;

        float counter = 0;

        float waitTime = 4;
        while (counter < waitTime)
        {
            Debug.Log("Waiting! " + counter);
            counter += Time.deltaTime;

            yield return null;
        }

        levelComplete.enabled = false;
        SetupLevel();
    }


    // Enable and disable playercontrolls. Used between levels and on GameOver.
    private void EnablePlayerControls()
    {
        Player.GetComponent<PlayerBehaviour>().enabled = true;
    }
    
    private void DisablePlayerControls()
    {
        Player.GetComponent<PlayerBehaviour>().enabled = false;
    }    

    // Increase level number and give player one more life(max 3), spawn asteroids andflag level as not cleared.
    private void SetupLevel()
    {
        Debug.Log("SetupLevel method");
        Debug.Break();
        levelCleared = false;
        _levelNumber++;

        if (_playerLives < 3) 
        {
            _playerLives++;
        }
        Debug.Log("About to spawn asteroids...");
        Debug.Break();
        spawner.GetComponent<AsteroidSpawner>().SpawnAsteroids(_levelNumber);
        EnablePlayerControls();
    }

    // Player got hit my an asteroid
    void PlayerDamage()
    {
        _playerLives--;
        FindObjectOfType<AudioManager>().Play("damage");
        UpdateHealthBar();
        if(_playerLives <= 0)
        {
            gameEnded = true;
        }
    }

    // Update when Player his an asteroid
    void AsteroidHit()
    {
        _asteroidShot++;
        UpdateScores();
    }

    // Update timer value on UI
    private void TimerUpdate()
    {
        timeCounter.text = playTimeCounter.ToString("F2");
    }
    // Update the score value on UI
    private void UpdateScores()
    {
        asteroidCounter.text = _asteroidShot.ToString();
    }

    private void GameOver()
    {        
        DisablePlayerControls();
        StartCoroutine(GameOverReset());
    }

    IEnumerator GameOverReset()
    {

        gameOver.enabled = true;

        yield return new WaitForSeconds(5f);

        float totalScore = _asteroidShot / playTimeCounter;
        // TODO setup API POST with score.

        gameOver.enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Updates visibility and color of the health indicator on top right.
    private void UpdateHealthBar()
    {
        switch (_playerLives)
        {
            case 3:
                live1.gameObject.SetActive(true);
                live2.gameObject.SetActive(true);
                live3.gameObject.SetActive(true);
                break;

            case 2:
                live1.GetComponent<SpriteRenderer>().color = Color.yellow;
                live1.gameObject.SetActive(true);
                live2.GetComponent<SpriteRenderer>().color = Color.yellow;
                live2.gameObject.SetActive(true);
                live3.gameObject.SetActive(false);
                FindObjectOfType<AudioManager>().Play("warning");
                break;

            case 1:
                live1.GetComponent<SpriteRenderer>().color = Color.red;
                live1.gameObject.SetActive(true);
                live2.gameObject.SetActive(false);
                live3.gameObject.SetActive(false);
                FindObjectOfType<AudioManager>().Play("critical");
                break;

            case 0:
                live1.gameObject.SetActive(false);
                live2.gameObject.SetActive(false);
                live3.gameObject.SetActive(false);
                break;
        }
    }
}
