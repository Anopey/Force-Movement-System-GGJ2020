using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alveol : MonoBehaviour
{
    public GameObject[] yuvarlar;
    public Quaternion rot;
    int a = 0,b;
    public List<GameObject> yes;
    void FixedUpdate()
    {
        a++;
        
        if (a == 40)
        {
            b = Random.Range(0, 2);
            Vector3 callPosition = new Vector3(transform.position.x - 10, transform.position.y - Random.Range(-4, 4), 0);
            if (yes.Count > 25)
            {
                Destroy(yes[0]);
                yes.RemoveAt(0);
            }
            GameObject obj = Instantiate(yuvarlar[b], callPosition, rot);
            yes.Add(obj);
            a = 0;
        }
    }
}
