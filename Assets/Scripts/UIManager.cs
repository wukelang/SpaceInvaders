using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public UIDocument uiDocument;

    [Header("Gameplay UI, UI Toolkit")]
    [SerializeField] private Label scoreText;
    [SerializeField] private Label livesText;
    [SerializeField] private Label highScoreText;
    [SerializeField] private Label waveText;

    [Header("Game Over UI, UGUI")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Label finalScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Pause Menu, UGUI")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseMenuButton;


    void Start()
    {
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        livesText = uiDocument.rootVisualElement.Q<Label>("LivesLabel");
        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        waveText = uiDocument.rootVisualElement.Q<Label>("WaveLabel");
    }

    void SetupButtons()
    {
        if (restartButton != null)
            restartButton.clicked += GameManager.Instance.RestartGame;

        if (mainMenuButton != null)
            mainMenuButton.clicked += GameManager.Instance.LoadMainMenu;

        if (resumeButton != null)
            resumeButton.clicked += GameManager.Instance.ResumeGame;

        if (pauseMenuButton != null)
            pauseMenuButton.clicked += GameManager.Instance.LoadMainMenu;
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score\n\n{score}";
        }
    }

    public void UpdateHighScore(int highscore)
    {
        if (highScoreText != null)
        {
            highScoreText.text = $"Hi-Score\n\n{highscore}";
        }
    }

    public void UpdateLives(int lives)
    {
        VisualElement livesContainer = uiDocument.rootVisualElement.Q<VisualElement>("LivesContainer");

        if (livesText != null)
        {
            livesText.text = $"{lives}";
        }

        // Icons
        for (int i = 0; i < livesContainer.childCount; i++)
        {
            VisualElement lifeIcon = livesContainer[i];

            if (i < lives)
            {
                lifeIcon.style.display = DisplayStyle.Flex;
            }
            else
            {
                lifeIcon.style.display = DisplayStyle.None;
            }
        }
    }

    public void UpdateWave(int waveCount)
    {
        if (highScoreText != null)
        {
            waveText.text = $"Wave\n\n{waveCount}";
        }
    }

    public void ShowPauseMenu(bool isTrue)
    {
        if (pauseMenu)
            pauseMenu.SetActive(isTrue);
    }

    public void ShowGameOverScreen(bool isTrue)
    {
        if (gameOverMenu)
            gameOverMenu.SetActive(isTrue);
    }
}
