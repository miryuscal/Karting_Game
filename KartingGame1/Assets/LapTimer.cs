using UnityEngine;
using TMPro; // Assuming you're using TextMeshPro for UI elements

public class LapTimer : MonoBehaviour
{
    [SerializeField] private Transform startTrigger; // Reference to the start/finish line trigger
    [SerializeField] private TextMeshProUGUI lapTimeText; // Reference to the UI text element for lap time display

    private float currentLapTime; // Stores the time for the current lap
    private bool isLapStarted; // Flag to indicate if a lap is in progress

    private void Start()
    {
        currentLapTime = 0f;
        isLapStarted = false;
    }

    private void Update()
    {
        if (isLapStarted)
        {
            currentLapTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car") // Assuming you have a "Car" tag for your player vehicle
        {
            if (isLapStarted)
            {
                // Lap completed, update UI with new lap time
                lapTimeText.text = "Lap Time: " + FormatLapTime(currentLapTime);
                currentLapTime = 0f; // Reset lap time for the next lap
            }
            else
            {
                // Lap started, start timer
                isLapStarted = true;
                currentLapTime = 0f;
            }
        }
    }

    private string FormatLapTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time % 1) * 1000f);

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}