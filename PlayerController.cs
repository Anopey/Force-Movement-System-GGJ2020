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
    private float[] directionalDockPlacementDistances = new float[8];

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
            //try to dock.
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, Vector2.up, directionalDockDistances[0], 1 << 10);
            if(hit.collider != null)
            {
                //can dock.

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

}
