using System;
using UnityEngine;

public class EarthArcRenderer : MonoBehaviour
{
    public FlightHandler flightHandler;
    public Camera mainCamera;

    [Header("Arc Settings")]
    [SerializeField] private float earthRadius = 1.98f;
    [SerializeField] private int arcSegments = 50;
    [SerializeField] private float arcHeight = 0.04f;
    [SerializeField] private float lineWidth = 0.025f;

    private void Update()
    {
        for (int i = 0; i < flightHandler.FlightData.Count; i++) {
            FlightData data = flightHandler.FlightData[i];

            if (!data.IsLineReaderSetup)
                data.SetupLineRenderer(lineWidth, arcSegments, mainCamera);

            DrawArc(data);
        }
    }

    void DrawArc(FlightData data)
    {
        Vector3 start = data.Origin.CoordinateVisual.transform.position;
        Vector3 end = data.Destination.CoordinateVisual.transform.position;
        Vector3 earthCenter = transform.position;

        // Normalize points to sphere surface
        Vector3 startDir = (start - earthCenter).normalized;
        Vector3 endDir = (end - earthCenter).normalized;

        float angle = Vector3.Angle(startDir, endDir);
        Vector3 rotationAxis = Vector3.Cross(startDir, endDir).normalized;

        // Handle edge case where points are opposite (180 degrees)
        if (rotationAxis == Vector3.zero) {
            rotationAxis = Vector3.Cross(startDir, Vector3.up);
            if (rotationAxis == Vector3.zero) {
                rotationAxis = Vector3.Cross(startDir, Vector3.right);
            }
            rotationAxis.Normalize();
        }

        // Generate arc points
        for (int i = 0; i < arcSegments; i++) {
            float t = i / (float)(arcSegments - 1);

            float currentAngle = angle * t;

            // Rotate start direction around axis
            Quaternion rotation = Quaternion.AngleAxis(currentAngle, rotationAxis);
            Vector3 direction = rotation * startDir;

            Transform[] visualTransforms = data.GetCoordinateVisuals();
            float heightMult = MathF.Max(Vector3.Distance(visualTransforms[0].position, visualTransforms[1].position), 0.05f);

            float heightMultiplier = 1.0f + arcHeight * heightMult * Mathf.Sin(t * Mathf.PI);
            Vector3 position = earthCenter + direction * (earthRadius * heightMultiplier);

            data.LineRenderer.SetPosition(i, position);
        }

        if (!data.IsLineRendererMeshSetup) data.GenerateMesh();
    }
}