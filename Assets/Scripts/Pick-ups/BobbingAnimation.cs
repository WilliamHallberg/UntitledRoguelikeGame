using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency; // Speed of the bobbing
    public float amplitude; // Height of the bobbing
    public Vector3 direction; // Direction of the bobbing
    Vector3 initialPosition; // Initial position of the object

    private void Start()
    {
        // Save the starting position of the object
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Calculate the new position of the object
        Vector3 newPosition = initialPosition + direction * Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = newPosition;
    }
}
