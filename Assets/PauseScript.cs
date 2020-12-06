using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseScript : MonoBehaviour
{

    public GameObject pauseMenuUI;
    public GameManager gameManager;

    public static bool gameIsPaused;


    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
		
	}


    public void Resume()
    {
        gameManager.ReturnControl();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName != "Menu" && sceneName != "Hub") {
            gameManager.TakeAwayControl();
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            gameIsPaused = true;
        }
    }


    public void QuitGame()
    {
        Debug.Log("Qutting Game..");
        Application.Quit();
    }
}
