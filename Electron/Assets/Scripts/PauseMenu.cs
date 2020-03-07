using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject pauseButton;
    [HideInInspector]
    public bool slowingDown;
    public Button restart;
    public Button restartAfterLoss;
    public Button quit;
    public Button quitAfterLoss;
    public Button proceed;

    private void Start()
    {
        proceed.onClick.AddListener(() => NextLevel());
        restart.onClick.AddListener(() => Restart());
        restartAfterLoss.onClick.AddListener(() => Restart());
        quit.onClick.AddListener(() => Quit());
        quitAfterLoss.onClick.AddListener(() => Quit());
    }

    private void Update()
    {
        if (pausePanel.activeSelf == true)
        {
            Time.timeScale = 0f;
            slowingDown = true;
        }
        else if (pausePanel.activeSelf == false)
        {
            Time.timeScale = 1f;
            slowingDown = false;
        }

        if (gameOverPanel.activeSelf == true)
            pauseButton.SetActive(false);
        else
            pauseButton.SetActive(true);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
    
    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
