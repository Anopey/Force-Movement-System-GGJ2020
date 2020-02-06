using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Rigidbody2D myrigidb;
    public GameObject toFollow;

    void Start()
    {
        StartCoroutine(Follow());
    }

    bool followBreak;
    IEnumerator Follow()
    {
        //this way instead of transform making it chubby it makes everything smoother 
        Vector3 placeToGo = toFollow.transform.position;
        placeToGo = placeToGo - transform.position;
        myrigidb.velocity = placeToGo * 5;
        yield return new WaitForSeconds(0.01f);
        if (!followBreak)
            StartCoroutine(Follow());
    }
}
