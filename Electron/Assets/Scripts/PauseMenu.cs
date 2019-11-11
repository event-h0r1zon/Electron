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
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        if (panel.activeSelf == true)
        {
            slowingDown = true;
            SlowMotion();
        }
        else if (panel.activeSelf == false)
            slowingDown = false;
    }

    public void SlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
