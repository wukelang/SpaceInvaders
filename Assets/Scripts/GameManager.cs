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
        Time.timeScale = 0f;
        GameDelay(1f);
        Time.timeScale = 1f;
    }

    public void LoseLife()
    {
        lives -= 1;
        // Debug.Log("GameManager - lives: " + lives);
        // Time.timeScale = 0f;
    }

    IEnumerator GameDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    
}
