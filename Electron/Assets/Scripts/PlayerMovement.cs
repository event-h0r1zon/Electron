﻿using System.Collections;
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
    public float jumpDelay = 0.3f;
    public float superpositionDelay = 0.1f;
    public float startDelay = 3f;
    public bool countdown;

    //Private variables
    private float temporaryJumpTime = 0f;
    private float rotationAngle1;
    private float rotationAngle2;
    private float temporarySuperPositionTime = 0f;
    private bool inSuperposition = false;
    private bool pressedPause = false;
    private float numberOfJumpsR;
    private float numberOfJumpsL;

    //Public monobehaviors
    public ParticleSystem jumpingParticle;
    public GameObject[] superpositioners;
    public Button pause;
    public List<Superpositioner> superpositionerScripts;

    //Private monobehaviors
    public GameObject proton;
    private ProtonOrbit protonOrbit;
    private Rigidbody2D rb;

    //Runtime functions

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        proton = GameObject.Find("Proton Sprite");
        protonOrbit = proton.GetComponent<ProtonOrbit>();
        superpositioners = GameObject.FindGameObjectsWithTag("Superpositioner");
        numberOfJumpsL = 0;
        numberOfJumpsR = 0;
        for (int i = 0; i < superpositioners.Length; i++)
            superpositionerScripts.Add(superpositioners[i].GetComponent<Superpositioner>());
    }
    private void Update()
    {
        if (countdown)
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }
        else if(!countdown)
        {
            rb.gravityScale = 5.5f;
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
            if (Input.touchCount > 0 && !pressedPause)
                touchSetter(jumpForce);
            if (protonOrbit.electronInOrbit)
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
            }
        }
    }

    public void Pause()
    {
        pressedPause = true;
    }

    public void Resume()
    {
        pressedPause = false;
    }

    private void touchSetter(Vector2 jumpForce)
    {
        Touch touch = Input.GetTouch(0);
        Vector2 tpos = touch.position;
        RectTransform pauseRT = pause.GetComponent<RectTransform>();
        bool pressedButton = RectTransformUtility.RectangleContainsScreenPoint(pauseRT, tpos);
        
        switch (touch.phase)
        {
            case TouchPhase.Began:
                if(!pressedButton)
                    JumpMechanic(jumpForce, tpos);
                break;
            case TouchPhase.Moved:
                break;
            case TouchPhase.Ended:
                break;
        }
    }

    private void JumpMechanic(Vector2 jumpForce, Vector2 touchPosition)
    {
        if (Time.time - temporaryJumpTime >= jumpDelay && !protonOrbit.electronInOrbit && !inSuperposition)
        {
            jumped = true;
            ParticleSystem.Instantiate(jumpingParticle, transform.position, Quaternion.identity);
            if (Screen.width / 2 < touchPosition.x)
            {
                if (numberOfJumpsR > 0)
                {
                    rb.velocity = jumpForce*Mathf.Log(10 + numberOfJumpsR, 10);
                    numberOfJumpsR++;
                }
                else
                {
                    rb.velocity = jumpForce;
                    numberOfJumpsR = 0;
                    numberOfJumpsL = 0;
                    numberOfJumpsR++;
                }
            }
            else
            {
                if (numberOfJumpsL > 0)
                {
                    rb.velocity = new Vector2(-jumpForce.x, jumpForce.y) * Mathf.Log(10 + numberOfJumpsL, 10);
                    numberOfJumpsL++;
                }
                else
                {
                    rb.velocity = new Vector2(-jumpForce.x, jumpForce.y);
                    numberOfJumpsL = 0;
                    numberOfJumpsR = 0;
                    numberOfJumpsL++;
                }
            }
            temporaryJumpTime = Time.time;
        }
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