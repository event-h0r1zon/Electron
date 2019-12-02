using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public GameObject proton;
    public ParticleSystem collisionParticle;
    private ProtonOrbit orbitScript;
    public Electron electron;
    private float speed = 20f;
    public Repulse repulse;
    private float rotateSpeed = 400f;
    private Rigidbody2D antielectronRB;
    private Rigidbody2D electronRB;
    private float temporaryY;
    private int currentPositron;
    private bool repulsion = false;
    private bool destroy = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            destroy = true;
        else if (other.name == "Proton Sprite")
            StartCoroutine(PushBack());
    }

    void Start()
    {
        electron.AssignGameObjects();
        if (gameObject.name.Length >= 10)
            currentPositron = gameObject.name[10] - '0';
        else
            currentPositron = 0;
        temporaryY = gameObject.transform.position.y;
        antielectronRB = electron.positrons[currentPositron].GetComponent<Rigidbody2D>();
        electronRB = electron.electron.GetComponent<Rigidbody2D>();
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
        electron.PositronFalling(currentPositron, temporaryY);

        if (electron.electron == null || orbitScript.electronInOrbit)
            antielectronRB.velocity = Vector2.zero;

        else if (electron.electron != null && !repulsion)
        {
            Vector2 direction = electronRB.position - antielectronRB.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            antielectronRB.angularVelocity = -rotateAmount * rotateSpeed;

            antielectronRB.velocity = transform.up * speed;
        }

        if (destroy)
        {
            ParticleSystem.Instantiate(collisionParticle, transform.position, Quaternion.identity);
            Destroy(electron.positrons[currentPositron]);
            Destroy(electron.electron);
        }
    }
}
