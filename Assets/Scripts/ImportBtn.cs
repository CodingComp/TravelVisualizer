using SFB;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ImportBtn : MonoBehaviour
{
    
    public FlightHandler flightHandler;
    
#if UNITY_WEBGL &&  !UNITY_EDITOR
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
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "csv", false);
        if (paths.Length > 0) {
            StartCoroutine(OutputRoutineOpen(new System.Uri(paths[0]).AbsoluteUri));
        }
    }
    
#endif

    private IEnumerator OutputRoutineOpen(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            string text = www.downloadHandler.text;
            string[] flights = text.Split(',');
            flightHandler.ImportFlights(flights);
        }
    }

}
