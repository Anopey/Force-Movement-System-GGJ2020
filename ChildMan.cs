using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChildMan : MonoBehaviour
{

    [SerializeField]
    private PlayerController ship, child;

    private float maxDistLength;

    private CircleCollider2D circleCollider;

    [SerializeField]
    private float lineMaxWidth = 0.1f, lineMinWidth = 0f;

    private bool controlShip = true;

    //to disable the man when he is inside

    //repeating codes to disable
    [SerializeField]
    private Behaviour[] shipDisable;
    [SerializeField]
    private GameObject childObject;

    private Vector2 childInitPos;

    [SerializeField]
    private LineRenderer lineRenderer;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        maxDistLength = circleCollider.radius;
        childInitPos = childObject.transform.localPosition;
        EnableShip();
    }

    public void ChangeControlled()
    {
        if (controlShip)
        {
            //switch to character.
            EnableChild();
            controlShip = false;
            ShipAnimationController.Enable(false);
        }
        else
        {
            //switch to ship.
            EnableShip();
            controlShip = true;
            ShipAnimationController.Enable(true);
        }
    }

    private void EnableChild()
    {
        foreach (Behaviour b in shipDisable)
        {
            b.enabled = false;
        }
        childObject.SetActive(true);
        childObject.transform.parent = null;
        lineRenderer.enabled = true;
    }

    private void EnableShip()
    {
        foreach (Behaviour b in shipDisable)
        {
            b.enabled = true;
        }
        childObject.SetActive(false);
        childObject.transform.SetParent(shipDisable[0].gameObject.transform);
        childObject.transform.localPosition = childInitPos;
        lineRenderer.enabled = false;
    }


    private void FixedUpdate()
    {
        if (!controlShip)
        {
            CheckChildDistance();
            //draw lines
            DrawLineToChild();
        }
    }

    private void DrawLineToChild()
    { 
        lineRenderer.SetPositions(new Vector3[] { shipDisable[0].gameObject.transform.position, childObject.transform.position});
        float width = (lineMaxWidth - lineMinWidth) * (1 / ((childObject.transform.position - shipDisable[0].transform.position).magnitude / maxDistLength)) + lineMinWidth;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    private void CheckChildDistance()
    {
        if((childObject.transform.position - shipDisable[0].transform.position).magnitude > maxDistLength)
        {
            //DEDEDEDEDED
            SceneManager.LoadScene(4);   
        }
    }
}
