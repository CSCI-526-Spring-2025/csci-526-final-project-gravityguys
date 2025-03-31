using UnityEngine;
using UnityEngine.UI;
using TMPro; // or UnityEngine.UI if you're using legacy UI Text

public class RevealLoadingBar : MonoBehaviour
{
    public Image dashBarFill;
    public Image dashBarBG;
    public TextMeshProUGUI dashText;

    public Color loadingColor = Color.red;
    public Color readyColor = Color.cyan;

    public void ShowDashLoadingBar()
    {
        gameObject.SetActive(true);
        dashBarFill.fillAmount = 0f;
        dashBarFill.color = loadingColor;
        dashText.text = "Loading";
        dashText.color = Color.white;
    }

    public void ShowDashReadyBar()
    {
        gameObject.SetActive(true);
        dashBarFill.fillAmount = 1f;
        dashBarFill.color = readyColor;
        dashText.text = "Ready";
        dashText.color = Color.yellow;

        // Optional: add pulsing glow animation, sound, etc.
    }
}