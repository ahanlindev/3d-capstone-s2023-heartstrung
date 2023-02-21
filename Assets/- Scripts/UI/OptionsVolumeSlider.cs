using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsVolumeSlider : MonoBehaviour
{
    public UnityEngine.UI.Slider VolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        VolumeSlider.onValueChanged.AddListener(delegate {Slide();});
        VolumeSlider.value = AudioManager.instance.volume;
    }

    void Slide() {
        AudioManager.instance.volume = VolumeSlider.value;
        AudioManager.instance.playSoundEvent("KittyJump");
    }
}
