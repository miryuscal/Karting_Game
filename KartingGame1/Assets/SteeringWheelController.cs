using UnityEngine;

public class SteeringWheelController : MonoBehaviour
{
    // Referanslar
    [SerializeField] private RectTransform steeringWheelRectTransform;
    [SerializeField] private CarController carController;

    // D�nme katsay�s�
    [SerializeField] private float rotationSpeed = 200f;

    // Ekrandaki direksiyonun d�nme aral���
    private float maxRotationAngle;

    // Ekrandaki direksiyonun orijinal rotasyonu
    private Quaternion originalRotation;

    // Ba�lang��
    private void Start()
    {
        // Ekrandaki direksiyonun d�nme aral���n� belirle
        maxRotationAngle = carController.maxSteerAngle;

        // Ekrandaki direksiyonun orijinal rotasyonunu kaydet
        originalRotation = steeringWheelRectTransform.rotation;
    }

    // Her frame'de �al���r
    private void Update()
    {
        // Kullan�c�n�n fare veya dokunmatik giri�ini al
        float input = Input.GetAxis("Horizontal");

        // Ekrandaki direksiyonu d�nd�r
        RotateSteeringWheel(input);
    }

    // Ekrandaki direksiyonu d�nd�rme i�levi
    private void RotateSteeringWheel(float input)
    {
        // D�nd�rme miktar�, giri�e ve d�nme katsay�s�na ba�l� olarak hesaplan�r
        float rotationAmount = input * rotationSpeed * Time.deltaTime;

        // Ekrandaki direksiyonun yeni rotasyonunu hesapla
        Quaternion newRotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(steeringWheelRectTransform.localEulerAngles.z + rotationAmount, -maxRotationAngle, maxRotationAngle));

        // Ekrandaki direksiyonu d�nd�r
        steeringWheelRectTransform.rotation = originalRotation * newRotation;
    }
}
