using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
   
    public void ActivateKey()
    {
        gameObject.SetActive(true);
    }

    public void DeActivateKey()
    {
        gameObject.SetActive(false);
    }

    public void UpdateKeyHP()
    {

    }

}
