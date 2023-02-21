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
    }

    void Slide() {
        Debug.Log("Volume Changed to " + VolumeSlider.value);
        AudioManager.instance.soundVolume = VolumeSlider.value;
        AudioManager.instance.playSoundEvent("KittyJump");
    }
}
