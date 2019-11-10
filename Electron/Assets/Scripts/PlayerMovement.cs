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
    public float energyDecreaseDuration = 0.5f;
    public float jumpDelay = 0.35f;
    public float superpositionDelay = 0.1f;
    public float startDelay = 3f;

    //Private variables
    private float temporaryJumpTime = 0f;
    private float temporarySuperPositionTime = 0f;

    //Public monobehaviors
    public Slider slider;
    public GameObject proton;
    public GameObject superpositioner;

    //Private monobehaviors
    private ProtonOrbit protonOrbit;
    private Superpositioner superpositionerScript;
    private Rigidbody2D rb;

    //Runtime functions

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        protonOrbit = proton.GetComponent<ProtonOrbit>();
        superpositionerScript = superpositioner.GetComponent<Superpositioner>();
    }

    private void Update()
    {
        if (Time.time >= startDelay)
        {
            rb.gravityScale = 5f;
            if (slider.value > 0f)
            {
                if (Input.touchCount > 0 && Time.time - temporaryJumpTime >= jumpDelay && !protonOrbit.electronInOrbit && !superpositionerScript.setVelocity)
                    touchSetter(jumpForce);
            }
            else if (slider.value <= 0f)
                Destroy(gameObject);

            if (superpositionerScript.setVelocity && Time.time - temporarySuperPositionTime >= superpositionDelay)
                StartCoroutine(setSuperPosition());

            EnergyBalance();
        }
        else
            rb.gravityScale = 0f;
    }

    //Reusable functions

    private void EnergyBalance()
    {
        if (slider.value > 1f)
            slider.value = 1f;
        else if (slider.value < 0f)
            slider.value = 0f;
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

    IEnumerator setSuperPosition()
    {
        float randomRotation = Random.Range(rotation1, rotation2);
        float x = Mathf.Sin(randomRotation * Mathf.Deg2Rad) * pushForce;
        float y = Mathf.Cos(randomRotation * Mathf.Deg2Rad) * pushForce;

        Vector2 velocity = new Vector2(x, y);

        rb.velocity = velocity;
        temporarySuperPositionTime = Time.time;

        Destroy(superpositioner);

        yield return new WaitForSeconds(superpositionDuration);

        rb.velocity = velocity / slowdownFactor;
        superpositionerScript.setVelocity = false;
    }
}

