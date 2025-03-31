using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RevealLoadingBar : MonoBehaviour
{
    public Image dashBarFill;
    public Image dashBarBG;
    public TextMeshProUGUI dashText;

    public Color loadingColor = Color.red;
    public Color readyColor = new Color(20, 144, 0, 255); //dark-green;

    public float glowPulseSpeed = 2f;
    public float glowPulseStrength = 0.2f;

    private Coroutine pulseCoroutine;

    public void ShowDashLoadingBar()
    {
        dashBarBG.enabled = true;
        dashBarFill.fillAmount = 0f;
        dashBarFill.color = loadingColor;
        dashText.text = "Loading";
        dashText.color = Color.white;

        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
            dashBarFill.transform.localScale = Vector3.one; // reset scale
        }
    }

    public void ShowDashReadyBar()
    {
        dashBarFill.fillAmount = 1f;
        dashBarFill.color = readyColor;
        dashText.text = "Ready";
        dashText.color = Color.yellow;
        dashBarBG.enabled = false;

        if (pulseCoroutine == null)
        {
            pulseCoroutine = StartCoroutine(PulseGlow());
        }
    }

    private IEnumerator PulseGlow()
    {
        float time = 0f;
        while (true)
        {
            float scale = 1f + Mathf.Sin(time * glowPulseSpeed) * glowPulseStrength;
            dashBarFill.transform.localScale = new Vector3(scale, scale, 1f);
            time += Time.deltaTime;
            yield return null;
        }
    }
}