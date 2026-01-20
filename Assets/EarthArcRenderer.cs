using System;
using System.Collections.Generic;
using UnityEngine;

public class EarthArcRenderer : MonoBehaviour
{
    public TravelHandler travelHandler;

    [Header("Arc Settings")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float earthRadius = 1.98f; // Earth radius in your scene units
    [SerializeField] private int arcSegments = 50;
    [SerializeField] private float arcHeight = 0.2f; // Height above surface (as fraction of radius)

    [Header("Visual Settings")]
    [SerializeField] private Material arcMaterial;
    [SerializeField] private float lineWidth = 0.01f;
    [SerializeField] private Color arcColor = Color.cyan;

    private void Update()
    {
        foreach (TravelData data in travelHandler.TravelData) {
            if (!data.LineReaderSetup) SetupLineRenderer(data);
            DrawArc(data);
        }
    }

    void SetupLineRenderer(TravelData data)
    {
        data.LineRenderer.material = arcMaterial;
        data.LineRenderer.startColor = arcColor;
        data.LineRenderer.endColor = arcColor;
        data.LineRenderer.startWidth = lineWidth;
        data.LineRenderer.endWidth = lineWidth;
        data.LineRenderer.positionCount = arcSegments;
        data.LineRenderer.useWorldSpace = true;
        data.LineReaderSetup = true;
    }

    void DrawArc(TravelData data)
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
    }
}