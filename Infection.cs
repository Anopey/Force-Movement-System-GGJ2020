using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infection : MonoBehaviour
{

    private void Start()
    {
        RightKey.DeActivateKey();
        UpKey.DeActivateKey();
        DownKey.DeActivateKey();
        LeftKey.DeActivateKey();
        RKey.DeActivateKey();
        GameManager.AddInfection(this);
    }

    bool entered = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();
        if (p == null)
            return;
        Debug.Log("ENTERED RANGE OF INFECTION :O");
        Player.EnableCure(true);
        Player.SubscribePlayerKeyPressEvent(OnPlayerKeyPress);
        if (!Player.GetIsShip())
        {
            RKey.ActivateKey();
        }
        entered = true;
        StartCoroutine(WaitChangeEnumerator());
    }

    private IEnumerator WaitChangeEnumerator()
    {
        while (entered)
        {
            bool isChar = !Player.GetIsShip();
            if (isChar && !gameStarted)
            {
                RKey.ActivateKey();
            }

            if (!isChar && gameStarted)
            {
                EndGame();
            }

            if (!isChar && !gameStarted)
            {
                RKey.DeActivateKey();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();
        if (p == null)
            return;
        entered = false;
        Debug.Log("Exited range of infection :(");
        Player.EnableCure(false);
        Player.UnSubscribePlayerKeyPressEvent(OnPlayerKeyPress);
        RightKey.DeActivateKey();
        UpKey.DeActivateKey();
        DownKey.DeActivateKey();
        LeftKey.DeActivateKey();
        RKey.DeActivateKey();
        if (gameStarted)
            gameStarted = false;
    }

    #region Game

    [SerializeField]
    private Key UpKey, DownKey, LeftKey, RightKey, RKey;


    private Queue<KeyCode> gameCodes = new Queue<KeyCode>(); 
    private bool gameStarted = false;

    private KeyCode currentCheck = KeyCode.None;

    private void OnPlayerKeyPress(KeyCode c)
    {
        if(!gameStarted && c == KeyCode.R)
        {
            StartGame();
        }

        if (gameStarted)
        {
            currentFramePressed = c;
        }
    }
    
    private void StartGame()
    {
        int length = Random.Range(GameProperties.GetMinimumRandomChecks(), GameProperties.GetMaximumRandomChecks() + 1);
        for(int i = 0; i < length; i++)
        {
            int rand = Random.Range(0, 4);
            gameCodes.Enqueue(RandToCode(rand));
        }
        gameStarted = true;
        currentCheck = KeyCode.None;
        StartCoroutine(MiniGameEnumeration());
    }

    private void EndGame()
    {
        gameCodes.Clear();
        gameStarted = false;
        RightKey.DeActivateKey();
        UpKey.DeActivateKey();
        DownKey.DeActivateKey();
        LeftKey.DeActivateKey();
        RKey.DeActivateKey();

    }

    private KeyCode RandToCode(int rand)
    {
        switch (rand)
        {
            case 0:
                return KeyCode.UpArrow;
            case 1:
                return KeyCode.RightArrow;
            case 2:
                return KeyCode.DownArrow;
            case 3:
                return KeyCode.LeftArrow;
        }
        throw new System.Exception("NOOO");
    }

    private KeyCode currentFramePressed;

    private IEnumerator MiniGameEnumeration()
    {
        currentCheck = KeyCode.None;
        float currentCooldown = GameProperties.GetCheckTime();
        while (gameStarted)
        {
            //get new check
            if(currentCheck == KeyCode.None)
            {
                currentCheck = gameCodes.Dequeue();
            }

            switch (currentCheck)
            {
                case KeyCode.UpArrow:
                    RightKey.DeActivateKey();
                    UpKey.ActivateKey();
                    DownKey.DeActivateKey();
                    LeftKey.DeActivateKey();
                    RKey.DeActivateKey();
                    break;
                case KeyCode.RightArrow:
                    RightKey.ActivateKey();
                    UpKey.DeActivateKey();
                    DownKey.DeActivateKey();
                    LeftKey.DeActivateKey();
                    RKey.DeActivateKey();
                    break;
                case KeyCode.DownArrow:
                    RightKey.DeActivateKey();
                    UpKey.DeActivateKey();
                    DownKey.ActivateKey();
                    LeftKey.DeActivateKey();
                    RKey.DeActivateKey();
                    break;
                case KeyCode.LeftArrow:
                    RightKey.DeActivateKey();
                    UpKey.DeActivateKey();
                    DownKey.DeActivateKey();
                    LeftKey.ActivateKey();
                    RKey.DeActivateKey();
                    break;
            }

            //start timer
            currentCooldown = GameProperties.GetCheckTime();

            while(currentFramePressed != currentCheck)
            {
                yield return new WaitForEndOfFrame();
                currentCooldown -= Time.fixedDeltaTime;
                if(currentCooldown < 0)
                {
                    Debug.Log("FAIL");
                    EndGame();
                    yield break;
                }
            }

            currentCheck = KeyCode.None;
            //check if any left
            if(gameCodes.Count == 0)
            {
                Debug.Log("WIN");
                GameManager.RemoveInfection(this);
                transform.parent.GetComponent<SpriteRenderer>().enabled = false;
                Destroy(gameObject);
                yield break;
            }
        }
    }

    #endregion
}
