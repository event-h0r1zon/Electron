using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current : MonoBehaviour
{
    public bool electronInCurrent = false;
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
            electronInCurrent = false;
        }
    }

    void Start(){
        electronRB = electron.CheckIfAntiMatter(false);
    }

    void FixedUpdate(){
        if(electronInCurrent)
            electronRB.velocity = MovementForce();
    }

    Vector2 MovementForce(){

        Vector2 pushForce = Vector2.zero;

        switch(directionValue){
            case 1:
                pushForce = new Vector2(0f, 40f);
                break;
            case 2:
                pushForce = new Vector2(0f, -40f);
                break;
            case 3:
                pushForce = new Vector2(40f, 0f);
                break;
            case 4:
                pushForce = new Vector2(-40f, 0f);
                break;
        }

        return pushForce;
    }
}
