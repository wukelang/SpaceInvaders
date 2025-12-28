using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int direction = 1;  // 1 = Right, -1 = Left
    [SerializeField] private float moveInterval = 0.5f;
    [SerializeField] private float moveDistance = 0.1f;
    [SerializeField] private float downDistance = 0.25f;
    private float timePassed = 0f;
    [SerializeField] private float leftBoundary;
    [SerializeField] private float rightBoundary;
    [SerializeField] private float groupLeftX;
    [SerializeField] private float groupRightX;
    [SerializeField] private float spriteHeight;
    [SerializeField] private float spriteWidth = 0.6f;

    public GameObject enemyObj;
    public GameObject enemyShooterObj;
    public int enemyRows = 5;
    public float rowDistance = 0.8f;
    public int enemyColumns = 12;
    public float columnDistance = 0.8f;

    void Start()
    {
        GenerateEnemies();

        Camera cam = Camera.main;
        float screenHalfWidth = cam.aspect * cam.orthographicSize;
        leftBoundary = -screenHalfWidth;
        rightBoundary = screenHalfWidth;
    }

    void GenerateEnemies()
    {
        float yPointer = 0f;
        for (int row = 0; row < enemyRows; row++)
        {
            float xPointer = 0f;

            GameObject enemyToGenerate = enemyShooterObj;
            if (row != 0)
            {
                enemyToGenerate = enemyObj;
            }

            for (int col = 0; col < enemyColumns; col++)
            {
                Vector2 position = new Vector2(transform.position.x + xPointer, transform.position.y + yPointer);
                Instantiate(enemyToGenerate, position, transform.rotation, transform);
                // Debug.Log($"enemy generated at {xPointer},{yPointer}");
                xPointer += columnDistance;
            }

            yPointer -= rowDistance;
        }
    }

    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= moveInterval)
        {
            timePassed = 0f;
            MoveRoutine();
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
            return rightMost >= rightBoundary;
        }
        else
        {
            return leftmost <= leftBoundary;
        }
    }

    void MoveRoutine()
    {
        Vector2 currentPos = transform.position;
        currentPos.x += moveDistance * direction;

        bool boundaryReached = CheckBoundaryCollision();
        if (boundaryReached)
        {
            Debug.Log("Boundary Reached");

            direction *= -1;
            currentPos.x += moveDistance * direction;
            currentPos.y -= downDistance;
        }

        transform.position = currentPos;
    }

}
