using System;
using UnityEngine;

public class EarthMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = -1f; // Degrees per second

    void Update()
    {
        // Rotate around the y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}