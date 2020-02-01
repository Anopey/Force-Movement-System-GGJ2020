using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ForceFieldType { Static, Dynamic}
public enum ChangeType { None, MinMax}
[RequireComponent(typeof(Collider2D))]
public class ForceField : MonoBehaviour
{
    [SerializeField]
    private bool forceScaled;
    private Vector2 effectiveForce;
    private float forceEffectFactor = 1f;
    [SerializeField]
    private Vector2 force; //for dynamic, the x component of this shall be the base magnitude of the force.
    [SerializeField]
    private ForceFieldType fieldType;
    [SerializeField]
    private float dynamicForceModifier = 2;

    [SerializeField]
    private bool fieldActive = true;

    [SerializeField]
    private ChangeType changeType = ChangeType.None;

    [SerializeField]
    private ChangeFalloffParameters changeFalloffParameters;


    protected List<MovableObject> affectedObjects = new List<MovableObject>();

    #region Unity Collision System

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

    private void Awake()
    {
        effectiveForce = force * forceEffectFactor;
        if (changeType != ChangeType.None)
        {
            HandleFalloffParameters();
        }
    }

    private void FixedUpdate()
    {
        if (!forceScaled)
        {
            return;
        }
        foreach (MovableObject obj in affectedObjects)
        {
            AffectedObjectAdded(obj);
        }
    }

    #endregion


    protected void AffectedObjectAdded(MovableObject obj)
    {
        obj.AddChangeAffectingForce(gameObject, GetAddedForce(obj.transform.position));
    }

    private void AffectedObjectRemoved(MovableObject obj)
    {
        obj.RemoveAffectingForce(gameObject);
    }



    private Vector2 GetAddedForce(Vector2 affectedPos)
    {
        Vector2 pos = transform.position;
        if(fieldType == ForceFieldType.Dynamic)
        {
            return ((forceScaled ? (effectiveForce.x * (pos - affectedPos).normalized) / Mathf.Pow((affectedPos - pos).magnitude, dynamicForceModifier) : effectiveForce.x * (pos - affectedPos).normalized));
        }
        else
        {
            return ((forceScaled ? effectiveForce / Mathf.Pow((affectedPos - pos).magnitude, dynamicForceModifier)  : effectiveForce));
        }
    }

    #region Fall off Handling

    private void HandleFalloffParameters()
    {
        if(changeType == ChangeType.MinMax)
        {

        }
    }

    private IEnumerator MinMaxFallOffRoutine()
    {
        while (gameObject != null)
        {
            float currentSeconds = changeFalloffParameters.changeSeconds;
            while (currentSeconds > 0)
            {
                currentSeconds -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            if(forceEffectFactor == changeFalloffParameters.max)
            {
                float totalChange = changeFalloffParameters.min - forceEffectFactor;
                float changeBase = (totalChange < 0? - Mathf.Abs(changeFalloffParameters.changeBaseCoeff) : Mathf.Abs(changeFalloffParameters.changeBaseCoeff));

                while(totalChange < 0)
                {
                    yield return new WaitForFixedUpdate();
                    float currentChange = totalChange * (1 - changeFalloffParameters.changeDynamicCoeff) + changeBase;
                    ChangeEffectiveForce(forceEffectFactor + currentChange);

                }
                ChangeEffectiveForce(changeFalloffParameters.min);
            }else if(forceEffectFactor == changeFalloffParameters.min)
            {
                float totalChange = changeFalloffParameters.max - forceEffectFactor;
                float changeBase = (totalChange < 0 ? -Mathf.Abs(changeFalloffParameters.changeBaseCoeff) : Mathf.Abs(changeFalloffParameters.changeBaseCoeff));

                while (totalChange > 0)
                {
                    yield return new WaitForFixedUpdate();
                    float currentChange = totalChange * (1 - changeFalloffParameters.changeDynamicCoeff) + changeBase;
                    ChangeEffectiveForce(forceEffectFactor + currentChange);

                }
                ChangeEffectiveForce(changeFalloffParameters.max);
            }
            else
            {
                Debug.LogError("This wasn't supposed to happen");
            }
        }
    }

    public Vector2 GetForce()
    {
        return force;
    }

    public Vector2 GetEffectiveForce()
    {
        return effectiveForce;
    }

    public void ChangeForce(Vector2 newForce)
    {
        force = newForce;
        ChangeEffectiveForce(forceEffectFactor);
    }

    public void ChangeEffectiveForce(float newForceFactor)
    {
        forceEffectFactor = newForceFactor;
        effectiveForce = force * newForceFactor;
        foreach (MovableObject affected in affectedObjects)
        {
            AffectedObjectAdded(affected);
        }
    }

    #endregion

}

[Serializable]
public class ChangeFalloffParameters
{
    public float min, max;
    public float changeSeconds = 2f;

    [Range(0,1)]
    public float changeDynamicCoeff = 0.98f;

    public float changeBaseCoeff = 0.002f;
}