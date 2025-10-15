using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPause;
    public GameObject options;
    public GameObject exit;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeKey();
        }
    }

    private void HandleEscapeKey()
    {
        if (menuPause.activeSelf)
        {
            UIResume();
        }
        else if (options.activeSelf || exit.activeSelf)
        {
            UIBack();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        menuPause.SetActive(true);
        Time.timeScale = 0; // Pausa el juego
    }

    public void UIResume()
    {
        menuPause.SetActive(false);
        Time.timeScale = 1; // Reanuda el juego
    }

    public void UIOptions()
    {
        options.SetActive(true);
        menuPause.SetActive(false);
    }

    public void UIExit()
    {
        exit.SetActive(true);
        menuPause.SetActive(false);
    }

    public void UIBack()
    {
        menuPause.SetActive(true);
        options.SetActive(false);
        exit.SetActive(false);
    }
}
