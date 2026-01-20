using UnityEngine;

public class TravelVisuals : MonoBehaviour
{
    private SphereCollider _sphereCollider;
    private float _radius;
    public GameObject marker;

    void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _radius = transform.localScale.y * _sphereCollider.radius;
    }

    private void Update()
    {

    }

    public GameObject CreateCoordinateVisual(float lat, float lon)
    {
        GameObject coordinateVisual = Instantiate(marker, transform);
        coordinateVisual.transform.position = CoordinateToSphere(lat, lon);
        return coordinateVisual;
    }

    private Vector3 CoordinateToSphere(float lat, float lon)
    {
        float latitude = Mathf.PI * lat / 180;
        float longitude = Mathf.PI * lon / 180;

        // adjust position by radians	
        latitude -= 1.570795765134f; // subtract 90 degrees (in radians)

        // and switch z and y (since z is forward)
        float xPos = (_radius) * Mathf.Sin(latitude) * Mathf.Cos(longitude);
        float zPos = (_radius) * Mathf.Sin(latitude) * Mathf.Sin(longitude);
        float yPos = (_radius) * Mathf.Cos(latitude);

        return Quaternion.Euler(transform.rotation.eulerAngles - new Vector3(0f, -90f, 0f)) * new Vector3(xPos, yPos, zPos);
    }
}