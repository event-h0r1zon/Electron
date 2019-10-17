using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current : MonoBehaviour
{
    public bool electronInCurrent = false;
    private Vector2 pushForce;
    //1 goes up, 2 goes down, 3 goes right, 4 goes left
    public int directionValue;
    public Electron electron;
    private Rigidbody2D electronRB;

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            electronInCurrent = true;
        }
        else{
            Destroy(electron.antielectronG);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Player"){
            pushForce = new Vector2(4f, 30f);
            electronRB.velocity = pushForce;
            electronInCurrent = false;
        }
    }

    void Start(){
        electronRB = electron.CheckIfAntiMatter(false);
    }

    void Update(){
        if(electronInCurrent == true && electronRB.position.x < transform.position.x){
            switch(directionValue){
                case 1:
                    pushForce = new Vector2(3f, 40f);
                    electronRB.velocity = pushForce;
                    break;
                case 2:
                    pushForce = new Vector2(3f, -40f);
                    electronRB.velocity = pushForce;
                    break;
            }
        }
        /* 
        else if(electronInCurrent == true && electronRB.position.x >= transform.position.x){
            switch(directionValue){
                case 1:
                    pushForce = new Vector2(-2f, 20f);
                    electronRB.velocity = pushForce;
                    break;
                case 2:
                    pushForce = new Vector2(-2f, -20f);
                    electronRB.velocity = pushForce;
                    break;
            }
        }
        else if(electronInCurrent == true && electronRB.position.y >= transform.position.y && directionValue > 2){
            switch(directionValue){
                case 3:
                    pushForce = new Vector2(20f, -2f);
                    electronRB.velocity = pushForce;
                    break;
                case 4:
                    pushForce = new Vector2(20f, -2f);
                    electronRB.velocity = pushForce;
                    break;
            }
        }
        else if(electronInCurrent == true && electronRB.position.y < transform.position.y && directionValue > 2){
            switch(directionValue){
                case 3:
                    pushForce = new Vector2(20f, 2f);
                    electronRB.velocity = pushForce;
                    break;
                case 4:
                    pushForce = new Vector2(20f, 2f);
                    electronRB.velocity = pushForce;
                    break;
            }
        }
        */
    }
}
