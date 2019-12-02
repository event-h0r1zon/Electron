using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{

    //Public variables
    public float pushForce;
    [HideInInspector]
    public bool jumped = false;
    public float superpositionDuration;
    public Vector2 jumpForce = new Vector2(6f, 5f);
    public float slowdownFactor;
    public float jumpDelay = 0.35f;
    public float superpositionDelay = 0.1f;
    public float startDelay = 3f;

    //Private variables
    private float temporaryJumpTime = 0f;
    private float rotationAngle1;
    private float rotationAngle2;
    private float temporarySuperPositionTime = 0f;
    private bool inSuperposition = false;

    //Public monobehaviors
    public GameObject proton;
    public ParticleSystem jumpingParticle;
    public GameObject[] superpositioners;
    public List<Superpositioner> superpositionerScripts;

    //Private monobehaviors
    private ProtonOrbit protonOrbit;
    private Rigidbody2D rb;

    //Runtime functions

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        protonOrbit = proton.GetComponent<ProtonOrbit>();
        superpositioners = GameObject.FindGameObjectsWithTag("Superpositioner");
        for (int i = 0; i < superpositioners.Length; i++)
            superpositionerScripts.Add(superpositioners[i].GetComponent<Superpositioner>());
    }

    private void Update()
    {
        for (int i = 0; i < superpositioners.Length; i++)
        {
            if (superpositionerScripts[i].setVelocity && Time.time - temporarySuperPositionTime >= superpositionDelay)
            {
                if (superpositionerScripts[i].direction1 != null || superpositionerScripts[i].direction2 != null)
                {
                    rotationAngle1 = -superpositionerScripts[i].direction1.eulerAngles.z;
                    rotationAngle2 = -superpositionerScripts[i].direction2.eulerAngles.z;
                }
                inSuperposition = true;
                StartCoroutine(setSuperPosition(i, rotationAngle1, rotationAngle2));
            }
        }
        if (Input.touchCount > 0 && Time.time - temporaryJumpTime >= jumpDelay && !protonOrbit.electronInOrbit && !inSuperposition)
            touchSetter(jumpForce);
    }
    private void touchSetter(Vector2 jumpForce)
    {
        Touch touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                JumpMechanic(jumpForce, touch.position);
                break;
            case TouchPhase.Moved:
                break;
            case TouchPhase.Ended:
                break;
        }
    }

    private void JumpMechanic(Vector2 jumpForce, Vector2 touchPosition)
    {
        jumped = true;
        ParticleSystem.Instantiate(jumpingParticle, transform.position, Quaternion.identity);
        if (Screen.width / 2 < touchPosition.x)
            rb.velocity = jumpForce;
        else
            rb.velocity = new Vector2(-jumpForce.x, jumpForce.y);
        temporaryJumpTime = Time.time;
    }

    //Coroutines

    IEnumerator setSuperPosition(int currentSuperpositioner, float rotation1, float rotation2)
    {
        float randomRotation = Random.Range(rotation1, rotation2);
        float x = Mathf.Sin(randomRotation * Mathf.Deg2Rad) * pushForce;
        float y = Mathf.Cos(randomRotation * Mathf.Deg2Rad) * pushForce;

        Vector2 velocity = new Vector2(x, y);

        rb.velocity = velocity;
        temporarySuperPositionTime = Time.time;

        Destroy(superpositioners[currentSuperpositioner]);

        yield return new WaitForSeconds(superpositionDuration);

        rb.velocity = velocity / slowdownFactor;
        superpositionerScripts[currentSuperpositioner].setVelocity = false;
        inSuperposition = false;
    }
}

