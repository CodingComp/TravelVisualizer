using UnityEngine;

public class EarthMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = -1f;
    public bool shouldRotate = true;

    void Update()
    {
        if (!shouldRotate) return;
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}