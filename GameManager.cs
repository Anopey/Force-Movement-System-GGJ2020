using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    #region Unity and Instantiation

    private static GameManager singleton;

    private static List<Infection> infections = new List<Infection>();

    private void Awake()
    {
        if(singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        if (singleton == this)
        {
            singleton = null;
            UnSubscribeToEvents();
            infections.Clear();
        }
    }

    private void SubscribeToEvents()
    {
        Player.SubscribePlayerHPChange(OnPlayerHPLoss);
    }

    private void UnSubscribeToEvents()
    {
        Player.UnSubscribePlayerHPChange(OnPlayerHPLoss);
    }

    #endregion

    #region Death

    private void OnPlayerHPLoss(float oldHP, float newHP)
    {
        if(newHP <= 0)
        {
            Debug.Log("Diedededededededededededededededededededeededed.");
            Diededededed();
        }
    }

    private void Diededededed()
    {
        SceneManager.LoadScene(3);
    }

    #endregion
    #region Victory

    public static void AddInfection(Infection inf)
    {
        infections.Add(inf);
    }

    public static void RemoveInfection(Infection inf)
    {
        infections.Remove(inf);
        if(infections.Count == 0)
        {
            //win!
            SceneManager.LoadScene(5);
        }
    }

    #endregion

}
