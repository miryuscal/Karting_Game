using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;
    private bool isBreaking;

    [SerializeField] private GameObject steeringWheel;

    // UI Elements
    [SerializeField] private RectTransform gasPedal;
    [SerializeField] private RectTransform brakePedal;
    [SerializeField] private Image gasPedalImage;
    [SerializeField] private Image brakePedalImage;

    // Settings
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float breakForce = 3000f;
    [SerializeField] public float maxSteerAngle = 30f;
    [SerializeField] private float divider = 10f;
    [SerializeField] private float steeringWheelMultiplier = 90f;
    [SerializeField] private Vector3 centerOfMassOffset = new Vector3(0, -0.5f, 0); // Adjusted for stability

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private Vector3 gasPedalDefaultScale;
    private Vector3 brakePedalDefaultScale;

    private void Start()
    {
        // Adjust the center of mass for better stability
        GetComponent<Rigidbody>().centerOfMass += centerOfMassOffset;

        // Gaz ve fren pedalý scale deðerlerini kaydediyoruz
        gasPedalDefaultScale = gasPedal.localScale;
        brakePedalDefaultScale = brakePedal.localScale;
    }

    private void FixedUpdate()
    {
        UpdateSteeringWheel();
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        // Bu kýsým mobil giriþle güncellendiðinde verticalInput ve isBreaking MobileController tarafýndan ayarlanacak
    }

    private void HandleMotor()
    {
        float torque = verticalInput * motorForce;
        frontLeftWheelCollider.motorTorque = torque;
        frontRightWheelCollider.motorTorque = torque;

        // Adjust braking force
        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        if (isBreaking)
        {
            // Apply brake torque evenly to all wheels for stability
            frontRightWheelCollider.brakeTorque = currentBreakForce / divider;
            frontLeftWheelCollider.brakeTorque = currentBreakForce / divider;
            rearLeftWheelCollider.brakeTorque = currentBreakForce;
            rearRightWheelCollider.brakeTorque = currentBreakForce;
        }
        else
        {
            // Reset brake torque when not braking
            frontRightWheelCollider.brakeTorque = 0f;
            frontLeftWheelCollider.brakeTorque = 0f;
            rearLeftWheelCollider.brakeTorque = 0f;
            rearRightWheelCollider.brakeTorque = 0f;
        }
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
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

    private void UpdateSteeringWheel()
    {
        // Smoothly rotate the steering wheel based on target steer angle
        steeringWheel.transform.localRotation = Quaternion.Lerp(steeringWheel.transform.localRotation, Quaternion.Euler(0, 0, -horizontalInput * steeringWheelMultiplier), Time.deltaTime * 5);
    }

    public void SetInput(float vertical, bool brake)
    {
        verticalInput = vertical;
        isBreaking = brake;
    }
}
