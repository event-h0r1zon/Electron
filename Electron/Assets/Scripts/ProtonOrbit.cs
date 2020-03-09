using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtonOrbit : MonoBehaviour
{
    //Public Monobehaviours
    public Electron electron;
    [HideInInspector]
    public Orbit orbit;
    public GameObject Pause;
    public GameObject winPanel;
    
    //Private Monobehaviours
    private UImanager uiScript;
    private Collider2D col2D;
    
    //Private Variables
    private float radius;
    private bool electronFalling;

    //Public Variables
    [HideInInspector]
    public bool electronInOrbit = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            electronInOrbit = true;
            Vector2 radiusVector = electron.electron.transform.position - gameObject.GetComponentInParent<Transform>().position;
            radius = radiusVector.magnitude;
            orbit.EnterCollider(electron.electron.transform, gameObject.GetComponentInParent<Transform>(), electronFalling);
            winPanel.SetActive(true);
            Destroy(col2D);
        }
    }

    void Start()
    {
        Pause = GameObject.Find("UI Manager");
        uiScript = Pause.GetComponent<UImanager>();
        electron.AssignGameObjects();
        col2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        electronFalling = electron.ElectronFalling();
        if (electron.electron != null && !uiScript.slowingDown && electronInOrbit)
            orbit.ExecuteOrbit(electron.electron.transform, transform, radius);
        else
            return;
    }
}
