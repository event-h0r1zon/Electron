using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntiProton : MonoBehaviour
{
    private bool enteredZone = false;
    public Electron electron;
    public GameObject[] positrons;
    private Rigidbody2D electronRB;
    private Collider2D col2D;
    private int enteredPositron;
    public Orbit orbit;
    public Repulse repulse;
    private float radius;
    public bool electronPushedBack = false;

    private void Start(){
        electronRB = electron.CheckIfAntiMatter(false);
        col2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player")
            enteredZone = true;
        else if(other.tag == "Positron")
        {
            GetRadius(positrons[0].transform.position, GetComponentInParent<Transform>(), 0);
            enteredPositron = 1;
        }
        else if(other.tag == "Positron-1")
        {
            GetRadius(positrons[1].transform.position, GetComponentInParent<Transform>(), 1);
            enteredPositron = 2;
        }
        else if (other.tag == "Positron-2")
        {
            GetRadius(positrons[2].transform.position, GetComponentInParent<Transform>(), 2);
            enteredPositron = 3;
        }
        else if (other.tag == "Positron-3")
        {
            GetRadius(positrons[3].transform.position, GetComponentInParent<Transform>(), 3);
            enteredPositron = 4;
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
        if (electron.antielectronG != null)
        {
            switch (enteredPositron)
            {
                case 1:
                    orbit.ExecuteOrbit(positrons[0].transform, transform, radius);
                    break;
                case 2:
                    orbit.ExecuteOrbit(positrons[1].transform, transform, radius);
                    break;
                case 3:
                    orbit.ExecuteOrbit(positrons[2].transform, transform, radius);
                    break;
                case 4:
                    orbit.ExecuteOrbit(positrons[3].transform, transform, radius);
                    break;

            }
        }
        electron.CheckIfFalling(true);
    }

    void GetRadius(Vector2 positronPosition, Transform antiprotonPosition, int certainPositron)
    {
        Vector2 antiprotonVector = new Vector2(antiprotonPosition.position.x, antiprotonPosition.position.y);
        Vector2 radiusVector = positronPosition - antiprotonVector;
        radius = radiusVector.magnitude;
        orbit.EnterCollider(positrons[certainPositron].transform, antiprotonPosition, electron.antielectronIsFalling);
        Destroy(col2D);
    }
}
