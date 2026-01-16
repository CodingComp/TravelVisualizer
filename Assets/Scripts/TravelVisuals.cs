using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TravelVisuals : MonoBehaviour
{
    private SphereCollider _sphereCollider;
    private float _radius;
    public GameObject marker;
    
    public float timeBetweenMarkers = 3.0f; 
    private float lastMarkerTime;
    
    void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _radius = transform.localScale.y * _sphereCollider.radius;
    }
    
    // Use this for initialization
    void Start()
    {
        CreateCoordinateVisual(52.377956f, 4.897070f);
    }

    private void Update()
    {
        if (Time.time - lastMarkerTime > timeBetweenMarkers) {
            float lat = Random.Range(-90.0f, 90.0f);
            float lon = Random.Range(-180.0f, 180.0f);
            CreateCoordinateVisual(lat, lon);
            lastMarkerTime = Time.time;
            print(lat + " " + lon + " \n =========== \n ");
        }
    }

    private void CreateCoordinateVisual(float lat, float lon)
    {
        Instantiate(marker, transform).transform.position = CoordinateToSphere(lat, lon);;
    }

    private Vector3 CoordinateToSphere(float lat, float lon)
    {
        // calculation code taken from 
        // @miquael http://www.actionscript.org/forums/showthread.php3?p=722957#post722957
        // convert lat/long to radians

        float latitude = Mathf.PI * lat / 180;
        float longitude = Mathf.PI * lon / 180;

        // adjust position by radians	
        latitude -= 1.570795765134f; // subtract 90 degrees (in radians)

        // and switch z and y (since z is forward)
        float xPos = (_radius) * Mathf.Sin(latitude) * Mathf.Cos(longitude);
        float zPos = (_radius) * Mathf.Sin(latitude) * Mathf.Sin(longitude);
        float yPos = (_radius) * Mathf.Cos(latitude);

        //Vector3 pos = new Vector3(xPos, yPos, zPos);
        return Quaternion.Euler(transform.rotation.eulerAngles - new Vector3(0f, -90f, 0f))* new Vector3(xPos, yPos, zPos);
    }
}
