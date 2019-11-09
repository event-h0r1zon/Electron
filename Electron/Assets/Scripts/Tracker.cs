using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public GameObject proton;
    private ProtonOrbit orbitScript;
    public Electron electron;
    private float speed = 20f;
    public Repulse repulse;
    private float rotateSpeed = 600f;
    private Rigidbody2D antielectronRB;
    private Rigidbody2D electronRB;
    private bool repulsion = false;
    private bool destroy = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            destroy = true;
        else if (other.tag == "Proton")
            StartCoroutine(PushBack());
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
        repulsion = true;
        yield return new WaitForSeconds(1f);
        repulsion = false;
    }

    private void Update()
    {
        if (electronRB == null || orbitScript.electronInOrbit)
            antielectronRB.velocity = Vector2.zero;

        else if (electron.electronG != null && !repulsion)
        {
            Vector2 direction = electronRB.position - antielectronRB.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            antielectronRB.angularVelocity = -rotateAmount * rotateSpeed;

            antielectronRB.velocity = transform.up * speed;
        }

        if (destroy)
        {
            Destroy(electron.antielectronG);
            Destroy(electron.electronG);
        }
    }
}
