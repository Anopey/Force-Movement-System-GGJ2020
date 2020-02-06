using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CinematicsScenary : MonoBehaviour
{
    public Color StartFrom;
    public float StartTimer;
    public Frame[] frames;

    public Image color, image;
    public CanvasGroup colorGroup, imageGroup;

    public AudioSource musicSource, audioSource;
    public AudioClip music;

    private void Start()
    {
        musicSource.clip = music;
        musicSource.volume = MenuManager.music;
        audioSource.volume = MenuManager.sound;
        musicSource.Play();
        StartCoroutine(Anim());
    }

    IEnumerator Animate()
    {
        color.color = StartFrom;
        image.sprite = frames[0].sprite;
        colorGroup.alpha = 1f;
        while (colorGroup.alpha >= 0.05f)
        {
            colorGroup.alpha += -0.02f / StartTimer;
            yield return new WaitForFixedUpdate();
        }
        colorGroup.alpha = 0f;
        for (int i = 0; i < frames.Length; i++)
        {
            image.sprite = frames[i].sprite;
            float timePassed = 0;
            while (timePassed < frames[i].StayingTime)
            {
                timePassed += Time.deltaTime;
            }
            //here the images
            colorGroup.alpha = 0f;
            audioSource.clip = frames[i].soundToProduce;
            audioSource.Play();
            while (colorGroup.alpha <= 0.97f)
            {
                colorGroup.alpha += +0.02f / frames[i].fadeTime / 2;
                yield return new WaitForFixedUpdate();
            }
            colorGroup.alpha = 1;
            if (i + 1 >= frames.Length)
            {
                SceneManager.LoadScene("demo_level");
            }
            else
            {
                image.sprite = frames[i+1].sprite;
                while (colorGroup.alpha >= 0.03f)
                {
                    colorGroup.alpha += -0.02f / frames[i + 1].fadeTime / 2;
                    yield return new WaitForFixedUpdate();
                }
                colorGroup.alpha = 0f;
            }
        }
    }

    
    public CanvasGroup[] groups;
    public bool[] isImage;
    public GameObject button;
    public IEnumerator Anim()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < groups.Length; i++)
        {
            if (isImage[i])
            {
                groups[i].alpha = 1;
                while (groups[i].alpha >= 0.02f)
                {
                    groups[i].alpha += -0.02f;
                    yield return new WaitForFixedUpdate();
                }
                groups[i].alpha = 0f;
            }
            else
            {
                groups[i].alpha = 0;
                while (groups[i].alpha <= 0.98f)
                {
                    groups[i].alpha += 0.02f;
                    yield return new WaitForFixedUpdate();
                }
                groups[i].alpha = 1;
            }
            yield return new WaitForSeconds(2f);
        }
        button.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("demo_level");
    }
}

[System.Serializable]
public class Frame
{
    public Sprite sprite;
    public float StayingTime;
    public Color toFade;
    public float fadeTime;
    public AudioClip soundToProduce;
}
[System.Serializable]
public class Vocals
{
    public int startAt;
    public AudioClip clip;
}
