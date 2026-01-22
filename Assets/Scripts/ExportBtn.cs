using SFB;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ExportBtn : MonoBehaviour
{
    public FlightHandler flightHandler;

#if UNITY_WEBGL && !UNITY_EDITOR
    DllImport("__Internal")
    private static extern void UploadFile(string objName, string methodName, string filter, bool multiple);
    
    public void OnClickOpen() {
        UploadFile(gameObject.name, "OnFileUpload", ".csv", false);
    }
    
    public void OnFileUpload(string url) {
        StartCoroutine(OutputRoutineUpload(url));
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
