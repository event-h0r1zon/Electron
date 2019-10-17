using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntiProton : MonoBehaviour
{
    private bool enteredZone = false;
    public Electron electron;
    private Rigidbody2D antielectronRB;
    private Rigidbody2D electronRB;
    private Collider2D col2D;
    public Orbit orbit;
    public Repulse repulse;
    private float radius;
    public bool electronPushedBack = false;

    private void Start(){
        electronRB = electron.CheckIfAntiMatter(false);
        antielectronRB = electron.CheckIfAntiMatter(true);
        col2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            enteredZone = true;
            repulse.FindDifference(electronRB.position, transform.position);
        }
        else{
            Vector2 radiusVector = electron.antielectronG.transform.position - gameObject.GetComponentInParent<Transform>().position;
            radius = radiusVector.magnitude;
            orbit.EnterCollider(antielectronRB.transform, gameObject.GetComponentInParent<Transform>(), electron.antielectronIsFalling);
            Destroy(col2D);
        }
        
    }

    void FixedUpdate(){

        if(electronRB != null && enteredZone){
            electronRB.velocity = repulse.PushBackVelocity(electronRB.position, transform.position);
            electronPushedBack = true;
            enteredZone = false;
        }
        else
            return;
    }

    void Update(){
        orbit.ExecuteOrbit(antielectronRB.transform, transform, radius);
        electron.CheckIfFalling(true);
    }

}
