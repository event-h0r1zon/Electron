using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Electron
{
    public GameObject electronG;
    public GameObject antielectronG;
    [HideInInspector]
    public bool electronIsFalling;
    [HideInInspector]
    public bool antielectronIsFalling;
    [HideInInspector]
    [Range(0f, 1f)]
    public float energy = 1f;
    [HideInInspector]
    public float fallingBonus = 0.01f;
    [HideInInspector]
    public float jumpEnergy = 0.013f;
    private float tempY;
    public Electron(bool antimatter){
        if(antimatter)
            tempY = antielectronG.transform.position.y;
        else
            tempY = electronG.transform.position.y;
    }

    public Rigidbody2D CheckIfAntiMatter(bool antimatter){
        if(antimatter){
            Rigidbody2D antielectronRB = antielectronG.GetComponent<Rigidbody2D>();
            return antielectronRB;
        }
        else{
            Rigidbody2D electronRB = electronG.GetComponent<Rigidbody2D>();
            return electronRB;
        }
    }

    public void EnergyBalance(){
        if(energy >= 0f && energy <= 1f)
            energy = electronIsFalling ? energy + fallingBonus : energy - jumpEnergy;
        else if(energy < 0f)
            energy = 0f;
        else if(energy > 1f)
            energy = 1f;
    }

    public void CheckIfFalling(bool antimatter){
        if(antimatter){
            if(tempY > antielectronG.transform.position.y){
                antielectronIsFalling = true;
                tempY = antielectronG.transform.position.y;
            }
            else{
                antielectronIsFalling = false;
                tempY = antielectronG.transform.position.y;
            }
        }
        else{
            if(tempY > electronG.transform.position.y){
                electronIsFalling = true;
                tempY = electronG.transform.position.y;
            }
            else{
                electronIsFalling = false;
                tempY = electronG.transform.position.y;
            }
        }
    }
}
