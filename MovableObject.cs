using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovableType { Wall, Player, Other}

[RequireComponent(typeof(Collider2D))]
public class MovableObject : MonoBehaviour
{

    [SerializeField]
    [Range(0,1)]
    private float mirrorFactor = 1f;

    [SerializeField]
    private float movementFactor = 1;

    [SerializeField]
    private float repellantForceFactor = 1f;

    [SerializeField]
    private float damageToPlayer = 0.25f;

    [SerializeField]
    private MovableType movableType = MovableType.Other;

    private Dictionary<GameObject, Vector2> affectingVectors = new Dictionary<GameObject, Vector2>(); //the affecting vectors are managed here via a registration system, which is accessed by the colliders.
    private List<GameObject> collisionForcers = new List<GameObject>();
    private Rigidbody2D rigidBody;
    private Vector2 lastAggregate;

    private RandomMover randomMover;

    [SerializeField]
    private AudioSource hitAudioSource;

    #region Unity and Instantiation

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        randomMover = GetComponent<RandomMover>();
    }

    List<GameObject> toBeRemoved = new List<GameObject>();

    // Update is called once per frame
    void FixedUpdate()
    {
        //deal with force fields
        if (affectingVectors.Count == 0)
            return;
        Vector2 aggregateVector = Vector2.zero;
        foreach(Vector2 vector in affectingVectors.Values)
        {
            aggregateVector += vector;
        }
        rigidBody.MovePosition(new Vector2(transform.position.x, transform.position.y) + aggregateVector * GameProperties.GetMovementTweakConstant() * movementFactor);
        lastAggregate = aggregateVector;

        //deal with collision forcers
        if (collisionForcers.Count <= 0)
            return;

        toBeRemoved.Clear();

        foreach (GameObject g in collisionForcers)
        {
            affectingVectors[g] -= affectingVectors[g].normalized * GameProperties.GetForceFallOffBaseModifier();
            affectingVectors[g] *= GameProperties.GetDynamicFallOffConstant();
            if (affectingVectors[g].magnitude <= GameProperties.GetForceFallOffBaseModifier())
            {
                //movement too little nao.
                toBeRemoved.Add(g);
                affectingVectors.Remove(g);
                Debug.Log("done with force of " + g.name + " acting upon " + gameObject.name);
            }
        }

        foreach(GameObject g in toBeRemoved)
        {
            collisionForcers.Remove(g);
        }
    }

    protected virtual void onFixedUpdate()
    {

    }

    #endregion

    #region Vector System
        
    public void AddChangeAffectingForce(GameObject forcer, Vector2 force)
    {
        affectingVectors[forcer] = force;
    }

    public void RemoveAffectingForce(GameObject forcer)
    {
        if (!affectingVectors.ContainsKey(forcer))
        {
            Debug.LogError("The affecting vector was never registered in the first place.");
            return; 
        }
        affectingVectors.Remove(forcer);
    }

    #endregion

    #region Collision System

    public Vector2 GetLastAggregateVelocityVector()
    {
        return lastAggregate;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MovableObject movable = collision.collider.GetComponent<MovableObject>();
        if (movable == null || collision.otherCollider.GetComponent<MovableObject>() == null)
            return;
        Vector2 pos = transform.position;
        Vector2 collidedPos = collision.transform.position;
        Vector2 baseCollision = -collision.GetContact(0).normal;
        float angle = Vector2.Angle(lastAggregate, baseCollision);
        float aggregateMultiplier = Mathf.Abs( 90 - angle) / 90;
        if (angle < 0)
            aggregateMultiplier *= -1;
        //same thing for repulsive factor.
        Vector2 baseRepulsiveFactor = collision.otherCollider.GetComponent<MovableObject>().GetLastAggregateVelocityVector();
        float repulsiveAngle = Vector2.Angle(baseRepulsiveFactor, baseCollision);
        float repulsiveMultiplier = Mathf.Abs(90 - repulsiveAngle) / 90;
        movable.AddForce(collision.otherCollider.gameObject, ((baseCollision) * repellantForceFactor * GameProperties.GetBaseCollisionForce()) + (lastAggregate * aggregateMultiplier) * GameProperties.GetCollisionVelocityTransferrance() * repellantForceFactor + 
            baseRepulsiveFactor * repulsiveMultiplier * mirrorFactor);
        HandleCollidedMovableType(collision, movable);
    }

    public void AddForce(GameObject collider, Vector2 force)
    {
        Debug.Log((force * movementFactor).ToString() + "added to " + gameObject.name);
        affectingVectors[collider] = force * movementFactor;
        if (!collisionForcers.Contains(collider))
            collisionForcers.Add(collider);
    }

    #endregion

    #region Movable Type and Type Interactions

    public MovableType GetMovableType()
    {
        return movableType;
    }

    private void HandleCollidedMovableType(Collision2D col, MovableObject collided)
    {
        if(hitAudioSource != null)
        {
            hitAudioSource.Play();
        }
        switch (collided.GetMovableType())
        {
            case MovableType.Other:

                break;
            case MovableType.Player:
                Player.ChangePlayerHP(-damageToPlayer);
                break;
            case MovableType.Wall:
                if(randomMover != null)
                {
                    randomMover.RandomizeMovement();
                }
                break;
        }
    }

    #endregion

    #region Various Getters and Setters

    public float GetMovementFactor()
    {
        return movementFactor;
    }

    public void SetMovementFactor(float newFactor)
    {
        movementFactor = newFactor;
    }

    #endregion

}
