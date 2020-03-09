using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour{

    //Public Monobehaviours
    public Transform playerTarget;
    public Transform protonPosition;
    public Vector2 electronThreashold;
    public Vector2 protonThreashold;
    public GameObject gameOver;
    public Slider slider;
    public GameObject player;

    //Public Variable
    private bool move = false;

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

    void LateUpdate(){
        if(playerTarget == null || protonPosition == null)
            return;
        else
        {
            if (protonPosition.position.x - transform.position.x <= protonThreashold.x)
                move = false;
            else
                move = true;

            if (transform.position.x - playerTarget.position.x <= electronThreashold.x && move)
                transform.position = new Vector3(playerTarget.position.x + electronThreashold.x, transform.position.y, -10);
        }
    }
}
