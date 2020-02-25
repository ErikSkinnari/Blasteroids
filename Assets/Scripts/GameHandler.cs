using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    private int playerLives, asteroidShot, levelNumber, missileCount;
    public GameObject Player, live1, live2, live3, AsteroidPrefab, AsteroidPrefabSmall, spawner;
    private HighscoreHandler ScoreHandler;
    private GameObject _player;
    public Text asteroidCounter, timeCounter, gameOver, finalScore, levelComplete, levelNumberText;
    public float playTimeCounter;
    float totalScore, skillLevel;
    public string playerName;
    private bool levelCleared, gameEnded, levelTransition, gameReset;

    void Awake()
    {
        ScoreHandler = gameObject.AddComponent<HighscoreHandler>();

        // Some dummy high scores.
        ScoreHandler.AddDummyScores();
    }


    void Start()
    {
        gameOver.gameObject.SetActive(false);
        levelComplete.gameObject.SetActive(false);
        PlayerBehaviour.PlayerHit += PlayerDamage;
        PlayerBehaviour.MissileFired += MissileCounter;
        MissileController.AsteroidHit += AsteroidHit;
        timeCounter.text = playTimeCounter.ToString("F2");
        asteroidCounter.text = asteroidShot.ToString();
        _player = Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);

        StartNewGame();
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

            playTimeCounter += Time.fixedDeltaTime;
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
        _player.GetComponent<PlayerBehaviour>().Dissolve();
        DisablePlayerControls();
        StartCoroutine(CompleteMessage());             
    }

    IEnumerator CompleteMessage()
    {
        levelComplete.text = "Level " + levelNumber + " Complete!";
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
        _player.GetComponent<PlayerBehaviour>().MovementEnabled(true);
    }
    
    private void DisablePlayerControls()
    {
        _player.GetComponent<PlayerBehaviour>().MovementEnabled(false);
    }    

    // Increase level number and give player one more life(max 3), spawn asteroids andflag level as not cleared.
    private void SetupLevel()
    {
        Debug.Log("SetupLevel");
        levelCleared = false;
        levelNumber++;
        levelNumberText.text = levelNumber.ToString();


        _player.GetComponent<PlayerBehaviour>().ResetPosition();
        _player.GetComponent<PlayerBehaviour>().MakeVisible();

        // Give player one extra life every fourth level
        if (playerLives < 3 && (levelNumber % 4 == 0))
        {
            playerLives++;
            UpdateHealthBar();
        }

        spawner.GetComponent<AsteroidSpawner>().SpawnAsteroids(levelNumber);
        EnablePlayerControls();
    }

    // Player got hit my an asteroid
    void PlayerDamage()
    {
        playerLives--;
        FindObjectOfType<AudioManager>().Play("damage");
        UpdateHealthBar();
        if(playerLives <= 0)
        {
            gameEnded = true;
            gameReset = true;
        }
    }

    // Update when Player his an asteroid
    void AsteroidHit()
    {
        asteroidShot++;
        UpdateScores();
    }

    void MissileCounter()
    {
        missileCount++;
    }

    // Update timer value on UI
    private void TimerUpdate()
    {
        timeCounter.text = playTimeCounter.ToString("F2");
    }
    // Update the score value on UI
    private void UpdateScores()
    {
        asteroidCounter.text = asteroidShot.ToString();
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

        _player.GetComponent<PlayerBehaviour>().Dissolve();
        DisablePlayerControls();

        totalScore = asteroidShot / playTimeCounter * 100;
        finalScore.text = "Your final score is: " + totalScore.ToString("F2");
        StartCoroutine(GameOverReset());
    }

    IEnumerator GameOverReset()
    {
        gameOver.gameObject.SetActive(true);
        finalScore.gameObject.SetActive(true);
        skillLevel = totalScore / missileCount;

        Highscore h = new Highscore("Housepainter", totalScore, asteroidShot, missileCount, levelNumber, skillLevel);
        ScoreHandler.SendHighscore(h);

        yield return new WaitForSeconds(5f);

        gameOver.gameObject.SetActive(false);
        finalScore.gameObject.SetActive(false);

        StartNewGame();
    }

    // Reset everything and start game from begining.
    private void StartNewGame()
    {
        asteroidShot = 0;
        playTimeCounter = 0;
        levelNumber = 0;
        playerLives = 3;
        gameEnded = false;
        levelCleared = false;
        levelTransition = false;
        FindObjectOfType<AudioManager>().Stop("damage");
        FindObjectOfType<AudioManager>().Stop("warning");
        SetupLevel();
        UpdateHealthBar();
        UpdateScores();
    }

    // Updates visibility and color of the health indicator on top right.
    private void UpdateHealthBar()
    {
        switch (playerLives)
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
