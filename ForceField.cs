using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ForceFieldType { Static, Dynamic}
[RequireComponent(typeof(Collider2D))]
public class ForceField : MonoBehaviour
{
    [SerializeField]
    private bool forceScaled;
    [SerializeField]
    protected Vector2 force; //for dynamic, the x component of this shall be the base magnitude of the force.
    [SerializeField]
    private ForceFieldType fieldType;
    [SerializeField]
    private float dynamicForceModifier = 2;

    [SerializeField]
    private bool fieldActive = true;



    protected List<MovableObject> affectedObjects = new List<MovableObject>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!fieldActive)
            return;
        Debug.Log("Force field enter");
        MovableObject movable = collision.collider.GetComponent<MovableObject>();
        if (movable == null)
            return;
        affectedObjects.Add(movable);
        AffectedObjectAdded(movable);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!fieldActive)
            return;
        Debug.Log("Force field exit");
        MovableObject movable = collision.collider.GetComponent<MovableObject>();
        if (movable == null)
            return;
        affectedObjects.Remove(movable);
        AffectedObjectRemoved(movable);
    }

    protected void AffectedObjectAdded(MovableObject obj)
    {
        obj.AddChangeAffectingForce(gameObject, GetAddedForce(obj.transform.position));
    }

    private void AffectedObjectRemoved(MovableObject obj)
    {
        obj.RemoveAffectingForce(gameObject);
    }

    private void FixedUpdate()
    {
        if (!forceScaled)
        {
            return;
        }

        foreach(MovableObject obj in affectedObjects)
        {
            AffectedObjectAdded(obj);
        }
    }

    private Vector2 GetAddedForce(Vector2 affectedPos)
    {
        Vector2 pos = transform.position;
        if(fieldType == ForceFieldType.Dynamic)
        {
            return ((forceScaled ? (force.x * (pos - affectedPos).normalized) / Mathf.Pow((affectedPos - pos).magnitude, dynamicForceModifier) : force.x * (pos - affectedPos).normalized));
        }
        else
        {
            return ((forceScaled ? force / Mathf.Pow((affectedPos - pos).magnitude, dynamicForceModifier)  : force));
        }
    }

}
