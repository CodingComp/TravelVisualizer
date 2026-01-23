using SFB;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using System.Text;

public class ExportBtn : MonoBehaviour
{
    public FlightHandler flightHandler;

#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    public void OnClickOpen() {
        var bytes = Encoding.UTF8.GetBytes(GetAllFlightData());
        DownloadFile(gameObject.name, "OnFileDownload", "Flights.csv", bytes, bytes.Length);
    }

    // Called from browser
    public void OnFileDownload() {
        
    }

#else

    public void OnClickOpen()
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "csv", "csv");
        File.WriteAllText(path, GetAllFlightData());
    }

#endif

    private string GetAllFlightData()
    {
        string flightText = "";

        foreach (FlightData flight in flightHandler.FlightData) {
            flightText += flight.Callsign + ",";
        }

        return flightText.TrimEnd(',');
    }

}
