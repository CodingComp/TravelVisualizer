using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class FlightData
{
    public bool IsLineReaderSetup;
    public bool IsLineRendererMeshSetup;

    // Line renderer used for visualizing travel line
    public readonly LineRenderer LineRenderer;

    private MeshCollider _meshCollider;

    public readonly string Callsign;
    public readonly string Airline;

    public FlightLocationData Origin;
    public FlightLocationData Destination;

    public Transform[] GetCoordinateVisuals() => new Transform[2] {
        Origin.CoordinateVisual.transform, Destination.CoordinateVisual.transform
    };

    public FlightData(Flightroute data, GameObject originVisual, GameObject destinationVisual)
    {
        Callsign = data.callsign;
        Airline = data.airline.name;

        Origin = new FlightLocationData(
            data.origin.latitude,
            data.origin.longitude,
            data.origin.country_name,
            data.origin.municipality,
            data.origin.iata_code,
            data.origin.name,
            data.origin.elevation,
            originVisual);

        Destination = new FlightLocationData(
            data.destination.latitude,
            data.destination.longitude,
            data.destination.country_name,
            data.destination.municipality,
            data.destination.iata_code,
            data.destination.name,
            data.destination.elevation,
            destinationVisual);

        LineRenderer = Origin.CoordinateVisual.AddComponent<LineRenderer>();
        Origin.CoordinateVisual.GetComponent<CoordinateMarker>().FlightData = this;
        IsLineReaderSetup = false;
    }

    public void SetupLineRenderer(Material arcMaterial, Color arcColor, float lineWidth, int arcSegments, Camera mainCamera)
    {
        if (IsLineReaderSetup) return;

        LineRenderer.material = arcMaterial;
        LineRenderer.startColor = arcColor;
        LineRenderer.endColor = arcColor;
        LineRenderer.startWidth = lineWidth;
        LineRenderer.endWidth = lineWidth;
        LineRenderer.positionCount = arcSegments;
        LineRenderer.useWorldSpace = true;
        LineRenderer.numCapVertices = 4;

        IsLineReaderSetup = true;
    }

    public void GenerateMesh()
    {
        if (_meshCollider == null) _meshCollider = Origin.CoordinateVisual.AddComponent<MeshCollider>();

        Mesh mesh = new Mesh();
        LineRenderer.BakeMesh(mesh, true);

        var vertices = mesh.vertices;
        Origin.CoordinateVisual.transform.InverseTransformPoints(vertices);
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        _meshCollider.sharedMesh = mesh;

        IsLineRendererMeshSetup = true;
    }
}

public struct FlightLocationData
{
    public float Latitude;
    public float Longitude;

    public string Country;
    public string IataCode;
    public string Municipality;
    public string Airport;

    public int Elevation;
    public readonly GameObject CoordinateVisual;

    public FlightLocationData(float latitude, float longitude, string country, string municipality, string iata_code, string airport, int elevation, GameObject visual)
    {
        Latitude = latitude;
        Longitude = longitude;
        Country = country;
        Municipality = municipality;
        IataCode = iata_code;
        Airport = airport;
        Elevation = elevation;

        CoordinateVisual = visual;
    }
}

/// <summary>
/// Handles the data from each place the user has traveled to. Storing it and ensuring that each travel point is visualized.
/// </summary>
public class FlightHandler : MonoBehaviour
{
    public readonly List<FlightData> FlightData = new List<FlightData>();

    public Transform earthTransform;
    public FlightVisualsHelperMethods flightVisuals;

    [Header("Ui")]
    public TMP_InputField inputField;

    public void Awake()
    {
        // StartCoroutine(RequestFlightData("DL56"));
    }

    public void InputFlight()
    {
        StartCoroutine(RequestFlightData(inputField.text));
    }

    public void ImportFlights(string[] flights)
    {
        foreach (string flight in flights) {
            StartCoroutine(RequestFlightData(flight));
        }
    }

    IEnumerator RequestFlightData(string callsign)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://api.adsbdb.com/v0/callsign/" + callsign);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
            // Display error message to user
        }
        else {
            FlightDataRequest data = JsonUtility.FromJson<FlightDataRequest>(www.downloadHandler.text);
            AddFlightData(data.response.flightroute);
        }
    }

    private void AddFlightData(Flightroute data)
    {
        GameObject originVisual = flightVisuals.CreateCoordinateVisual(data.origin.latitude, data.origin.longitude);
        GameObject destinationVisual = flightVisuals.CreateCoordinateVisual(data.destination.latitude, data.destination.longitude);
        FlightData.Add(new FlightData(data, originVisual, destinationVisual));
    }
}