using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject[] pages;
    int openedPage = 0;

    public void ButtonsOnMenu(int a)
    {
        pages[openedPage].SetActive(false);
        openedPage = a;
        pages[openedPage].SetActive(true);
    }
    public void OpenGame()
    {
        SceneManager.LoadScene("Cinematics");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public static float sound = 1, music = 1;
    public Slider soundSlider, musicSlider;

    public void SetThings()
    {
        sound = soundSlider.value;
        music = musicSlider.value;
        musicAuSource.volume = music;
    }

    public AudioSource musicAuSource;

    
}
