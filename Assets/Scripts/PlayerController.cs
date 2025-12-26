using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 10f;
    private float minX;
    private float maxX;
    public GameObject bulletObject;
    private GameObject activeBullet;  // Only allow one bullet on screen
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
        Shoot();

    }

    void Shoot()
    {
        bool shootPressed = Keyboard.current.spaceKey.isPressed;
        if (shootPressed && activeBullet == null)
        {
            Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y + (spriteHeight / 2), 1);
            activeBullet = Instantiate(bulletObject, bulletPos, transform.rotation);
        }
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
