using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class DashTint : MonoBehaviour
{
    public Image tintImage;
    public float tintDuration = 2f;

    private void Start()
    {
        tintImage.enabled = false;
    }

    public void TriggerTint()
    {
        StopAllCoroutines();
        StartCoroutine(FlashTint());
    }

    IEnumerator FlashTint()
    {
        tintImage.enabled = true;
        yield return new WaitForSeconds(tintDuration);
        tintImage.enabled = false;
    }
}