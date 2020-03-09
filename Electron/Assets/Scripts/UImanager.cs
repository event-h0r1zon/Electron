using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    //Public Monobehaviours
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject pauseButton;
    public Button[] restarts;
    public Button[] quits;
    public Button proceed;
    public Button previous;

    //Public Variable
    [HideInInspector]
    public bool slowingDown;

    private void Start()
    {
        proceed.onClick.AddListener(() => NextLevel());
        previous.onClick.AddListener(() => PreviousLevel());
        foreach(Button restart in restarts)
            restart.onClick.AddListener(() => Restart());
        foreach(Button quit in quits)
            quit.onClick.AddListener(() => Quit());
        if(SceneManager.GetActiveScene().buildIndex - 2 == 0)
            Destroy(previous.gameObject);
        if(SceneManager.GetActiveScene().buildIndex - 2 == 2){  
            float positionY = proceed.GetComponent<RectTransform>().position.y;
            previous.GetComponent<RectTransform>().position = new Vector2(previous.GetComponent<RectTransform>().position.x, positionY);
            Destroy(proceed.gameObject);
        }
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

    void PreviousLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
