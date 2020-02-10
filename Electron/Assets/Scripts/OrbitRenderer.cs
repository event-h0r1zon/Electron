using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitRenderer : MonoBehaviour
{
    LineRenderer lineRenderer;
    [Range(3, 200)]
    public int segments;
    public Orbit orbit;
    private float radius = (float)12.5;

    void Awake(){
        lineRenderer = GetComponent<LineRenderer>();
        CalculateOrbit();
    }

    private void CalculateOrbit(){
        Vector3[] points = new Vector3[segments + 1];
        for(int i = 0; i < segments; i++){
            Vector2 orbitPos = orbit.SetOrbitPosition((float)i/(float)segments, radius);
            points[i] = new Vector2(orbitPos.x + transform.position.x, orbitPos.y + transform.position.y);
        }
        points[segments] = points[0];

        lineRenderer.positionCount = segments + 1;
        lineRenderer.SetPositions(points);
    }
}
