using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLeveler : MonoBehaviour
{
    public AudioSource me;
    public bool isMusic;
    public bool update;
    private void Start()
    {
        if (isMusic)
            me.volume = MenuManager.music;
        else
            me.volume = MenuManager.sound;
    }
    void FixedUpdate()
    {
        if (update)
        {
            if (isMusic)
                me.volume = MenuManager.music;
            else
                me.volume = MenuManager.sound;
        }
    }
}
