using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public Transform proton;
    public Electron electron;
    public Repulse repulse;
    private Rigidbody2D antielectronRB;
    private Rigidbody2D electronRB;
    private bool repulsion;
    private bool destroy = false;
    public float G = 1;

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player")
            destroy = true;
        else{
            repulse.FindDifference(antielectronRB.position, proton.position);
            repulsion = true;
        }
    }

    void Start(){
        antielectronRB = electron.CheckIfAntiMatter(true);
        electronRB = electron.CheckIfAntiMatter(false);
    }

    void FixedUpdate(){
        if(electron.electronG == null || electron.antielectronG == null)
            return;
        else{
            Vector2 difference = electronRB.position - antielectronRB.position;
            float radius = difference.magnitude;
            float gravitationalForce = G*(antielectronRB.mass*electronRB.mass)/Mathf.Pow(radius, 2);
            Vector2 force = difference.normalized * gravitationalForce;

            antielectronRB.AddForce(force, ForceMode2D.Force);

            if(destroy){
                Destroy(electron.antielectronG);
                Destroy(electron.electronG);  
            }

            else if(repulsion == true){
                antielectronRB.velocity = repulse.PushBackVelocity(antielectronRB.position, proton.position);
                repulsion = false;
            }
        }
        electron.CheckIfFalling(true);
    }
}
