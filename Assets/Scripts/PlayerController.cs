using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 10f;
    private float minX;
    private float maxX;
    private float spriteWidth;

    void Start()
    {
        Camera cam = Camera.main;
        float screenHalfWidth = cam.aspect * cam.orthographicSize;
        // Debug.Log("Screen half width: " + screenHalfWidth);

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.extents.x;
        // Debug.Log("Sprite width: " + spriteWidth);

        minX = -screenHalfWidth + spriteWidth;
        maxX = screenHalfWidth - spriteWidth;
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 currentPos = transform.position;

        bool goLeft = Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed;
        bool goRight = Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed;
        if (goLeft && !goRight)
        {
            currentPos.x -= moveSpeed * Time.deltaTime;
        }
        else if (goRight && !goLeft)
        {
            currentPos.x += moveSpeed * Time.deltaTime;
        }
        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);

        transform.position = currentPos;
    }


}
