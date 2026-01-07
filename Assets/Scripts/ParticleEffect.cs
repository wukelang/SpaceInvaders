using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    void Awake()
    {
        // ParticleSystem partSys = gameObject.GetComponent<ParticleSystem>();
        float totalDuration = 0.2f;
        Destroy(gameObject, totalDuration);
    }
}
