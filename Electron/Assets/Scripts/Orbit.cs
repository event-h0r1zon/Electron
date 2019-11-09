using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Orbit
{
    [HideInInspector]
    public bool enteredOrbit = false;
    private bool setProgression = false;
    private bool progressDirector;
    private float orbitProgression = 0f;

    public void EnterCollider(Transform electronPos, Transform protonPos, bool isFalling){
        enteredOrbit = true;
        setProgression = true;
        if(electronPos.position.x <= protonPos.position.x)
            progressDirector = isFalling ? false : true;
        else
            progressDirector = isFalling ? true : false;
    }

    public void ExecuteOrbit(Transform electronPos, Transform protonPos, float radius)
    {
        if(enteredOrbit){
            if(setProgression){
                orbitProgression = SetOrbitProgression(electronPos.position, protonPos.position);
                setProgression = false;
            }
            else{
                SetElectronPosition(electronPos, protonPos, radius);
                orbitProgression = (progressDirector) ? orbitProgression + 0.011f : orbitProgression - 0.011f;
                if(orbitProgression >= 1f)
                    orbitProgression = 0f;
            }
        }
        else
            return;
    }

    public Vector2 SetOrbitPosition(float angleDesider, float radius){
        
        float angle = Mathf.Deg2Rad * 360 * angleDesider;

        float x = (Mathf.Sin(angle) * radius);
        float y = (Mathf.Cos(angle) * radius);

        return new Vector2(x, y);
    }

    public float SetOrbitProgression(Vector2 objectEnteringPosition, Vector2 protonPosition){
        
        Vector2 radiusVector = objectEnteringPosition - protonPosition;
        float radius = radiusVector.magnitude;
        float angle = 0;

        //Base Zone
        if(objectEnteringPosition.y >= protonPosition.y && objectEnteringPosition.x >= protonPosition.x)
            angle = (Mathf.Asin(radiusVector.x/radius)) * Mathf.Rad2Deg;
        // 90 degree zone
        else if(objectEnteringPosition.y > protonPosition.y && objectEnteringPosition.x < protonPosition.x)
            angle = (Mathf.Asin(radiusVector.x/radius)) * Mathf.Rad2Deg;
        //180 degree zone
        else if(objectEnteringPosition.y < protonPosition.y && objectEnteringPosition.x < protonPosition.x)
            angle = (-Mathf.Acos(radiusVector.y/radius)) * Mathf.Rad2Deg;
        //270 degree zone
        else if(objectEnteringPosition.y <= protonPosition.y && objectEnteringPosition.x >= protonPosition.x)
            angle = (Mathf.Acos(radiusVector.y/radius)) * Mathf.Rad2Deg;
        
        float progression = angle/360;
        return progression;
    }

    public void SetElectronPosition(Transform electronPos, Transform protonPos, float radius){
        Vector2 orbitPosition = SetOrbitPosition(orbitProgression, radius);
        electronPos.position = new Vector2(orbitPosition.x + protonPos.position.x, orbitPosition.y + protonPos.position.y);
    }
}
