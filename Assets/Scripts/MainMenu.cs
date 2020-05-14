using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject pauseMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Escape))                   //Hvis man trykker på "Venstreklik"
        {
            if(pauseMenu.activeInHierarchy == true)
            {
                UnPause();
            }
            else
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
        }
    }
}
