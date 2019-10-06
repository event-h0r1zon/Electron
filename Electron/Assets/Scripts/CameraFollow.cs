using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour{
    public Transform playerTarget;
    public Vector2 offset;

    void Start(){
        offset = transform.position - playerTarget.position;
    }
    void LateUpdate(){
        if(playerTarget == null)
            return;
        else{
            transform.position = new Vector3(playerTarget.position.x + offset.x, transform.position.y, -10);
        }
    }
}
