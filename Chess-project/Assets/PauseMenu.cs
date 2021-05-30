using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   
    public static bool GameIsPause = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }
    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        
        GameObject varGameObject = GameObject.Find("Board");
        varGameObject.GetComponent<TileSelector>().enabled = true;
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        
        GameObject varGameObject = GameObject.Find("Board");
        varGameObject.GetComponent<TileSelector>().enabled = false;
        Time.timeScale = 0f;
        GameIsPause = true;
    }
    public void Menu()
    {
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void QuitGame()
    {
        Debug.Log("FF 15");
        //Destroy(board.GetComponent<TileSelector>());
        //Destroy(board.GetComponent<MoveSelector>());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void WhiteWins()
    {
        Player current = GameManager.instance.currentPlayer;
        if(current.name == "white")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
        }

    }

}
