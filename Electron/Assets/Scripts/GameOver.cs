using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject gameOver;
    public Slider slider;
    public GameObject player;
    private bool over = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            over = true;
    }

    private void Update()
    {
        if (player == null || over == true)
        {
            slider.value -= 0.02f;
            gameOver.SetActive(true);
        }
    }
}
