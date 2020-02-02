using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ForceField
{

    [SerializeField]
    private float movementConstant = 0.066f;

    [SerializeField]
    private bool canDock = true;

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

    [SerializeField]
    private float dockingKeyPressCooldown = 0.5f;

    [SerializeField]
    private ChildMan childMan;

    //docking
    private bool docked = false;
    private float oldMovementFactor;
    private float currentDockingCooldown = 0f;

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

        if (Player.CanSwitchPlayers() && Input.GetKey(KeyCode.V))
        {
            //SWITCH
            Player.ReportSwitchPlayer();
            childMan.ChangeControlled();
        }
        
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

        if (canDock)
        {
            if (currentDockingCooldown > 0)
            {
                currentDockingCooldown -= Time.fixedDeltaTime;
            }
            else
            {
                //CTRL Dock
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    currentDockingCooldown = dockingKeyPressCooldown;
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
                                OnDock();
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
                        OnUnDock();
                    }
                }
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

    #region Utility

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

    #endregion


    //If you have forked this or downloaded it on Github, get rid of these as they are not needed.
    #region Animation and Outside-Repo

    private void OnDock()
    {
        ShipAnimationController.ChangeHands(false);
    }

    private void OnUnDock()
    {
        ShipAnimationController.ChangeHands(true);
    }

    #endregion

}
