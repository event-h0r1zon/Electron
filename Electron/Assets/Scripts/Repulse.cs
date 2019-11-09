using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Repulse
{
    public float pushForce;
    public Vector2 PushBackVelocity(Vector2 repulsedObject, Vector2 repulsingObject)
    {
        Vector2 radiusVector = repulsedObject - repulsingObject;
        return radiusVector * pushForce;
    }
}
