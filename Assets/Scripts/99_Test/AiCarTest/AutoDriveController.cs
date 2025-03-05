using System.Collections.Generic;
using UnityEngine;

public class AutoDriveController : MonoBehaviour
{
    public WaypointGizmoDrawer gizmoDrawer; // WaypointGizmoDrawer ÇéQè∆
    public float maxMotorTorque = 150f;
    public float maxSteeringAngle = 30f;
    public float waypointThreshold = 2f;
    public float minSpeed = 5f;
    public float maxSpeed = 20f;
    public float brakeTorque = 300f;
    public float curveSensitivity = 100f;
    public float brakingDistanceFactor = 2.0f;

    public WheelCollider[] wheelColliders;
    public Transform[] wheelModels;

    private List<Vector3> bezierPoints;
    private int currentBezierIndex = 0;
    private Rigidbody rb;
    private float currentSpeed = 0f;

    [SerializeField]
    private RaceData raceData;

    private AIAutoReverse autoReverse;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (gizmoDrawer != null)
        {
            bezierPoints = new List<Vector3>(gizmoDrawer.bezierPoints);
        }
        else
        {
            Debug.LogError("WaypointGizmoDrawer is not assigned!");
        }
    }

    void FixedUpdate()
    {
        if (raceData.isPlay)
        {
            if (bezierPoints == null || bezierPoints.Count < 2) return;

            Vector3 targetPoint = bezierPoints[currentBezierIndex];
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0;

            float distanceToTarget = Vector3.Distance(transform.position, targetPoint);

            if (distanceToTarget < waypointThreshold)
            {
                currentBezierIndex = (currentBezierIndex + 1) % bezierPoints.Count;
            }

            float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
            float steering = Mathf.Clamp(angle / maxSteeringAngle, -1f, 1f);

            wheelColliders[0].steerAngle = steering * maxSteeringAngle;
            wheelColliders[1].steerAngle = steering * maxSteeringAngle;

            float targetSpeed = maxSpeed;
            AdjustSpeed(targetSpeed);
            UpdateWheelModels();
        }
    }

    private void AdjustSpeed(float targetSpeed)
    {
        currentSpeed = rb.velocity.magnitude;

        if (currentSpeed > targetSpeed)
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.motorTorque = 0;
                wheel.brakeTorque = brakeTorque;
            }
        }
        else
        {
            foreach (WheelCollider wheel in wheelColliders)
            {
                wheel.brakeTorque = 0;
                wheel.motorTorque = maxMotorTorque;
            }
        }
    }

    private void UpdateWheelModels()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            if (wheelModels[i] != null)
            {
                wheelColliders[i].GetWorldPose(out Vector3 position, out Quaternion rotation);
                wheelModels[i].position = position;
                wheelModels[i].rotation = rotation;
            }
        }
    }
}
