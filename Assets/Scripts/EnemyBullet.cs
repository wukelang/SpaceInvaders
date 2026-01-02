using UnityEngine;

public class EnemyBulletBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bulletSpeed = -2.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = bulletSpeed;
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // if (collider.tag == "Player")
        // {
        //     // Destroy(collider.gameObject);
        // }

        if (collider.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
