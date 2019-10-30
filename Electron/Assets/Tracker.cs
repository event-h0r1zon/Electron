using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public GameObject proton;
    public ProtonOrbit orbitScript;
    public Electron electron;
    private float speed = 20f;
    public Repulse repulse;
    private float rotateSpeed = 200f;
    private Rigidbody2D antielectronRB;
    private Rigidbody2D electronRB;
    private bool repulsion = false;
    private bool destroy = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            destroy = true;
        else if (other.tag == "Proton")
            repulsion = true;
    }

    void Start()
    {
        antielectronRB = electron.CheckIfAntiMatter(true);
        electronRB = electron.CheckIfAntiMatter(false);
        orbitScript = proton.GetComponentInChildren<ProtonOrbit>();
    }

    IEnumerator PushBack()
    {
        antielectronRB.velocity = repulse.PushBackVelocity(antielectronRB.position, proton.transform.position);
        yield return new WaitForSeconds(0.7f);
        repulsion = false;
    }

    void FixedUpdate()
    {
        if (electron.antielectronG == null)
            return;
        else if (repulsion)
            StartCoroutine(PushBack());
        else if (electronRB == null || orbitScript.electronInOrbit)
            antielectronRB.velocity = Vector2.zero;
        else if (electron.electronG != null)
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
