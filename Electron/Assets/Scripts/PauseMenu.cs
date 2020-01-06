using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject pauseButton;
    [HideInInspector]
    public bool slowingDown;

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
}
