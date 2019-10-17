using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour{

    private Rigidbody2D rb;
    public Slider slider;
    public GameObject proton;
    public GameObject antiproton;
    private AntiProton antiProtonScript;
    private ProtonOrbit protonOrbit;
    [SerializeField]
    private Vector2 jumpForce = new Vector2(6f, 5f);
    public Electron electron;
    private float energy;

    private void Start(){
        rb = electron.CheckIfAntiMatter(false);
        protonOrbit = proton.GetComponent<ProtonOrbit>();
        antiProtonScript = antiproton.GetComponent<AntiProton>();
        slider.value = 1f;
        energy = 1f;
    }

    private void touchSetter(Vector2 jumpForce){
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            switch (touch.phase){
                case TouchPhase.Began:
                    JumpMechanic(jumpForce, touch.position);
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Ended:
                    break;
            }
        }
    }

    private void JumpMechanic(Vector2 jumpForce, Vector2 touchPosition){
        if(Screen.width/2 < touchPosition.x)
            rb.velocity = jumpForce;
        else
            rb.velocity = new Vector2(-jumpForce.x, jumpForce.y);
    }

    private void FixedUpdate(){
        if(energy > 0f)
            touchSetter(jumpForce);
        else{
            Destroy(gameObject);
        }
    }

    private void Update(){
        electron.CheckIfFalling(false);
        if(!antiProtonScript.electronPushedBack)
            EnergyBalance();
        else{
            energy -= 0.2f;
            antiProtonScript.electronPushedBack = false;
        }
        slider.value = energy;
    }

    private void EnergyBalance(){
        if(energy >= 0f && energy <= 1f && !protonOrbit.electronInOrbit)
            energy = electron.electronIsFalling ? energy + electron.fallingBonus/12 : energy - electron.jumpEnergy/10;
        else if(protonOrbit.electronInOrbit)
            energy += electron.fallingBonus/10; 
        else if(energy > 1f)
            energy = 1f;
        else if(energy < 0f)
            energy = 0f;
    }
}
