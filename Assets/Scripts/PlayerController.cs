using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    public GameObject bulletObject;
    [SerializeField] private GameObject activeBullet;  // Only allow one bullet on screen
    [SerializeField] private float spriteHeight;
    [SerializeField] private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    public GameObject onDeathParticleEffect;

    void Start()
    {
        Camera cam = Camera.main;
        float screenHalfWidth = cam.aspect * cam.orthographicSize;
        // Debug.Log("Screen half width: " + screenHalfWidth);

        // SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        float spriteWidth = spriteRenderer.bounds.extents.x;
        spriteHeight = spriteRenderer.bounds.extents.y;

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
        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);  // Prevent player movement out of camera bounds

        transform.position = currentPos;
    }

    public void EnableInvincibility(float duration)
    {
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }
        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isInvincible) { return; }

        Instantiate(onDeathParticleEffect, transform.position, transform.rotation);

        if (collider.tag == "EnemyBullet")
        {
            GameManager.Instance?.LoseLife();
        } 
        else if (collider.tag == "Enemy")
        {
            GameManager.Instance?.GameOver();
        }

        spriteRenderer.enabled = false;
    }

}
