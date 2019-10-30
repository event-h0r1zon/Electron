using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Repulse
{
    public void FindDifference(Vector2 repulsedObject, Vector2 repulsingObject){
    }

    public Vector2 PushBackVelocity(Vector2 repulsedObject, Vector2 repulsingObject){
        float differenceInX;
        float differenceInY;

        differenceInX = Mathf.Abs(repulsedObject.x - repulsingObject.x);
        differenceInY = Mathf.Abs(repulsedObject.y - repulsingObject.y);

        if (repulsedObject.x > repulsingObject.x && repulsedObject.y > repulsingObject.y){
            if(differenceInX >= differenceInY)
                return new Vector2(10f, 6f) * 2f;
            else
                return new Vector2(6f, 14f) * 2f;
        }

        else if(repulsedObject.x < repulsingObject.x && repulsedObject.y > repulsingObject.y){
            if(differenceInX >= differenceInY)
                return new Vector2(-10f, 4f) * 2f;
            else
                return new Vector2(-4f, 14f) * 2f;
        }

        else if(repulsedObject.x > repulsingObject.x && repulsedObject.y < repulsingObject.y){
            if(differenceInX >= differenceInY)
                return new Vector2(10f, -6f) * 2f;
            else
                return new Vector2(4f, -10f) * 2f;
        }

        else if(repulsedObject.x < repulsingObject.x && repulsedObject.y < repulsingObject.y){
            if(differenceInX >= differenceInY)
                return new Vector2(-10f, -6f) * 2f;
            else
                return new Vector2(-6f, -10f) * 2f;
        }

        return Vector2.zero;

    }

}
