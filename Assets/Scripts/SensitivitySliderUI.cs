using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitivitySliderUI : MonoBehaviour
{
    public Slider sensitivitySlider;
    private PlayerCamera playerCam;
    public TextMeshProUGUI valueText;
    void Start()
    {
        // Get the PlayerCamera script that's on the same GameObject
        playerCam = GetComponent<PlayerCamera>();
        sensitivitySlider.value = SensitivityManager.sensX;
        OnSliderValueChanged(SensitivityManager.sensX);
        sensitivitySlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        valueText.text = ((int)value).ToString();
        if (playerCam != null)
        {
            playerCam.sensX = value;
            playerCam.sensY = value;
        }
        SensitivityManager.sensX = value;
        SensitivityManager.sensY = value;
    }
}
