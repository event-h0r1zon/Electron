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
            slowingDown = true;
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
        }
        else if (panel.activeSelf == false)
            slowingDown = false;
    }
}
