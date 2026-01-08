using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int startingScore = 0;
    [SerializeField] private int startingLives = 3;
    [SerializeField] private int startingWave = 0;

    [SerializeField] private UIManager uiManager;

    public enum GameState { Menu, Playing, Paused, GameOver }
    public GameState currentState { get; private set; }
    private const string HIGH_SCORE_KEY = "HighScore";

    // Gameplay
    private float elapsedTime = 0f;
    // public PlayerController playerObject;
    public Vector2 playerSpawnLocation = new Vector2(0, -4f);

    public float respawnInvincibilityDuration = 2.0f;
    public EnemyGroupController enemyGroupObject;
    public float enemyGroupSpawnY = 1.0f;
    private EnemyGroupController currentWaveEnemyGroup;
    public MysteryShip shipObject;
    public float shipSpawnFrequency = 20f;
    public int score;
    public int lives;
    public int waveCount;
    private int highScore;
    private int savedHighScore;

    void Awake()
    {
        Instance = this;
        InitGame();

        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        savedHighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= shipSpawnFrequency)
        {
            SpawnSpaceship();
            elapsedTime = 0f;
        }

        if (currentWaveEnemyGroup == null)
        {
            // TODO: display wave number before spawning?
            SpawnWave();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
            {
                PauseGame();
            }
            else if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
        }
    }

    void SpawnWave()
    {
        // if (waveCount > 0)
        // {
        //     score += 100;
        // }

        float ySpawnOffset = waveCount * 0.25f;
        if (ySpawnOffset < -2) 
            ySpawnOffset = -2;
        Vector2 spawnPos = new Vector2(0, enemyGroupSpawnY - ySpawnOffset);
        currentWaveEnemyGroup = Instantiate(enemyGroupObject, spawnPos, transform.rotation);
        waveCount += 1;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (uiManager)
        {
            uiManager.UpdateScore(score);
            uiManager.UpdateLives(lives);
            uiManager.UpdateHighScore(highScore);
            uiManager.UpdateWave(waveCount);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        // Debug.Log("GameManager - total score: " + score);
        // StartCoroutine(GameDelay(0.3f));
        if (score > highScore)
        {
            highScore = score;
        }

        UpdateUI();
    }

    public void LoseLife()
    {
        lives -= 1;

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            RespawnPlayer();   
        }

        UpdateUI();
    }

    void RespawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            player.transform.position = playerSpawnLocation;
            PlayerController playerScript = player.GetComponent<PlayerController>();
            if (playerScript)
            {
                playerScript.EnableInvincibility(respawnInvincibilityDuration);
            }
        }
    }

    void SpawnSpaceship()
    {
        Instantiate(shipObject);
    }

    IEnumerator GameDelay(float duration)
    {
        Debug.Log("game delay - " + duration);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    // Game State
    // public void UpdateGameState(GameState newState)
    // {
    //     currentState = newState;

    //     OnGameStateChanged?.Invoke(newState);
    // }

    void InitGame()
    {
        score = startingScore;
        lives = startingLives;
        currentState = GameState.Playing;
        waveCount = startingWave;
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        InitGame();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0f;
        uiManager.ShowPauseMenu(true);
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        uiManager.ShowPauseMenu(false);
    }

    public void GameOver()
    {
        lives = 0;
        UpdateUI();

        if (score > savedHighScore)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
            PlayerPrefs.Save();
        }

        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        uiManager.ShowGameOverScreen(true);
    }
}
