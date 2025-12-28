using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int direction = 1;  // 1 = Right, -1 = Left
    [SerializeField] private float moveInterval = 0.1f;
    [SerializeField] private float moveDistance = 0.1f;
    [SerializeField] private float downDistance = 0.25f;
    private float timePassed = 0f;
    [SerializeField] private float leftBoundary;
    [SerializeField] private float rightBoundary;
    [SerializeField] private float groupLeftX;
    [SerializeField] private float groupRightX;
    [SerializeField] private float spriteHeight;
    [SerializeField] private float spriteWidth = 0.6f;

    void Start()
    {
        Camera cam = Camera.main;
        float screenHalfWidth = cam.aspect * cam.orthographicSize;
        leftBoundary = -screenHalfWidth;
        rightBoundary = screenHalfWidth;
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
        // transform.position += Vector3.right * moveDistance * direction;
    }


}
