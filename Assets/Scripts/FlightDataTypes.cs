[System.Serializable]
public class Airline
{
    public string name;
    public string icao;
    public string iata;
    public string country;
    public string country_iso;
    public string callsign;
}

[System.Serializable]
public class Destination
{
    public string country_iso_name;
    public string country_name;
    public int elevation;
    public string iata_code;
    public string icao_code;
    public float latitude;
    public float longitude;
    public string municipality;
    public string name;
}

[System.Serializable]
public class Flightroute
{
    public string callsign;
    public string callsign_icao;
    public string callsign_iata;
    public Airline airline;
    public Origin origin;
    // Airport name
    public Destination destination;
}

[System.Serializable]
public class Origin
{
    public string country_iso_name;
    public string country_name;
    public int elevation;
    public string iata_code;
    public string icao_code;
    public float latitude;
    public float longitude;
    public string municipality;
    // Airport name
    public string name;
}

[System.Serializable]
public class Response
{
    public Flightroute flightroute;
}

[System.Serializable]
public class FlightDataRequest
{
    public Response response;
}