using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject gameOver;
    public Slider slider;
    public GameObject player;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Destroy(player);
    }

    private void Update()
    {
        if (player == null)
        {
            slider.value -= 0.02f;
            gameOver.SetActive(true);
        }
    }
}
