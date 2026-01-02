using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int startingScore = 0;
    [SerializeField] private int startingLives = 3;

    public enum GameState { Menu, Playing, Paused, GameOver }
    public GameState currentState { get; private set; }

    public static event Action<GameState> OnGameStateChanged;
    public Vector2 playerSpawnLocation = new Vector2(0, -4.2f);
    public float respawnInvincibilityDuration = 2.0f;

    // Game Data
    [SerializeField] private int score;
    [SerializeField] private int lives;
    [SerializeField] private int highScore;

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

    }

    void InitGame()
    {
        score = startingScore;
        lives = startingLives;
        currentState = GameState.Playing;
        Time.timeScale = 1f;
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log("GameManager - total score: " + score);
        StartCoroutine(GameDelay(0.3f));
    }

    public void LoseLife()
    {
        lives -= 1;
        Debug.Log("GameManager - lives: " + lives);
        RespawnPlayer();
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

    IEnumerator GameDelay(float duration)
    {
        Debug.Log("game delay - " + duration);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    
}
