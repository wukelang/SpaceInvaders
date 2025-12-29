using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int startingScore = 0;
    [SerializeField] private int startingLives = 3;

    public enum GameState { Menu, Playing, Paused, GameOver }
    public GameState currentState { get; private set; }

    // Game Data
    [SerializeField] private int score;
    [SerializeField] private int lives;
    [SerializeField] private int highScore;


    void Start()
    {
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
        Debug.Log("GameManager - Points: " + points);
    }

    public void LoseLife()
    {
        lives -= 1;
        Debug.Log("GameManager - lives: " + lives);

    }


    
}
