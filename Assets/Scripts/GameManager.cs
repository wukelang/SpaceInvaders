using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int startingScore = 0;
    [SerializeField] private int startingLives = 3;
    [SerializeField] private int startingWave = 1;

    [SerializeField] private UIManager uiManager;

    public enum GameState { Menu, Playing, Paused, GameOver }
    public GameState currentState { get; private set; }
    public static event Action<GameState> OnGameStateChanged;
    public static event Action OnWaveClear;

    // Gameplay
    private float elapsedTime = 0f;
    public Vector2 playerSpawnLocation = new Vector2(0, -4f);
    public float respawnInvincibilityDuration = 2.0f;
    public EnemyGroupController enemyGroupObject;
    public float enemyGroupSpawnY = 1.0f;
    public MysteryShip shipObject;
    public float shipSpawnFrequency = 15f;
    [SerializeField] private int score;
    [SerializeField] private int lives;
    [SerializeField] private int highScore;
    [SerializeField] private int waveCount;

    public void UpdateGameState(GameState newState)
    {
        currentState = newState;

        OnGameStateChanged?.Invoke(newState);
    }

    void Awake()
    {
        Instance = this;
        InitGame();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= shipSpawnFrequency)
        {
            SpawnSpaceship();
            elapsedTime = 0f;
        }
    }

    void InitGame()
    {
        score = startingScore;
        lives = startingLives;
        currentState = GameState.Playing;
        waveCount = startingWave;
        Time.timeScale = 1f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        InitGame();
    }

    void UpdateUI()
    {
        if (uiManager)
        {
            uiManager.UpdateScore(score);
            uiManager.UpdateLives(lives);
            uiManager.UpdateHighScore(highScore);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        // Debug.Log("GameManager - total score: " + score);
        // StartCoroutine(GameDelay(0.3f));

        UpdateUI();
    }

    public void LoseLife()
    {
        lives -= 1;
        RespawnPlayer();

        UpdateUI();

        if (lives <= 0)
        {
            Debug.Log("RestartGame");
            RestartGame();
        }
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

    
}
