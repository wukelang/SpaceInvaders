using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public UIDocument uiDocument;

    [Header("Gameplay UI")]
    [SerializeField] private Label scoreText;
    [SerializeField] private Label livesText;
    [SerializeField] private Label highScoreText;

    [Header("Game Over UI")]
    [SerializeField] private Label gameOverText;
    [SerializeField] private Label finalScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseMenuButton;


    void Start()
    {
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        livesText = uiDocument.rootVisualElement.Q<Label>("LivesLabel");
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
            } else
            {
                lifeIcon.style.display = DisplayStyle.None;
            }
        }
    }


}
