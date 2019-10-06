using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Repulse
{
    private float differenceInX;
    private float differenceInY;

    public void FindDifference(Vector2 repulsedObject, Vector2 repulsingObject){
        differenceInX = Mathf.Abs(repulsedObject.x - repulsingObject.x);
        differenceInY = Mathf.Abs(repulsedObject.y - repulsingObject.y);
    }

    public Vector2 PushBackVelocity(Vector2 repulsedObject, Vector2 repulsingObject){
        if(repulsedObject.x > repulsingObject.x && repulsedObject.y > repulsingObject.y){
            if(differenceInX >= differenceInY)
                return new Vector2(10f, 6f);
            else
                return new Vector2(6f, 14f);
        }

        else if(repulsedObject.x < repulsingObject.x && repulsedObject.y > repulsingObject.y){
            if(differenceInX >= differenceInY)
                return new Vector2(-10f, 4f);
            else
                return new Vector2(-4f, 14f);
        }

        else if(repulsedObject.x > repulsingObject.x && repulsedObject.y < repulsingObject.y){
            if(differenceInX >= differenceInY)
                return new Vector2(10f, -6f);
            else
                return new Vector2(4f, -10f);
        }

        else if(repulsedObject.x < repulsingObject.x && repulsedObject.y < repulsingObject.y){
            if(differenceInX >= differenceInY)
                return new Vector2(-10f, -6f);
            else
                return new Vector2(-6f, -10f);
        }

        return Vector2.zero;

    }

}
