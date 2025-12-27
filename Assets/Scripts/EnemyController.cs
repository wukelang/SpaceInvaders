using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int direction = 1;  // 1 = Right, -1 = Left
    [SerializeField] private float moveInterval = 0.5f;
    [SerializeField] private float moveDistance = 0.25f;
    [SerializeField] private float downDistance = 0.25f;
    private float timePassed = 0f;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float spriteWidth;
    [SerializeField] private float spriteHeight;

    void Start()
    {
        Camera cam = Camera.main;
        float screenHalfWidth = cam.aspect * cam.orthographicSize;
        // Debug.Log("Screen half width: " + screenHalfWidth);

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.extents.x;
        spriteHeight = spriteRenderer.bounds.extents.y;
        // Debug.Log("Sprite width: " + spriteWidth);

        minX = -screenHalfWidth + spriteWidth;
        maxX = screenHalfWidth - spriteWidth;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        // InvokeRepeating(nameof(MoveRoutine), moveInterval, moveInterval);

        if (timePassed >= moveInterval)
        {
            timePassed = 0f;
            MoveRoutine();
        }

        // Debug.Log($"Current Left Boundaries: {transform.position.x - (spriteWidth / 2)}");
        // Debug.Log($"Current Right Boundaries: {transform.position.x + (spriteWidth / 2)}");
    }

    void MoveRoutine()
    {
        Debug.Log("move routine");

        Vector2 currentPos = transform.position;
        currentPos.x += moveDistance * direction;

        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        if (currentPos.x == maxX || currentPos.x == minX)
        {
            direction *= -1;
            currentPos.y -= downDistance;
        }


        transform.position = currentPos;
        // transform.position += Vector3.right * moveDistance * direction;
    }


}
