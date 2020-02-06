using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : ForceField
{

    private enum KeyStatus { increase, decrease, none}

    private Dictionary<char, KeyStatus> keyStatuses = new Dictionary<char, KeyStatus>();

    [SerializeField]
    private float movementConstant = 0.066f;

    [SerializeField]
    private float movementRandomizationCooldown = 0.25f;

    [SerializeField]
    private float movementSmoothConstant = 3;

    [SerializeField]
    private int maxHold = 20;

    [SerializeField]
    private int stopCoefficient = 2; //has to at least be 1.

    [SerializeField]
    [Range(0,1)]
    private float sameFrameProbability = 0.98f;

    private MovableObject movableObject;

    private Dictionary<char, int> clickCounts = new Dictionary<char, int>();

    private float currentRandomizationCooldown = 0f;

    private void Start()
    {
        clickCounts['W'] = 0;
        clickCounts['D'] = 0;
        movableObject = GetComponent<MovableObject>();
        affectedObjects.Add(movableObject);
        AffectedObjectAdded(movableObject);
        keyStatuses['W'] = GetRandomKeyStatus();
        keyStatuses['D'] = GetRandomKeyStatus();
    }

    private void FixedUpdate()
    {
        if (currentRandomizationCooldown <= 0f)
        {
            float rand = Random.Range(0f, 1f);
            if (rand >= sameFrameProbability)
            {
                RandomizeMovement();
            }
        }
        currentRandomizationCooldown -= Time.fixedDeltaTime;

        //up
        if (keyStatuses['W'] == KeyStatus.increase)
        {
            clickCounts['W']++;
            if (clickCounts['W'] < 0)
            {
                clickCounts['W'] += (stopCoefficient - 1);
            }
        }

        //right
        if (keyStatuses['D'] == KeyStatus.increase)
        {
            clickCounts['D']++;
            if (clickCounts['D'] < 0)
            {
                clickCounts['D'] += (stopCoefficient - 1);
            }
        }

        //left
        if (keyStatuses['D'] == KeyStatus.decrease)
        {
            clickCounts['D']--;
            if (clickCounts['D'] > 0)
            {
                clickCounts['D'] -= (stopCoefficient - 1);
            }
        }

        //down
        if (keyStatuses['W'] == KeyStatus.decrease)
        {
            clickCounts['W']--;
            if (clickCounts['W'] > 0)
            {
                clickCounts['W'] -= (stopCoefficient - 1);
            }
        }

        if (keyStatuses['W'] == KeyStatus.none)
        {
            if (clickCounts['W'] > 0)
                clickCounts['W'] = Mathf.Clamp(clickCounts['W'] - stopCoefficient, 0, maxHold);
            else if (clickCounts['W'] < 0)
            {
                clickCounts['W'] = Mathf.Clamp(clickCounts['W'] + stopCoefficient, -maxHold, 0);
            }
        }

        if (keyStatuses['D'] == KeyStatus.none)
        {
            if (clickCounts['D'] > 0)
                clickCounts['D'] = Mathf.Clamp(clickCounts['D'] - stopCoefficient, 0, maxHold);
            else if (clickCounts['D'] < 0)
            {
                clickCounts['D'] = Mathf.Clamp(clickCounts['D'] + stopCoefficient, -maxHold, 0);
            }
        }
        clickCounts['W'] = Mathf.Clamp(clickCounts['W'], -maxHold, maxHold);
        clickCounts['D'] = Mathf.Clamp(clickCounts['D'], -maxHold, maxHold);
        ChangeForce(new Vector2((clickCounts['D'] / (float)maxHold * movementSmoothConstant * movementConstant), (clickCounts['W'] / (float)maxHold * movementSmoothConstant * movementConstant)));
        AffectedObjectAdded(movableObject);
    }

    private KeyStatus GetRandomKeyStatus()
    {
        int rand  = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                return KeyStatus.decrease;
            case 1:
                return KeyStatus.none;
            case 2:
                return KeyStatus.increase;
        }
        throw new System.Exception("Nani");
    }

    public void RandomizeMovement()
    {
        if (currentRandomizationCooldown > 0f)
            return;
        Debug.Log("yess");
        keyStatuses['W'] = GetRandomKeyStatus();
        keyStatuses['D'] = GetRandomKeyStatus();
        currentRandomizationCooldown = movementRandomizationCooldown;
    }
}
