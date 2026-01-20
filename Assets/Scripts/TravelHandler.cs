using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public struct TravelData
{
    public bool LineReaderSetup;
    // Line renderer used for visualizing travel line
    public LineRenderer LineRenderer;

    public string Callsign;
    public string Airline;

    public FlightLocationData Origin;
    public FlightLocationData Destination;

    public Transform[] GetCoordinateVisuals() => new Transform[2] {
        Origin.CoordinateVisual.transform, Destination.CoordinateVisual.transform
    };

    public TravelData(Flightroute data, GameObject originVisual, GameObject destinationVisual)
    {
        Callsign = data.callsign;
        Airline = data.airline.name;

        Origin = new FlightLocationData(
            data.origin.latitude,
            data.origin.longitude,
            data.origin.country_name,
            data.origin.municipality,
            data.origin.name,
            data.origin.elevation,
            originVisual);

        Destination = new FlightLocationData(
            data.destination.latitude,
            data.destination.longitude,
            data.destination.country_name,
            data.destination.municipality,
            data.destination.name,
            data.destination.elevation,
            destinationVisual);

        LineRenderer = Origin.CoordinateVisual.AddComponent<LineRenderer>();
        ;
        LineReaderSetup = false;
    }
}

public struct FlightLocationData
{
    public float Latitude;
    public float Longitude;

    public string Country;
    public string Municipality;
    public string Airport;

    public int Elevation;
    public readonly GameObject CoordinateVisual;

    public FlightLocationData(float latitude, float longitude, string country, string municipality, string airport, int elevation, GameObject visual)
    {
        Latitude = latitude;
        Longitude = longitude;
        Country = country;
        Municipality = municipality;
        Airport = airport;
        Elevation = elevation;

        CoordinateVisual = visual;
    }
}

/// <summary>
/// Handles the data from each place the user has traveled to. Storing it and ensuring that each travel point is visualized.
/// </summary>
public class TravelHandler : MonoBehaviour
{
    public readonly List<TravelData> TravelData = new List<TravelData>();

    public Transform earthTransform;
    public TravelVisuals travelVisuals;

    [Header("Ui")]
    public TMP_InputField inputField;

    public void Awake()
    {
        StartCoroutine(RequestFlightData("DL56"));
    }

    public void InputFlight()
    {
        StartCoroutine(RequestFlightData(inputField.text));
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
            AddTravelData(data.response.flightroute);
        }
    }

    private void AddTravelData(Flightroute data)
    {
        GameObject originVisual = travelVisuals.CreateCoordinateVisual(data.origin.latitude, data.origin.longitude);
        GameObject destinationVisual = travelVisuals.CreateCoordinateVisual(data.destination.latitude, data.destination.longitude);
        TravelData.Add(new TravelData(data, originVisual, destinationVisual));
    }
}