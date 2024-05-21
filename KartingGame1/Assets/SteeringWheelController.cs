using UnityEngine;

public class SteeringWheelController : MonoBehaviour
{
    // Referanslar
    [SerializeField] private RectTransform steeringWheelRectTransform;
    [SerializeField] private CarController carController;

    // Dönme katsayýsý
    [SerializeField] private float rotationSpeed = 200f;

    // Ekrandaki direksiyonun dönme aralýðý
    private float maxRotationAngle;

    // Ekrandaki direksiyonun orijinal rotasyonu
    private Quaternion originalRotation;

    // Baþlangýç
    private void Start()
    {
        // Ekrandaki direksiyonun dönme aralýðýný belirle
        maxRotationAngle = carController.maxSteerAngle;

        // Ekrandaki direksiyonun orijinal rotasyonunu kaydet
        originalRotation = steeringWheelRectTransform.rotation;
    }

    // Her frame'de çalýþýr
    private void Update()
    {
        // Kullanýcýnýn fare veya dokunmatik giriþini al
        float input = Input.GetAxis("Horizontal");

        // Ekrandaki direksiyonu döndür
        RotateSteeringWheel(input);
    }

    // Ekrandaki direksiyonu döndürme iþlevi
    private void RotateSteeringWheel(float input)
    {
        // Döndürme miktarý, giriþe ve dönme katsayýsýna baðlý olarak hesaplanýr
        float rotationAmount = input * rotationSpeed * Time.deltaTime;

        // Ekrandaki direksiyonun yeni rotasyonunu hesapla
        Quaternion newRotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(steeringWheelRectTransform.localEulerAngles.z + rotationAmount, -maxRotationAngle, maxRotationAngle));

        // Ekrandaki direksiyonu döndür
        steeringWheelRectTransform.rotation = originalRotation * newRotation;
    }
}
