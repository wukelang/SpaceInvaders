using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 10f;
    private float minX;
    private float maxX;
    public GameObject bulletObject;
    private bool shotLastFrame = false;  // Check if this is the best way to do this
    private float spriteHeight;

    void Start()
    {
        Camera cam = Camera.main;
        float screenHalfWidth = cam.aspect * cam.orthographicSize;
        // Debug.Log("Screen half width: " + screenHalfWidth);

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        float spriteWidth = spriteRenderer.bounds.extents.x;
        spriteHeight = spriteRenderer.bounds.extents.y;
        // Debug.Log("Sprite width: " + spriteWidth);

        minX = -screenHalfWidth + spriteWidth;
        maxX = screenHalfWidth - spriteWidth;
    }

    void Update()
    {
        MovePlayer();

        bool shootPressed = Keyboard.current.spaceKey.isPressed;
        if (shootPressed && !shotLastFrame)
        {
            Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y + (spriteHeight / 2), 1);
            Instantiate(bulletObject, bulletPos, transform.rotation);
        }
        shotLastFrame = shootPressed;  // Player cannot hold to shoot
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
