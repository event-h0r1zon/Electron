﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public GameObject proton;
    public ParticleSystem collisionParticle;
    public ParticleSystem superpositionParticle;
    private ProtonOrbit orbitScript;
    public Electron electron;
    private float speed = 18f;
    public Repulse repulse;
    private float rotateSpeed = 200f;
    private Rigidbody2D antielectronRB;
    private Rigidbody2D electronRB;
    private float pushForce = 80;
    public float superpositionDelay = 0.1f;
    private float rotationAngle1;
    private float rotationAngle2;
    private float temporaryY;
    private int currentPositron;
    private bool repulsion = false;
    private bool destroy = false;
    private bool positronSuperposition = false;
    private GameObject superpositioner;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            destroy = true;
        else if (other.name == "Proton Sprite")
            StartCoroutine(PushBack());
        else if (other.tag == "Superpositioner")
        {
            superpositioner = other.gameObject;
            ParticleSystem.Instantiate(superpositionParticle, superpositioner.transform.position, Quaternion.identity);
            positronSuperposition = true;
        }
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
        {
            antielectronRB.gravityScale = 0f;
            antielectronRB.velocity = Vector2.zero;
        }

        else if (electron.electron != null && !repulsion && !positronSuperposition)
        {
            Vector2 direction = electronRB.position - antielectronRB.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            antielectronRB.angularVelocity = -rotateAmount * rotateSpeed;

            antielectronRB.velocity = transform.up * speed;
        }

        if (positronSuperposition)
        {
            if (superpositioner != null)
            {
                if (superpositioner.GetComponent<Superpositioner>().direction1 != null || superpositioner.GetComponent<Superpositioner>().direction2 != null)
                {
                    rotationAngle1 = -superpositioner.GetComponent<Superpositioner>().direction1.eulerAngles.z;
                    rotationAngle2 = -superpositioner.GetComponent<Superpositioner>().direction2.eulerAngles.z;
                }
            }
            StartCoroutine(setSuperPosition(superpositioner, rotationAngle1, rotationAngle2));
        }

        if (destroy)
        {
            ParticleSystem.Instantiate(collisionParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(electron.electron);
        }
    }

    IEnumerator setSuperPosition(GameObject superpositioner, float rotation1, float rotation2)
    {
        float randomRotation = Random.Range(rotation1, rotation2);
        float x = Mathf.Sin(randomRotation * Mathf.Deg2Rad) * pushForce;
        float y = Mathf.Cos(randomRotation * Mathf.Deg2Rad) * pushForce;

        Vector2 velocity = new Vector2(x, y);

        antielectronRB.velocity = velocity;

        Destroy(superpositioner);

        yield return new WaitForSeconds(0.5f);

        antielectronRB.velocity = velocity / 1.5f;
        positronSuperposition = false;
    }
}
