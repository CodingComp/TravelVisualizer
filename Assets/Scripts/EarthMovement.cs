using UnityEngine;

public class EarthMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = -1f; // Degrees per second
    public bool shouldRotate = true;

    void Update()
    {
        if (!shouldRotate) return;

        // Rotate around the y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}