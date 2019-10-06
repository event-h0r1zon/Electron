using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtonOrbit : MonoBehaviour
{
    public Electron electron;
    public Orbit orbit;
    private Collider2D col2D;

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
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
        if(electron.electronG != null)
            orbit.ExecuteOrbit(electron.electronG.transform, transform);
        else
            return;
    }
}
