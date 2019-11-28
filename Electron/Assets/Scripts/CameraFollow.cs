using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour{
    public Transform playerTarget;
    public Transform protonPosition;
    public Vector2 electronThreashold;
    public Vector2 protonThreashold;
    private bool move = false;

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
