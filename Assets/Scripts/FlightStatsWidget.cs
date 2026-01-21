using TMPro;
using UnityEngine;

public class FlightStatsWidget : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform canvasRectTransform;
    public GameObject widgetUi;
    public RectTransform widgetRect;

    public float widgetOffset = 150.0f;
    public bool flipWidget = false;
    private Vector2 _offset;
    
    public Flightroute displayedFlightData;

    public Camera c;

    [SerializeField] private TMP_Text flightNumber;
    [SerializeField] private TMP_Text airlineText;
    [SerializeField] private TMP_Text originText;
    [SerializeField] private TMP_Text originCountryText;
    [SerializeField] private TMP_Text destinationText;
    [SerializeField] private TMP_Text destinationCountryText;
    
    private void Awake()
    {
        _offset = new Vector2(0.0f, widgetOffset);
        widgetUi.SetActive(false);
    }

    private void Update()
    {
        MoveWidget();
        
        RaycastHit hit;
        Ray ray = c.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
            Debug.Log("found " + hit.transform.name);
    }

    private void MoveWidget()
    {
        flipWidget = Input.mousePosition.y - (widgetOffset + 120.0f) < 0.0f;
        
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        
        Vector2 widgetPos = flipWidget ? pos + _offset : pos - _offset;
        widgetRect.position = canvas.transform.TransformPoint(widgetPos);
    }

    public void ShowWidget(Flightroute data)
    {
        displayedFlightData = data;

        flightNumber.text = displayedFlightData.callsign;
        airlineText.text = displayedFlightData.airline.name;
        
        originText.text = displayedFlightData.origin.iata_code;
        originCountryText.text = displayedFlightData.origin.country_name;
        
        destinationText.text = displayedFlightData.destination.iata_code;
        destinationCountryText.text = displayedFlightData.destination.country_name;
        
        widgetUi.gameObject.SetActive(true);
    }

    public void HideWidget()
    {
        widgetUi.gameObject.SetActive(false);
    }
}
