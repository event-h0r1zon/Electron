using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Current 
{
    public float xCenter;

    public Current(float xCenter){
        this.xCenter = xCenter;
    }

    public Vector2 CalculateIncreasingPosition(Vector2 electronPosition, float progression){
        float angle = 60 * progression * Mathf.Deg2Rad;
        float x = electronPosition.x + xCenter/100;
        float y = electronPosition.y + (Mathf.Tan(angle) * xCenter/100);

        return new Vector2(x, y);
    }
}
