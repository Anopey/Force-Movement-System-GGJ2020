using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnım : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      //  StartCoroutine(idle());   
    }
    public Vector3 scale;
    IEnumerator idle()
    {
        yield return new WaitForFixedUpdate();
        for (int i = 0; i <= 10; i++)
        {
            transform.localScale = scale * (1 + (i / 20));
            yield return new WaitForFixedUpdate();
        }
        for (int i = 9; i >= 0; i++)
        {
            transform.localScale = scale * (1 - (i / 20));
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(idle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
