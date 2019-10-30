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
    private ProtonOrbit protonOrbit;
    [SerializeField]
    private Vector2 jumpForce = new Vector2(6f, 5f);
    public Electron electron;
    public float fallingBonus = 0.1f;
    public float jumpEnergy = 0.15f;
    private bool jumped = false;
    private float energy;
    private float temporaryTime = 0f;

    private void Start(){
        rb = electron.CheckIfAntiMatter(false);
        protonOrbit = proton.GetComponent<ProtonOrbit>();
        slider.value = 1f;
        energy = 1f;
    }

    private void touchSetter(Vector2 jumpForce){
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

    private void JumpMechanic(Vector2 jumpForce, Vector2 touchPosition){
        jumped = true;
        if(Screen.width/2 < touchPosition.x)
            rb.velocity = jumpForce;
        else
            rb.velocity = new Vector2(-jumpForce.x, jumpForce.y);
        temporaryTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (energy > 0f)
        {
            if (Input.touchCount > 0 && Time.time - temporaryTime >= 0.35f && !protonOrbit.electronInOrbit)
                touchSetter(jumpForce);
        }
        else if (energy <= 0f)
            Destroy(gameObject);
    }

    private void Update(){
        electron.CheckIfFalling(false);
        EnergyBalance();
        
        slider.value = energy;
    }

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

    IEnumerator JumpDecrease()
    {
        energy -= jumpEnergy / 7;
        yield return new WaitForSeconds(0.15f);
        jumped = false;
    }
}
