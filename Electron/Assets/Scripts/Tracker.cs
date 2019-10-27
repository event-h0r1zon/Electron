using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public Transform proton;
    public Electron electron;
    private float speed = 20f;
    public Repulse repulse;
    private float rotateSpeed = 1000f;
    private Rigidbody2D antielectronRB;
    private Rigidbody2D electronRB;
    private bool repulsion = false;
    private bool destroy = false;

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
        if (electron.electronG == null || electron.antielectronG == null)
            return;
        else if (repulsion)
        {
            antielectronRB.velocity = repulse.PushBackVelocity(antielectronRB.position, proton.position);
            repulsion = false;
        }
        else
        {
            Vector2 direction = electronRB.position - antielectronRB.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            antielectronRB.angularVelocity = -rotateAmount * rotateSpeed;

            antielectronRB.velocity = transform.up * speed;
        }
    }

    private void Update()
    {
        if (destroy)
        {
            Destroy(electron.antielectronG);
            Destroy(electron.electronG);
        }
    }
}
