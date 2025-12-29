using System;
using Unity.VisualScripting;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites;
    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void AnimateSprite()
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
