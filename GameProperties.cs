using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProperties : MonoBehaviour
{
    [SerializeField]
    private Vector2 movementTweakConstant;

    [SerializeField]
    private float baseCollisionForce = 0.1f;

    [SerializeField]
    private float forceFallOffBaseParameter = 0.005f;

    [SerializeField]
    [Range(0, 1)]
    private float dynamicFallOffConstant = 0.98f;

    [SerializeField]
    [Range(0, 1)]
    private float collisionVelocityTransferrance = 0.5f;

    [SerializeField]
    private float switchControllerCooldown = 1f;

    [SerializeField]
    private float maximumVelocityMagnitude = 5f;

    #region Unity and Instantiation

    private static GameProperties singleton;

    private void Start()
    {
        if (singleton != null)
            return;
        singleton = this;
       
    }

    private void OnDestroy()
    {
        if (singleton = this)
            singleton = null;
    }

    #endregion

    #region Parameter Getter

    public static Vector2 GetMovementTweakConstant()
    {
        return singleton.movementTweakConstant;
    }

    public static float GetBaseCollisionForce()
    {
        return singleton.baseCollisionForce;
    }

    public static float GetForceFallOffBaseModifier()
    {
        return singleton.forceFallOffBaseParameter;
    }

    public static float GetDynamicFallOffConstant()
    {
        return singleton.dynamicFallOffConstant;
    }

    public static float GetCollisionVelocityTransferrance()
    {
        return singleton.collisionVelocityTransferrance;
    }

    public static float GetSwitchControllerCooldown()
    {
        return singleton.switchControllerCooldown;
    }

    public static float GetMaximumVelocityMagnitude()
    {
        return singleton.maximumVelocityMagnitude;
    }

    #endregion

}
