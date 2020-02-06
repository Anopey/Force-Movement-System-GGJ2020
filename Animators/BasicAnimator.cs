using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimator : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> frames;

    [SerializeField]
    private List<float> colliderSizes;

    [SerializeField]
    private float secondsPerFrame;


    [SerializeField]
    private CircleCollider2D circleCollider;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private int currentIndex = 0;

    private float currentCooldown;

    private void Start()
    {
        currentCooldown = secondsPerFrame;
    }

    private void FixedUpdate()
    {

        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            return;
        }
        else
        {
            currentCooldown = secondsPerFrame;
        }
        spriteRenderer.sprite = frames[currentIndex];
        circleCollider.radius = colliderSizes[currentIndex];
        if (currentIndex == frames.Count - 2)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }

    }
}
