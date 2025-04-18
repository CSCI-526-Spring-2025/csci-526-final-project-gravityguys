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
        sensitivitySlider.onValueChanged.AddListener(OnSliderValueChanged);

        // Set initial values when scene starts
        OnSliderValueChanged(sensitivitySlider.value);
    }

    void OnSliderValueChanged(float value)
    {
        valueText.text = ((int)value).ToString();
        if (playerCam != null)
        {
            playerCam.sensX = value;
            playerCam.sensY = value;
        }
    }
}
