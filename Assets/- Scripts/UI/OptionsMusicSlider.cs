using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMusicSlider : MonoBehaviour
{
    public UnityEngine.UI.Slider VolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        VolumeSlider.onValueChanged.AddListener(delegate {Slide();});
    }

    void Slide() {
        if (!AudioManager.instance) { return; }
        AudioManager.instance.musicVolume = VolumeSlider.value;
        AudioManager.instance.UpdateMusicVolume();
        // If there is no music playing,
        // indicate the volume of the music via a sound effect
        if(!AudioManager.instance._musicPlaying) {
            float temp = AudioManager.instance.soundVolume;
            AudioManager.instance.soundVolume = VolumeSlider.value;
            AudioManager.instance.PlaySoundEvent("KittyJump");
            AudioManager.instance.soundVolume = temp;
        }
    }
}
