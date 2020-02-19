using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public int _playerLives, _asteroidShot, _levelNumber;
    public GameObject Player, live1, live2, live3, AsteroidPrefab, AsteroidPrefabSmall;
    public GameObject spawner;
    public Text asteroidCounter, timeCounter, gameOver, finalScore, levelComplete;
    public float playTimeCounter;
    float totalScore;
    public string playerName;
    private bool levelCleared, gameEnded, levelTransition, gameReset;


    void Start()
    {
        gameOver.gameObject.SetActive(false);
        levelComplete.gameObject.SetActive(false);
        PlayerBehaviour.PlayerHit += PlayerDamage;
        MissileController.AsteroidHit += AsteroidHit;

        playTimeCounter = 0;
        _asteroidShot = 0;
        _playerLives = 3;
        _levelNumber = 0;
        timeCounter.text = playTimeCounter.ToString("F2");
        asteroidCounter.text = _asteroidShot.ToString();
        Player = Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);

        SetupLevel();

    }

    private void FixedUpdate()
    {
        if (!gameEnded && !levelCleared)
        {
            Asteroid a = (Asteroid)FindObjectOfType(typeof(Asteroid));
            if (a == null)
            {
                levelCleared = true;
                levelTransition = true;
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
            if(gameReset)
            {
                gameReset = false;
                GameOver();
            }
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
        levelComplete.gameObject.SetActive(true);

        float counter = 0;

        float waitTime = 4;
        while (counter < waitTime)
        {
            counter += Time.deltaTime;

            yield return null;
        }

        levelComplete.gameObject.SetActive(false);
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
        levelCleared = false;
        _levelNumber++;

        // Give player one extra life every second level
        if (_playerLives < 3 && (_levelNumber % 2 == 0))
        {
            _playerLives++;
            UpdateHealthBar();
        }

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
            gameReset = true;
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
        Asteroid[] remainingAsteroids = FindObjectsOfType<Asteroid>();
        Debug.Log(remainingAsteroids.Length + " remaining asteroids");
        if(remainingAsteroids != null)
        {
            foreach(Asteroid a in remainingAsteroids)
            {
                Destroy(a.gameObject);
            }
        }

        DisablePlayerControls();

        totalScore = _asteroidShot / playTimeCounter;
        finalScore.text = "Your final score is: " + totalScore.ToString();
        StartCoroutine(GameOverReset());
    }

    IEnumerator GameOverReset()
    {

        gameOver.gameObject.SetActive(true);
        finalScore.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        
        // TODO setup API POST with score.

        gameOver.gameObject.SetActive(false);
        finalScore.gameObject.SetActive(false);

        ResetGame();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetGame()
    {
        _asteroidShot = 0;
        playTimeCounter = 0;
        _levelNumber = 0;
        _playerLives = 3;
        gameEnded = false;
        levelCleared = false;
        levelTransition = false;
        SetupLevel();
        UpdateHealthBar();
    }

    // Updates visibility and color of the health indicator on top right.
    private void UpdateHealthBar()
    {
        switch (_playerLives)
        {
            case 3:
                live1.GetComponent<SpriteRenderer>().color = Color.white;
                live2.GetComponent<SpriteRenderer>().color = Color.white;
                live3.GetComponent<SpriteRenderer>().color = Color.white;
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
