using System.Collections;
using UnityEngine;

public class CoordinateMarker : MonoBehaviour
{
    public FlightData FlightData;
    public GameObject originMarker;
    public GameObject destinationMarker;
    public float duration = 1.0f;

    private LineRenderer _lineRenderer;
    private float _startValue, _currentValue = 0.0f;
    private const float TargetAlpha = 1.0f;
    
    private Material _originMat;
    private Material _destinationMat;
    
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _originMat = originMarker.GetComponent<Renderer>().material;
        _destinationMat = destinationMarker.GetComponent<Renderer>().material;
        StartCoroutine(LerpValue());
    }

    // Fades in the coordinate marker elements
    IEnumerator LerpValue()
    {
        float elapsedTime = 0f;
        Color color;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            _currentValue = Mathf.Lerp(_startValue, TargetAlpha, t);

            color = _lineRenderer.material.color;
            color.a = _currentValue;
            
            _lineRenderer.material.color = color;
            _originMat.color = color;
            _destinationMat.color = color;
            
            yield return null;
        }
        
        color = _lineRenderer.material.color;
        color.a = TargetAlpha;
        _lineRenderer.material.color = color;
        _originMat.color = color;
        _destinationMat.color = color;
    }
}