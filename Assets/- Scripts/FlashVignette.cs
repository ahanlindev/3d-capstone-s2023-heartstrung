using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DamageVignette : MonoBehaviour
{
    public float flashDuration = 0.5f;
    public float maxIntensity = 0.5f;

    private PostProcessVolume postProcessVolume;
    private Vignette vignette;
    private float flashTimer = 0f;
    private bool isFlashing = false;

    private void Start()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out vignette);
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
                vignette.intensity.value = 0f;
            }
            else
            {
                float flashIntensity = maxIntensity * (1 - flashTimer / flashDuration);
                vignette.intensity.value = flashIntensity;
            }
        }
    }

    public void FlashVignette()
    {
        isFlashing = true;
    }
}