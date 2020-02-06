using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimationController : MonoBehaviour
{
    private bool enabled = true;
    public Animator[] hands, motors;
    public Animator guy;
    public Transform guyTransform;
    public ParticleSystem[] bubbleCreators;
    // Start is called before the first frame update
    void Start()
    {

        if(singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        hands[0].SetBool("HandsOpen", true);
        for (int i = 0; i < bubbleCreators.Length; i++)
        {
            bubbleCreators[i].Pause();
        }
    }

    private void OnDestroy()
    {
        if (singleton == this)
            singleton = null;
    }

    public void HandsChange(bool a)
    {
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].SetBool("HandsOpen", a);
        }
    }

    private void FixedUpdate()
    {
        if (!enabled)
        {
            if (Input.GetKey("s") || Input.GetKey("d") || Input.GetKey("w") || Input.GetKey("a"))
            {
                guy.SetBool("move", true);
                if (Input.GetKey("d"))
                    guyTransform.localScale = new Vector3(-1, guyTransform.localScale.y, guyTransform.localScale.z);
                else
                    guyTransform.localScale = new Vector3(1, guyTransform.localScale.y, guyTransform.localScale.z);
            }
            else
            {
                guy.SetBool("move", false);
            }
            if (Input.GetKey("r"))
                guy.SetBool("fix", true);
            if (Input.GetKey("t"))
                guy.SetBool("fix", false);
            return;
        }
        if (Input.GetKey("s"))
        {
            motors[0].SetBool("isGoingDirection", true);
            bubbleCreators[0].Play();
        }
        else
        {
            motors[0].SetBool("isGoingDirection", false);
            bubbleCreators[0].Pause();
            bubbleCreators[0].Clear();
        }
        if (Input.GetKey("d"))
        {
            motors[1].SetBool("isGoingDirection", true);
            bubbleCreators[1].Play();

        }
        else
        {
            motors[1].SetBool("isGoingDirection", false);
            bubbleCreators[1].Pause();

            bubbleCreators[1].Clear();
        }
        if (Input.GetKey("w"))
        {
            motors[2].SetBool("isGoingDirection", true);
            bubbleCreators[2].Play();
        }
        else
        {
            motors[2].SetBool("isGoingDirection", false);
            bubbleCreators[2].Pause();

            bubbleCreators[2].Clear();
        }
        if (Input.GetKey("a"))
        {
            motors[3].SetBool("isGoingDirection", true);
            bubbleCreators[3].Play();
        }
        else
        {
            motors[3].SetBool("isGoingDirection", false);
            bubbleCreators[3].Pause();

            bubbleCreators[3].Clear();
        }
    }

    public void MotorsChange(int whichMotor,bool a) // nesw
    {
        motors[whichMotor].SetBool("isGoingDirection", a);
    }

    #region Static Access

    private static ShipAnimationController singleton;

    public static void ChangeHands(bool a)
    {
        singleton.HandsChange(a);
    }

    public static void ChangeMotors(int whichMotor, bool a)
    {
        singleton.MotorsChange(whichMotor, a);
    }

    public static void Enable(bool enabled)
    {
        singleton.enabled = enabled;
    }

    #endregion
}
