using Unity.VisualScripting;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites;
    public bool canShoot;
    public GameObject enemyBullet;
    private float shootTime = 5f;
    public float minShootTime = 5f;
    public float maxShootTime = 20f;
    [SerializeField] private float elapsedTime;
    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GenerateNewShootTime();
    }

    void Start()
    {
        // InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
        InvaderGroupController groupController = GetComponentInParent<InvaderGroupController>();
        Debug.Log(groupController);
        if (groupController != null)
        {
            groupController.OnGroupMove += AnimateSprite;
        }
    }

    void Update()
    {
        if (canShoot)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= shootTime)
            {
                ShootBullet();
            }
        }
    }

    void GenerateNewShootTime()
    {
        shootTime = Random.Range(minShootTime, maxShootTime);
        elapsedTime = 0f;
    }

    void ShootBullet()
    {
        Instantiate(enemyBullet, transform.position, transform.rotation);
        GenerateNewShootTime();
    }

    void AnimateSprite()
    {
        animationFrame++;
        if (animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }

        if (spriteRenderer)
        {
            spriteRenderer.sprite = animationSprites[animationFrame];
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
