﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Escape))                   //Hvis man trykker på "Venstreklik"
        {
            SceneManager.LoadScene(4);
        }
    }
}
