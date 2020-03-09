using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    //Public Monobehaviours
    public Button play;
    public Button quit;

    private void Start()
    {
        play.onClick.AddListener(() => ChangeScene());
        quit.onClick.AddListener(() => Quit());
    }

    void Quit()
    {
        Application.Quit();
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("Level Select");
    }

}
