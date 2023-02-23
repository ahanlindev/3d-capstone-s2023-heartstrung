using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSoundSlider : MonoBehaviour
{
    public UnityEngine.UI.Slider VolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        VolumeSlider.onValueChanged.AddListener(delegate {Slide();});
        VolumeSlider.value = AudioManager.instance?.soundVolume ?? 0;
    }

    void Slide() {
        if (!AudioManager.instance) { return; }
        Debug.Log("Volume Changed to " + VolumeSlider.value);
        AudioManager.instance.soundVolume = VolumeSlider.value;
        AudioManager.instance.playSoundEvent("KittyJump");
    }
}
