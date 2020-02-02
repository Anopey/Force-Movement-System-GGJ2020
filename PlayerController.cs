using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ForceField
{

    [SerializeField]
    private float movementConstant = 0.066f;

    [SerializeField]
    private float movementSmoothConstant = 3;

    [SerializeField]
    private int maxHold = 20;

    [SerializeField]
    private int stopCoefficient = 2; //has to at least be 1.

    private MovableObject movableObject;

    private Dictionary<char, int> clickCounts = new Dictionary<char, int>();

    [SerializeField]
    private float[] directionalDockDistances = new float[8]; //starting from up, directions going clockwise.

    [SerializeField]
    private Vector2[] directionalDockPlacementDistances = new Vector2[8];


    //docking
    private bool docked = false;
    private float oldMovementFactor;

    private void Start()
    {
        clickCounts['W'] = 0;
        clickCounts['D'] = 0;

        movableObject = GetComponent<MovableObject>();
        affectedObjects.Add(movableObject);
        AffectedObjectAdded(movableObject);
    }

    private void FixedUpdate()
    {
        
        //up
        if (Input.GetKey(KeyCode.W)){
            clickCounts['W']++;
            if (clickCounts['W'] < 0)
            {
                clickCounts['W'] += (stopCoefficient - 1);
            }
        }

        //right
        if (Input.GetKey(KeyCode.D))
        {
            clickCounts['D']++;
            if (clickCounts['D'] < 0)
            {
                clickCounts['D'] += (stopCoefficient - 1);
            }
        }

        //left
        if (Input.GetKey(KeyCode.A))
        {
            clickCounts['D']--;
            if(clickCounts['D'] > 0)
            {
                clickCounts['D'] -= (stopCoefficient - 1);
            }
        }

        //down
        if (Input.GetKey(KeyCode.S))
        {
            clickCounts['W']--;
            if (clickCounts['W'] > 0)
            {
                clickCounts['W'] -= (stopCoefficient - 1);
            }
        }

        //CTRL Dock
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!docked)
            {
                //try to dock.
                for (int i = 0; i < 8; i++)
                {
                    RaycastHit2D hit;
                    hit = Physics2D.Raycast(transform.position, GetNormalizedVectorDirectionByID(i), directionalDockDistances[i], 1 << 10);
                    if (hit.collider != null)
                    {
                        //can dock.
                        Debug.Log("docked");
                        transform.position = hit.point + directionalDockPlacementDistances[i];
                        docked = true;
                        oldMovementFactor = movableObject.GetMovementFactor();
                        movableObject.SetMovementFactor(0);
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("undocked");
                //undock!
                docked = false;
                movableObject.SetMovementFactor(oldMovementFactor);
            }
        }

        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            if (clickCounts['W'] > 0)
                clickCounts['W'] = Mathf.Clamp(clickCounts['W'] - stopCoefficient, 0, maxHold);
            else if (clickCounts['W'] < 0)
            {
                clickCounts['W'] = Mathf.Clamp(clickCounts['W'] + stopCoefficient, -maxHold, 0 );
            }
        }

        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
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
        ChangeForce(new Vector2((clickCounts['D'] / (float)maxHold * movementSmoothConstant * movementConstant),(clickCounts['W'] / (float)maxHold * movementSmoothConstant * movementConstant)));
        AffectedObjectAdded(movableObject);
    }

    public static Vector2 GetNormalizedVectorDirectionByID(int id)
    {
        switch (id)
        {
            case 0:
                return Vector2.up;
            case 1:
                return new Vector2(1, 1).normalized;
            case 2:
                return Vector2.right;
            case 3:
                return new Vector2(1, -1).normalized;
            case 4:
                return Vector2.down;
            case 5:
                return new Vector2(-1, -1).normalized;
            case 6:
                return Vector2.left;
            case 7:
                return new Vector2(-1, 1).normalized;
        }
        throw new System.Exception("Invalid ID was passed");
    }

}
