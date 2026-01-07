using Unity.VisualScripting;
using UnityEngine;

public class MysteryShip : MonoBehaviour
{
    public float moveSpeed = 3f;
    [SerializeField] private int direction = 1;  // Left = -1, Right = 1
    public int pointValue = 50;
    public int[] pointValues = { 50, 100, 150, 300 };
    public float spawnPosY = 3.2f;
    private float leftBoundary;
    private float rightBoundary;
    private float width;
    public GameObject onDeathParticleEffect;

    void Awake()
    {
        // Boundaries
        Camera cam = Camera.main;
        float screenHalfWidth = cam.aspect * cam.orthographicSize;
        leftBoundary = -screenHalfWidth;
        rightBoundary = screenHalfWidth;

        direction = (Random.Range(0, 2) * 2) - 1;  // -1 or 1, left or right
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        width = sr.bounds.extents.x;

        // Move position to hard coded location
        Vector2 currentPos = transform.position;
        currentPos.x = direction > 0 ? leftBoundary - width : rightBoundary + width;
        currentPos.y = spawnPosY;
        transform.position = currentPos;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityX = moveSpeed * direction;

        // Random Point Value
        int pointValueIndex = Random.Range(0, pointValues.Length - 1);
        pointValue = pointValues[pointValueIndex];
    }

    void Update()
    {
        CheckBoundaries();
    }

    void CheckBoundaries()
    {
        float buffer = 0.5f;  // Prevent out of bounds on instantiate?
        if (transform.position.x + buffer < leftBoundary - width || transform.position.x - buffer > rightBoundary + width)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "PlayerBullet")
        {
            GameManager.Instance?.AddScore(pointValue);
            Instantiate(onDeathParticleEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
