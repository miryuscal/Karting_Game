using UnityEngine;

public class AICarController : MonoBehaviour
{
    public Transform path; // Waypoint'lerin parent GameObject'i
    public float maxMotorTorque = 150f; // Motor g�c�
    public float maxSteeringAngle = 30f; // Direksiyon a��s�
    public Transform centerOfMass; // A��rl�k merkezi
    public float maxSpeed = 100f; // Maksimum h�z
    public float brakeTorque = 3000f; // Fren g�c�

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
        // Rigidbody bile�enini al ve a��rl�k merkezini ayarla
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
        // �u anki waypoint'e do�ru s�r
        Drive();
        // Tekerlek g�rsellerini g�ncelle
        UpdateWheelPoses();
    }

    void Drive()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(waypoints[currentWaypointIndex].position);
        float steeringAngle = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;

        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;

        float motorTorque = maxMotorTorque;
        float currentSpeed = rb.velocity.magnitude * 3.6f; // H�z� km/h cinsinden al

        // H�z kontrol�
        if (currentSpeed > maxSpeed)
        {
            motorTorque = 0f;
            ApplyBrakes(brakeTorque);
        }
        else
        {
            ApplyBrakes(0f);
        }

        // Virajda h�z azaltma
        if (Mathf.Abs(steeringAngle) > maxSteeringAngle / 2 && currentSpeed > maxSpeed / 2)
        {
            motorTorque = maxMotorTorque / 2; // Virajda motor g�c�n� azalt
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
