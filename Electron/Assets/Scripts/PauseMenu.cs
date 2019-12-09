using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject panel;
    public float slowdownFactor;
    public float slowdownLength;
    [HideInInspector]
    public bool slowingDown;

    private void Update()
    {
        if (panel.activeSelf == true)
        {
            Time.timeScale = 0f;
            slowingDown = true;
        }
        else if (panel.activeSelf == false)
        {
            Time.timeScale = 1f;
            slowingDown = false;
        }
    }
}
