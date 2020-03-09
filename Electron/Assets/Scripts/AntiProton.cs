using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntiProton : MonoBehaviour
{
    //Public Variables
    public bool lowerEnergy = false;
    public float pushForce;

    //Public Monobehaviours
    public ParticleSystem electronPushed;
    public Electron electron;
    [HideInInspector]
    public Orbit orbit;
    [HideInInspector]
    public GameObject Pause;

    //Private Variables
    private bool enteredZone = false;
    private string enteringPositron;
    private int currentPositron;
    private float positronTmpY;
    private float radius;

    //Private Monobehaviours
    private UImanager uiScript;
    private Rigidbody2D electronRB;
    private Collider2D col2D;

    private void Start()
    {
        Pause = GameObject.Find("UI Manager");
        electron.AssignGameObjects();
        if(electron.positrons.Length > 0)
            positronTmpY = electron.positrons[currentPositron].transform.position.y;
        electronRB = electron.electron.GetComponent<Rigidbody2D>();
        uiScript = Pause.GetComponent<UImanager>();
        col2D = GetComponent<Collider2D>();
    }
    
    public Vector2 PushBackVelocity(Vector2 repulsedObject, Vector2 repulsingObject)
    {
        Vector2 radiusVector = repulsedObject - repulsingObject;
        return radiusVector * pushForce;
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
            electronRB.velocity = PushBackVelocity(electronRB.position, transform.position);
            enteredZone = false;
        }
        else
            return;
    }

    void Update()
    {
        if(GameObject.Find(enteringPositron) != null && !uiScript.slowingDown)
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
