using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Orbit
{
    public bool enteredOrbit = false;
    private bool setProgression = false;
    private bool progressDirector;
    private float orbitProgression = 0f;
    public float radius;

    public Orbit(float radius){
        this.radius = radius;
    }

    public void EnterCollider(Transform electronPos, Transform protonPos, bool isFalling){
        enteredOrbit = true;
        setProgression = true;
        Debug.Log(isFalling);
        if(electronPos.position.x <= protonPos.position.x)
            progressDirector = isFalling ? true : false;
        else
            progressDirector = isFalling ? false : true;
    }

    public void ExecuteOrbit(Transform electronPos, Transform protonPos){
        if(enteredOrbit){
            if(setProgression){
                orbitProgression = SetOrbitProgression(electronPos.position, protonPos.position);
                setProgression = false;
            }
            else{
                SetElectronPosition(electronPos, protonPos);
                orbitProgression = (progressDirector) ? orbitProgression + 0.011f : orbitProgression - 0.011f;
                if(orbitProgression >= 1f)
                    orbitProgression = 0f;
            }
        }
        else
            return;
    }

    public Vector2 SetOrbitPosition(float angleDesider){
        
        float angle = Mathf.Deg2Rad * 360 * angleDesider;

        float x = (Mathf.Sin(angle) * radius);
        float y = (Mathf.Cos(angle) * radius);

        return new Vector2(x, y);
    }

    public float SetOrbitProgression(Vector2 objectEnteringPosition, Vector2 protonPosition){
        float angle = 0f;

        if(objectEnteringPosition.y > protonPosition.y)
            angle = Mathf.Asin((objectEnteringPosition.x - protonPosition.x)/radius) * Mathf.Rad2Deg;

        else if(objectEnteringPosition.y < protonPosition.y && objectEnteringPosition.x > protonPosition.x)
            angle = Mathf.Acos((objectEnteringPosition.y - protonPosition.y)/radius) * Mathf.Rad2Deg;

        else if(objectEnteringPosition.y < protonPosition.y && objectEnteringPosition.x < protonPosition.x)
            angle = -Mathf.Acos((objectEnteringPosition.y - protonPosition.y)/radius) * Mathf.Rad2Deg;

        float progression = angle/360;
        return progression;
    }

    public void SetElectronPosition(Transform electronPos, Transform protonPos){
        Vector2 orbitPosition = SetOrbitPosition(orbitProgression);
        electronPos.position = new Vector2(orbitPosition.x + protonPos.position.x, orbitPosition.y + protonPos.position.y);
    }
}
