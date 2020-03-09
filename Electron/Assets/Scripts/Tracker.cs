using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    //Public Monobehaviours
    public GameObject proton;
    public ParticleSystem collisionParticle;
    public ParticleSystem superpositionParticle;
    public Electron electron;

    //Private Monobehaviours
    private ProtonOrbit orbitScript;
    private Rigidbody2D antielectronRB;
    private Rigidbody2D electronRB;
    private GameObject superpositioner;

    //Private Variables
    private float speed = 18f;
    private float rotateSpeed = 200f;
    private float rotationAngle1;
    private float rotationAngle2;
    private float pushForce = 80;
    private float temporaryY;
    private int currentPositron;
    private bool repulsion = false;
    private bool destroy = false;
    private bool positronSuperposition = false;
    private bool positronCollision = false;
    private bool positronInOrbit = false;

    //Public Variable
    public float superpositionDelay = 0.1f;
    public bool countdown;
    public float protonPushForce;

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
            if (superpositioner != null)
            {
                if (superpositioner.GetComponent<Superpositioner>().direction1 != null || superpositioner.GetComponent<Superpositioner>().direction2 != null)
                {
                    rotationAngle1 = -superpositioner.GetComponent<Superpositioner>().direction1.eulerAngles.z;
                    rotationAngle2 = -superpositioner.GetComponent<Superpositioner>().direction2.eulerAngles.z;
                }
            }
            positronSuperposition = true;
        }
        else if (other.tag == "Positron")
        {
            float push = 10f;
            StartCoroutine(PushOther(other.gameObject.GetComponent<Rigidbody2D>(), push));
        }
        else
            positronInOrbit = true;
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

    IEnumerator PushOther(Rigidbody2D positronRB, float pushForce)
    {
        positronCollision = true;
        Vector2 difference = positronRB.position - antielectronRB.position;
        if (positronInOrbit)
            positronRB.velocity = difference * pushForce;
        else
        {
            positronRB.velocity = -difference * pushForce;
            antielectronRB.velocity = difference * pushForce;
        }
        yield return new WaitForSeconds(0.7f);
        positronCollision = false;
    }
    
    public Vector2 PushBackVelocity(Vector2 repulsedObject, Vector2 repulsingObject)
    {
        Vector2 radiusVector = repulsedObject - repulsingObject;
        return radiusVector * protonPushForce;
    }

    IEnumerator PushBack()
    {
        positronSuperposition = false;
        antielectronRB.velocity = PushBackVelocity(antielectronRB.position, proton.transform.position);
        repulsion = true;
        yield return new WaitForSeconds(0.3f);
        repulsion = false;
    }

    private void Update()
    {
        if (countdown)
        {
            antielectronRB.gravityScale = 0f;
            antielectronRB.velocity = Vector2.zero;
        }
        else
        {
            antielectronRB.gravityScale = 5f;
            electron.PositronFalling(currentPositron, temporaryY);
            if ((electron.electron == null || orbitScript.electronInOrbit) && !positronCollision)
            {
                antielectronRB.gravityScale = 0f;
                antielectronRB.velocity = Vector2.zero;
                antielectronRB.angularVelocity = 0;
            }

            else if (electron.electron != null && !repulsion && !positronSuperposition && !positronCollision)
            {
                Vector2 direction = electronRB.position - antielectronRB.position;

                direction.Normalize();

                float rotateAmount = Vector3.Cross(direction, transform.up).z;

                antielectronRB.angularVelocity = -rotateAmount * rotateSpeed;

                antielectronRB.velocity = transform.up * speed;
            }

            if (positronSuperposition)
                StartCoroutine(setSuperPosition(superpositioner, rotationAngle1, rotationAngle2));

            if (destroy)
            {
                ParticleSystem.Instantiate(collisionParticle, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(electron.electron);
            }
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
        positronSuperposition = false;
    }
}
