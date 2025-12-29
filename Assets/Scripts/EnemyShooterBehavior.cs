using UnityEngine;

public class InvaderShooter : MonoBehaviour
{

    public GameObject bulletObj;
    public float shootTime;
    public float minShootTime = 5f;
    public float maxShootTime = 20f;
    [SerializeField] private float elapsedTime = 0f;

    void Start()
    {
        GenerateNewShootTime();
    }

    void GenerateNewShootTime()
    {
        shootTime = Random.Range(minShootTime, maxShootTime);
        elapsedTime = 0f;
    }

    void ShootBullet()
    {
        Instantiate(bulletObj, transform.position, transform.rotation);
        GenerateNewShootTime();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= shootTime)
        {
            ShootBullet();
        }

    }
}
