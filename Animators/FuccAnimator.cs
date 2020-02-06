using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuccAnimator : MonoBehaviour
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

    [SerializeField]
    private int idleCycles = 7;

    [SerializeField]
    private int attackCycles = 1;

    private int currentIdle = 0;

    private int currentAttack = 0;

    private bool idle = true;

    private int currentIndex = 0;

    private float currentCooldown;

    [SerializeField]
    private List<int> idleIndexes;

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
        if (!idle)
        {

            spriteRenderer.sprite = frames[currentIndex];
            circleCollider.radius = colliderSizes[currentIndex];
            if (currentIndex == frames.Count - 1)
            {
                currentIndex = 0;
                currentAttack++;
            }
            else
            {
                currentIndex++;
            }
            if(currentAttack == attackCycles)
            {
                currentIndex = 0;
                currentAttack = 0;
                idle = true;
            }
        }
        else
        {
            spriteRenderer.sprite = frames[idleIndexes[currentIndex]];
            circleCollider.radius = colliderSizes[idleIndexes[currentIndex]];
            if (currentIndex == idleIndexes.Count - 1)
            {
                currentIndex = 0;
                currentIdle++;
            }
            else
            {
                currentIndex++;
            }
            if(currentIdle == idleCycles)
            {
                currentIndex = 0;
                currentIdle = 0;
                idle = false;
            }
        }
    }
}
