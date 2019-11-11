using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{

    //Public variables
    public float rotation1;
    public float rotation2;
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
    private float temporarySuperPositionTime = 0f;
    private bool inSuperposition = false;

    //Public monobehaviors
    public GameObject proton;
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
        if (Time.time >= startDelay)
        {
            for(int i = 0; i < superpositioners.Length; i++)
            {
                if (superpositionerScripts[i].setVelocity && Time.time - temporarySuperPositionTime >= superpositionDelay)
                {
                    inSuperposition = true;
                    StartCoroutine(setSuperPosition(i));
                }
            }
            rb.gravityScale = 5f;
            if (Input.touchCount > 0 && Time.time - temporaryJumpTime >= jumpDelay && !protonOrbit.electronInOrbit && !inSuperposition)
                touchSetter(jumpForce);
        }
        else
            rb.gravityScale = 0f;
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
        if (Screen.width / 2 < touchPosition.x)
            rb.velocity = jumpForce;
        else
            rb.velocity = new Vector2(-jumpForce.x, jumpForce.y);
        temporaryJumpTime = Time.time;
    }

    //Coroutines

    IEnumerator setSuperPosition(int currentSuperpositioner)
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

