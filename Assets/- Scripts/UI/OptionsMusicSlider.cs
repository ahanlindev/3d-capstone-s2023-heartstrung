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
        Debug.Log("Music volume changed to " + VolumeSlider.value);
        AudioManager.instance.musicVolume = VolumeSlider.value;
        AudioManager.instance.updateMusicVolume();
        // If there is no music playing,
        // indicate the volume of the music via a sound effect
        if(!AudioManager.instance.musicPlaying) {
            float temp = AudioManager.instance.soundVolume;
            AudioManager.instance.soundVolume = VolumeSlider.value;
            AudioManager.instance.playSoundEvent("KittyJump");
            AudioManager.instance.soundVolume = temp;
        }
    }
}
