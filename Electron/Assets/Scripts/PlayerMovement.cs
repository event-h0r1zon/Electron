using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour{

    private Rigidbody2D rb;
    [SerializeField]
    private Vector2 jumpForce = new Vector2(6f, 5f);
    public Electron electron;

    private void Start(){
        rb = electron.CheckIfAntiMatter(false);
    }

    private void touchSetter(Vector2 jumpForce){
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            switch (touch.phase){
                case TouchPhase.Began:
                    JumpMechanic(jumpForce, touch.position);
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Ended:
                    break;
            }
        }
    }

    private void JumpMechanic(Vector2 jumpForce, Vector2 touchPosition){
        if(Screen.width/2 < touchPosition.x)
            rb.velocity = jumpForce;
        else
            rb.velocity = new Vector2(-jumpForce.x, jumpForce.y);
    }

    private void FixedUpdate(){
        touchSetter(jumpForce);
    }
}
