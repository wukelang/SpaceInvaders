using Unity.VisualScripting;
using UnityEngine;

public class MysteryShip : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    // [SerializeField] private float moveInterval = 0.1f;
    [SerializeField] private int direction = 1;  // Left = -1, Right = 1
    public int pointValue = 150;
    public float spawnPosY = 3.2f;
    public float spawnPosX = 0f;
    private float leftBoundary;
    private float rightBoundary;
    private float width;

    void Awake()
    {
        Camera cam = Camera.main;
        float screenHalfWidth = cam.aspect * cam.orthographicSize;
        leftBoundary = -screenHalfWidth;
        rightBoundary = screenHalfWidth;

        direction = (Random.Range(0, 2) * 2) - 1;  // -1 or 1, left or right
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        width = sr.bounds.extents.x;
        Vector2 currentPos = transform.position;
        // if (direction > 0)
        // {
        //     currentPos.x = rightBoundary + width;
        // }
        currentPos.x = direction > 0 ? leftBoundary - width : rightBoundary + width;
        transform.position = currentPos;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityX = moveSpeed * direction;
    }

    void Update()
    {
        CheckBoundaries();
        
    }

    void CheckBoundaries()
    {
        Vector2 currentPos = transform.position;
        // currentPos.x += moveSpeed * direction;
        // transform.position = currentPos;

        if (currentPos.x < leftBoundary - width || currentPos.x > rightBoundary + width)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "PlayerBullet")
        {
            GameManager.Instance?.AddScore(pointValue);
            Destroy(gameObject);
            // gameObject.SetActive(false);
        }
    }
}
