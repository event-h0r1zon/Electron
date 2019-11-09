using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour{

    //Public variables
    public float rotation1;
    public float rotation2;
    public float pushForce;
    public float fallingBonus = 0.1f;
    public float superpositionDuration;
    public float jumpEnergy = 0.15f;
    public float antiprotonEnergyDecrease;
    public Vector2 jumpForce = new Vector2(6f, 5f);
    public float slowdownFactor;
    public float energyDecreaseDuration = 0.5f;
    public float jumpDelay = 0.35f;
    public float superpositionDelay = 0.1f;
    public float startDelay = 3f;

    //Private variables
    private bool jumped = false;
    private float temporaryJumpTime = 0f;
    private float temporarySuperPositionTime = 0f;
    private float energy;

    //Public monobehaviors
    public Slider slider;
    public GameObject proton;
    public GameObject quantumEnergy;
    public GameObject antiproton;
    public GameObject superpositioner;
    public Electron electron;

    //Private monobehaviors
    private ProtonOrbit protonOrbit;
    private EnergyAdder energyAdder;
    private AntiProton antiprotonScript;
    private Superpositioner superpositionerScript;
    private Rigidbody2D rb;

    //Runtime functions

    private void Start(){

        rb = electron.CheckIfAntiMatter(false);
        protonOrbit = proton.GetComponent<ProtonOrbit>();
        antiprotonScript = antiproton.GetComponent<AntiProton>();
        superpositionerScript = superpositioner.GetComponent<Superpositioner>();
        energyAdder = quantumEnergy.GetComponent<EnergyAdder>();

        slider.value = 1f;
        energy = 1f;
    }

    private void Update(){
        if (Time.time >= startDelay)
        {
            rb.gravityScale = 5f;
            if (energy > 0f)
            {
                if (Input.touchCount > 0 && Time.time - temporaryJumpTime >= jumpDelay && !protonOrbit.electronInOrbit && !superpositionerScript.setVelocity)
                    touchSetter(jumpForce);
            }
            else if (energy <= 0f)
                Destroy(gameObject);


            if (superpositionerScript.setVelocity && Time.time - temporarySuperPositionTime >= superpositionDelay)
                StartCoroutine(setSuperPosition());

            if (energyAdder.booster)
                StartCoroutine(AddEnergy());

            if (antiprotonScript.lowerEnergy)
                StartCoroutine(LowerEnergy());

            electron.CheckIfFalling(false);
            EnergyBalance();

            slider.value = energy;
        }
        else
            rb.gravityScale = 0f;
    }

    //Reusable functions

    private void EnergyBalance(){
        if (energy > 1f)
            energy = 1f;
        else if (energy < 0f)
            energy = 0f;
        else if (jumped)
            StartCoroutine(JumpDecrease());
        else if (protonOrbit.electronInOrbit)
            energy += fallingBonus / 10;
        else if (electron.electronIsFalling)
            energy += fallingBonus / 10;
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

    IEnumerator JumpDecrease()
    {
        energy -= jumpEnergy / 7;
        yield return new WaitForSeconds(0.15f);
        jumped = false;
    }

    IEnumerator setSuperPosition()
    {
        float randomRotation = Random.Range(rotation1, rotation2);
        float x = Mathf.Sin(randomRotation * Mathf.Deg2Rad) * pushForce;
        float y = Mathf.Cos(randomRotation * Mathf.Deg2Rad) * pushForce;
        energy -= 0.005f;

        Vector2 velocity = new Vector2(x, y);

        rb.velocity = velocity;
        temporarySuperPositionTime = Time.time;

        Destroy(superpositioner);

        yield return new WaitForSeconds(superpositionDuration);

        rb.velocity = velocity/slowdownFactor; 
        superpositionerScript.setVelocity = false;
    }

    IEnumerator AddEnergy()
    {
        energy += energyAdder.energyBonus;
        Destroy(quantumEnergy);
        yield return new WaitForSeconds(0.5f);
        energyAdder.booster = false;
    }

    IEnumerator LowerEnergy()
    {
        energy -= 0.03f;
        yield return new WaitForSeconds(energyDecreaseDuration);
        antiprotonScript.lowerEnergy = false;
    }
}
