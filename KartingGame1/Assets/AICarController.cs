using UnityEngine;

public class AICarController : MonoBehaviour
{
    public Transform path; // Waypoint'lerin parent GameObject'i
    public float maxMotorTorque = 150f; // Motor gücü
    public float maxSteeringAngle = 30f; // Direksiyon açýsý
    public Transform centerOfMass; // Aðýrlýk merkezi
    public float maxSpeed = 100f; // Maksimum hýz
    public float brakeTorque = 3000f; // Fren gücü

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private Rigidbody rb;

    void Start()
    {
        // Rigidbody bileþenini al ve aðýrlýk merkezini ayarla
        rb = GetComponent<Rigidbody>();
        if (centerOfMass != null)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }

        // Waypoint'leri al
        waypoints = new Transform[path.childCount];
        for (int i = 0; i < path.childCount; i++)
        {
            waypoints[i] = path.GetChild(i);
        }
    }

    void FixedUpdate()
    {
        // Þu anki waypoint'e doðru sür
        Drive();
        // Tekerlek görsellerini güncelle
        UpdateWheelPoses();
    }

    void Drive()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(waypoints[currentWaypointIndex].position);
        float steeringAngle = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;

        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;

        float motorTorque = maxMotorTorque;
        float currentSpeed = rb.velocity.magnitude * 3.6f; // Hýzý km/h cinsinden al

        // Hýz kontrolü
        if (currentSpeed > maxSpeed)
        {
            motorTorque = 0f;
            ApplyBrakes(brakeTorque);
        }
        else
        {
            ApplyBrakes(0f);
        }

        // Virajda hýz azaltma
        if (Mathf.Abs(steeringAngle) > maxSteeringAngle / 2 && currentSpeed > maxSpeed / 2)
        {
            motorTorque = maxMotorTorque / 2; // Virajda motor gücünü azalt
            ApplyBrakes(brakeTorque / 2); // Hafif fren uygula
        }

        frontLeftWheelCollider.motorTorque = motorTorque;
        frontRightWheelCollider.motorTorque = motorTorque;

        if (relativeVector.magnitude < 10f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    void ApplyBrakes(float brakeTorque)
    {
        frontLeftWheelCollider.brakeTorque = brakeTorque;
        frontRightWheelCollider.brakeTorque = brakeTorque;
        rearLeftWheelCollider.brakeTorque = brakeTorque;
        rearRightWheelCollider.brakeTorque = brakeTorque;
    }

    void UpdateWheelPoses()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;

        // Apply rotation fix
        if (wheelTransform == frontLeftWheelTransform || wheelTransform == rearLeftWheelTransform)
            rot *= Quaternion.Euler(0, 0, -90);
        else if (wheelTransform == frontRightWheelTransform || wheelTransform == rearRightWheelTransform)
            rot *= Quaternion.Euler(0, -180, -90);

        wheelTransform.rotation = rot;
    }
}
