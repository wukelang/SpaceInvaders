using System;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites;
    public float animationTime = 1.0f;
    private SpriteRenderer spriteRenderer;
    private int animationFrame;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    }

    void AnimateSprite()
    {
        animationFrame++;
        if (animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }

        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.tag);
        if (collider.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
