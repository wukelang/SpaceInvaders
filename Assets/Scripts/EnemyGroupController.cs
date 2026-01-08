using System;
using UnityEngine;

public class EnemyGroupController : MonoBehaviour
{
    // Make this a static instance?
    [SerializeField] private int direction = 1;  // 1 = Right, -1 = Left
    [SerializeField] private float moveInterval = 0.8f;
    [SerializeField] private float baseMoveInterval = 0.8f;
    [SerializeField] private float moveDistance = 0.15f;
    [SerializeField] private float downDistance = 0.25f;
    private float timePassed = 0f;
    [SerializeField] private float leftBoundary;
    [SerializeField] private float rightBoundary;
    [SerializeField] private float groupLeftX;
    [SerializeField] private float groupRightX;
    [SerializeField] private float spriteHeight;
    [SerializeField] private float spriteWidth = 0.6f;

    public Invader[] prefabs; 
    public int rows = 5;
    public float rowDistance = 0.75f;
    public int columns = 11;
    public float columnDistance = 0.85f;
    private int numberOfEnemies;
    public event Action OnGroupMove;
    private AudioSource audioSource;
    public AudioClip[] tones;
    private bool tonePlayed = false;
    [SerializeField] private int currentToneIndex = 0;

    void Start()
    {
        GenerateEnemies();

        Camera cam = Camera.main;
        float screenHalfWidth = cam.aspect * cam.orthographicSize;
        leftBoundary = -screenHalfWidth;
        rightBoundary = screenHalfWidth;

        audioSource = GetComponent<AudioSource>();
    }

    void GenerateEnemies()
    {
        numberOfEnemies = rows * columns;
        float width = columnDistance * (columns - 1);
        float height = rowDistance * (rows - 1);
        Vector2 center = new Vector2(-width / 2, -height / 2);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float xPointer = col * columnDistance;
                float yPointer = row * rowDistance;
                Vector2 position = new Vector2(center.x + xPointer, center.y + yPointer);
                Invader instance = Instantiate(prefabs[row], transform);
                instance.transform.localPosition = position;  // Use parent object as "world"
                // Debug.Log($"enemy generated at {xPointer},{yPointer}");
            }
        }
    }

    void Update()
    {
        float currentEnemyCount = transform.childCount;
        float enemyKilledRatio = currentEnemyCount / numberOfEnemies;
        moveInterval = baseMoveInterval * enemyKilledRatio;
        // Debug.Log($"enemy ratio {currentEnemyCount} / {numberOfEnemies}, {moveInterval}");

        timePassed += Time.deltaTime;

        if (timePassed >= moveInterval)
        {
            timePassed = 0f;
            MoveRoutine();
        }

        if (currentEnemyCount == 0)
        {
            Destroy(gameObject);
        }
    }

    bool CheckBoundaryCollision()
    {
        // Check if any child objects reached boundary
        float leftmost = float.MaxValue;
        float rightMost = float.MinValue;

        foreach (Transform enemy in transform)
        {
            if (enemy.gameObject.activeSelf)
            {
                float enemyX = enemy.position.x;

                SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
                float halfWidth = sr != null ? sr.bounds.extents.x : spriteWidth;
                float enemyLeft = enemyX - halfWidth;
                float enemyRight = enemyX + halfWidth;
                leftmost = Math.Min(leftmost, enemyLeft);
                rightMost = Math.Max(rightMost, enemyRight);
            }
        }
        if (direction > 0)
        {
            return rightMost >= rightBoundary - 0.2f;
        }
        else
        {
            return leftmost <= leftBoundary + 0.2f;
        }
    }

    void MoveRoutine()
    {
        Vector2 currentPos = transform.position;
        currentPos.x += moveDistance * direction;

        bool boundaryReached = CheckBoundaryCollision();
        if (boundaryReached)
        {
            // Debug.Log("Boundary Reached");

            direction *= -1;
            currentPos.x += moveDistance * direction;
            currentPos.y -= downDistance;
        }

        transform.position = currentPos;

        OnGroupMove?.Invoke();  // Animate individual enemy sprite
        // Debug.Log($"{tones.Length}, {currentToneIndex}, {tones[currentToneIndex].name}");
        audioSource.clip = tones[currentToneIndex];

        if (tonePlayed)  // Reduce sfx freq by half
        {
            audioSource.Play();
            currentToneIndex = (currentToneIndex + 1) % tones.Length;
        }
        tonePlayed = !tonePlayed;

    }

}
