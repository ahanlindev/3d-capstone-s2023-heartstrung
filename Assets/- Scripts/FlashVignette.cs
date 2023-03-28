using UnityEngine;
using UnityEngine.UI;

public class DamageVignette : MonoBehaviour
{
    public float flashDuration = 0.5f;
    public float maxAlpha = 0.5f;

    private Image vignetteImage;
    private float flashTimer = 0f;
    private bool isFlashing = false;

    private void Start()
    {
        vignetteImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (isFlashing)
        {
            flashTimer += Time.deltaTime;
            if (flashTimer >= flashDuration)
            {
                isFlashing = false;
                flashTimer = 0f;
                vignetteImage.color = new Color(1f, 1f, 1f, 0f);
            }
            else
            {
                float flashAlpha = maxAlpha * (1 - flashTimer / flashDuration);
                vignetteImage.color = new Color(1f, 1f, 1f, flashAlpha);
            }
        }
    }

    public void FlashVignette()
    {
        isFlashing = true;
    }
}