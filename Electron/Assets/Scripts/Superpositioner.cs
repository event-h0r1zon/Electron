using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Superpositioner : MonoBehaviour
{
    [HideInInspector]
    public bool setVelocity = false;
    public Transform direction1;
    public Transform direction2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            setVelocity = true;
    }
}
