using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float playerHP = 1f;

    [SerializeField]
    private HealthBar bar;

    private bool canCure = false;

    private static Player singleton;

    #region Unity and Instantiation

    private void Awake()
    {
        if(singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
    }

    private void OnDestroy()
    {
        if (singleton == this)
            singleton = null;
    }

    private void FixedUpdate()
    {
        FixedCooldownProcedure();
    }

    #endregion

    #region Player HP

    public static void ChangePlayerHP(float change)
    {
        singleton.onPlayerHPChange(singleton.playerHP, singleton.playerHP + change);
        singleton.playerHP += change;
        UpdateHPBar();
    }

    #endregion


    #region Subscription

    private Action<float, float> playerHPChangeEvent;
    private Action<KeyCode> keyPressEvent; 

    public static void SubscribePlayerHPChange(Action<float, float> action)
    {
        singleton.playerHPChangeEvent += action;
    }

    public static void UnSubscribePlayerHPChange(Action<float,float> action)
    {
        if (singleton == null || singleton.playerHPChangeEvent == null)
            return;
        singleton.playerHPChangeEvent -= action;
    }

    public static void SubscribePlayerKeyPressEvent(Action<KeyCode> action)
    {
        singleton.keyPressEvent += action;
    }

    public static void UnSubscribePlayerKeyPressEvent(Action<KeyCode> action)
    {
        if (singleton == null || singleton.keyPressEvent == null)
            return;
        singleton.keyPressEvent -= action;
    }

    private void onPlayerHPChange(float oldHP, float newHP)
    {
        if (playerHPChangeEvent == null)
            return;

        playerHPChangeEvent(oldHP, newHP);
        Debug.Log("Player HP changed to " + newHP.ToString());
    }

    #endregion

    #region Switch System

    private static float currentPlayerSwitchCooldown = 0f;

    private void FixedCooldownProcedure()
    {
        if (currentPlayerSwitchCooldown > 0)
        {
            currentPlayerSwitchCooldown -= Time.fixedDeltaTime;
        }
    }

    public static bool CanSwitchPlayers()
    {
        return currentPlayerSwitchCooldown <= 0;
    }

    private bool isShip = true;

    public static void ReportSwitchPlayer()
    {
        singleton.isShip = !singleton.isShip;
        currentPlayerSwitchCooldown = GameProperties.GetSwitchControllerCooldown();
    }

    public static bool GetIsShip()
    {
        return singleton.isShip;
    }

    public static void EnableCure(bool enable)
    {
        singleton.canCure = enable;
    }

    public static bool GetCanCure()
    {
        return singleton.canCure;
    }

    #endregion


    #region Misc
    
    public static void UpdateHPBar()
    {
        singleton.bar.HealtSet(singleton.playerHP * 100);
    }

    public static void ReportKeyPress(KeyCode c)
    {
        if (singleton.keyPressEvent != null)
            singleton.keyPressEvent(c);
    }


    #endregion

}
