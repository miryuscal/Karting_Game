using UnityEngine;
using UnityEngine.EventSystems;

public class MobileController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CarController carController; // Ana CarController nesnesine eriþmek için referans

    private bool isGasPressed = false;
    private bool isBrakePressed = false;

    private void Update()
    {
        if (isGasPressed)
        {
            carController.SetInput(1f, false); // Gaz butonuna basýlýyken arabayý ileriye doðru hareket ettir
        }
        else if (isBrakePressed)
        {
            carController.SetInput(0f, true); // Fren butonuna basýlýyken arabayý durdur
        }
        else
        {
            carController.SetInput(0f, false); // Her iki butona da basýlmýyorsa giriþi sýfýrla
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerEnter.name == "GasButton")
        {
            GasButtonDown();
        }
        else if (eventData.pointerEnter.name == "BrakeButton")
        {
            BrakeButtonDown();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerEnter.name == "GasButton")
        {
            GasButtonUp();
        }
        else if (eventData.pointerEnter.name == "BrakeButton")
        {
            BrakeButtonUp();
        }
    }

    public void GasButtonDown()
    {
        isGasPressed = true;
    }

    public void BrakeButtonDown()
    {
        isBrakePressed = true;
    }

    public void GasButtonUp()
    {
        isGasPressed = false;
    }

    public void BrakeButtonUp()
    {
        isBrakePressed = false;
    }
}
