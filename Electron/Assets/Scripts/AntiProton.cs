using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntiProton : MonoBehaviour
{
    private bool enteredZone = false;
    public ParticleSystem electronPushed;
    [HideInInspector]
    public bool lowerEnergy = false;
    public GameObject Pause;
    private PauseMenu pauseScript;
    public Electron electron;
    private string enteringPositron;
    private int currentPositron;
    private Rigidbody2D electronRB;
    private float positronTmpY;
    private Collider2D col2D;
    [HideInInspector]
    public Orbit orbit;
    public Repulse repulse;
    private float radius;

    private void Start()
    {
        Pause = GameObject.Find("UI Manager");
        electron.AssignGameObjects();
        if(electron.positrons.Length > 0)
            positronTmpY = electron.positrons[currentPositron].transform.position.y;
        electronRB = electron.electron.GetComponent<Rigidbody2D>();
        pauseScript = Pause.GetComponent<PauseMenu>();
        col2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enteredZone = true;
            lowerEnergy = true;
        }
        else if(other.tag == "Positron")
        {
            enteringPositron = other.name;
            if (other.name.Length >= 10)
                currentPositron = other.name[10] - '0';
            else
                currentPositron = 0;
            GetRadius(GameObject.Find(enteringPositron).transform, transform);
        }
    }

    void FixedUpdate()
    {

        if (electronRB != null && enteredZone)
        {
            ParticleSystem.Instantiate(electronPushed, electron.electron.transform.position, Quaternion.identity);
            electronRB.velocity = repulse.PushBackVelocity(electronRB.position, transform.position);
            enteredZone = false;
        }
        else
            return;
    }

    void Update()
    {
        if(GameObject.Find(enteringPositron) != null && !pauseScript.slowingDown)
            orbit.ExecuteOrbit(GameObject.Find(enteringPositron).transform, transform, radius);
    }

    void GetRadius(Transform positronPosition, Transform antiprotonPosition)
    {
        Vector2 radiusVector = positronPosition.position - antiprotonPosition.position;
        radius = radiusVector.magnitude;
        orbit.EnterCollider(positronPosition, antiprotonPosition, electron.PositronFalling(currentPositron, positronTmpY));
        Destroy(col2D);
    }
}
