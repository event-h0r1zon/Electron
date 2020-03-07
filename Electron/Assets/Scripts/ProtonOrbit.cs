using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtonOrbit : MonoBehaviour
{
    public Electron electron;
    [HideInInspector]
    public Orbit orbit;
    public GameObject Pause;
    private PauseMenu pauseScript;
    private Collider2D col2D;
    private float radius;
    [HideInInspector]
    public bool electronInOrbit = false;
    private bool electronFalling;
    public GameObject winPanel;

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
        pauseScript = Pause.GetComponent<PauseMenu>();
        electron.AssignGameObjects();
        col2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        electronFalling = electron.ElectronFalling();
        if (electron.electron != null && !pauseScript.slowingDown && electronInOrbit)
            orbit.ExecuteOrbit(electron.electron.transform, transform, radius);
        else
            return;
    }
}
