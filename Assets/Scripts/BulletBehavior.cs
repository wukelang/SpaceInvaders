using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bulletSpeed = 5.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = bulletSpeed;
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
