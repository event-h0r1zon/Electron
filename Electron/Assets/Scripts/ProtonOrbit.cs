using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtonOrbit : MonoBehaviour
{
    public Electron electron;
    public Orbit orbit;
    private Collider2D col2D;
    private float radius;
    public bool electronInOrbit = false;

    void OnTriggerEnter2D(Collider2D other){;
        if(other.tag == "Player"){
            electronInOrbit = true;
            Vector2 radiusVector = electron.electronG.transform.position - gameObject.GetComponentInParent<Transform>().position;
            radius = radiusVector.magnitude;
            orbit.EnterCollider(electron.electronG.transform, gameObject.GetComponentInParent<Transform>(), electron.electronIsFalling);
            Destroy(col2D);
        }
    }

    void Start()
    {
        col2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        electron.CheckIfFalling(false);
        if(electron.electronG != null)
            orbit.ExecuteOrbit(electron.electronG.transform, transform, radius);
        else
            return;
    }
}
