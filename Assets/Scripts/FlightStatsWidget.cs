using TMPro;
using UnityEngine;

public class FlightStatsWidget : MonoBehaviour
{
    [Header("References")]
    public Canvas canvas;
    public GameObject widgetUi;
    public RectTransform widgetRect;
    public EarthMovement earthMovement;
    public Camera mainCamera;

    [Header("Settings")]
    public float widgetOffset = 150.0f;

    [Header("Ui Elements")]
    [SerializeField] private TMP_Text flightNumber;
    [SerializeField] private TMP_Text airlineText;
    [SerializeField] private TMP_Text originText;
    [SerializeField] private TMP_Text originCountryText;
    [SerializeField] private TMP_Text destinationText;
    [SerializeField] private TMP_Text destinationCountryText;

    private bool _flipWidget;
    private Vector2 _offset;

    private FlightData _displayedFlightData;
    private GameObject _hoveredGo;

    private const int FlightLayer = 6;
    private int _flightLayerMask;

    private void Awake()
    {
        _offset = new Vector2(0.0f, widgetOffset);
        widgetUi.SetActive(false);

        // Bitwise left shift to represent layer number by a single bit 
        _flightLayerMask = 1 << FlightLayer;

        Physics.queriesHitBackfaces = true;
    }

    private void Update()
    {
        MoveWidget();

        // If user is dragging world view dont display widget
        if (Cursor.lockState == CursorLockMode.Locked) {
            if (_hoveredGo) HideWidget();
            return;
        }

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _flightLayerMask)) { // Hit marker
            if (_hoveredGo && !_hoveredGo == hit.transform.gameObject) return; // If marker is initialized and is equal to hit obj return

            _hoveredGo = hit.transform.gameObject;
            _displayedFlightData = _hoveredGo.GetComponent<CoordinateMarker>().FlightData;
            ShowWidget();
        }
        else if (_hoveredGo) {
            HideWidget();
        }
    }

    private void MoveWidget()
    {
        _flipWidget = Input.mousePosition.y - (widgetOffset + 120.0f) < 0.0f;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);

        Vector2 widgetPos = _flipWidget ? pos + _offset : pos - _offset;
        widgetRect.position = canvas.transform.TransformPoint(widgetPos);
    }

    public void ShowWidget()
    {
        flightNumber.text = _displayedFlightData.Callsign;
        airlineText.text = _displayedFlightData.Airline;

        originText.text = _displayedFlightData.Origin.IataCode;
        originCountryText.text = _displayedFlightData.Origin.Country;

        destinationText.text = _displayedFlightData.Destination.IataCode;
        destinationCountryText.text = _displayedFlightData.Destination.Country;

        widgetUi.gameObject.SetActive(true);
        earthMovement.shouldRotate = false;
    }

    public void HideWidget()
    {
        widgetUi.gameObject.SetActive(false);
        earthMovement.shouldRotate = true;
        _hoveredGo = null;
    }
}