using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

// Attach this script to the LevelManager.
// 
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set;}

    [Tooltip("The number of AudioSources to instantiate. Roughly corresponds to the number of sound effects that can be played at once.")]
    [SerializeField] public int numAudioSources = 5;

    private Dictionary<string, AudioEvent> sounds = new Dictionary<string, AudioEvent>();

    private AudioSource[] audioSources = new AudioSource[5];
    
    private string resourcesPath = "Audio/SFX";

    void Awake() {
        // Singleton logic
        if(instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
        }

        // Instantiate the AudioSources
        for(int i = 0; i < numAudioSources; i++) {
            audioSources[0] = gameObject.AddComponent<AudioSource>();
            audioSources[0].playOnAwake = false;
        }

        // Don't destroy this between scenes
        DontDestroyOnLoad(this.transform.parent.gameObject);

        Debug.Log("Done loading sounds.");
    }

    void Start() {
        // Get the AudioEvents pre-defined as children of this gameObject
        AudioEvent[] audioEvents = GetComponentsInChildren<AudioEvent>();

        // To make search faster and easier, make a dictionary
        foreach(AudioEvent audioEvent in audioEvents) {
            sounds.Add(audioEvent.EventName, audioEvent);
        }

        // DEBUG: Print sounds dict to console
        foreach(KeyValuePair<string, AudioEvent> sound in sounds) {
            Debug.Log("Key = " + sound.Key + ", Value = " + sound.Value);
        }
    }

    /// <summary>Attempts to play a sound from the specified AudioEvent.</summary>
    /// <param name="sound">The name of the AudioPool to pool from.</param>
    public bool playSoundEvent(string sound) {
        AudioClip soundToPlay = sounds[sound].poolSound();
        if(soundToPlay == null) {
            Debug.LogError(sound + " is not a sound event!");
            return false;
        }
        // find an available AudioSource to play the sound
        foreach(AudioSource source in gameObject.GetComponents<AudioSource>()) {
            if(!source) {
                Debug.LogError("source is null bruh");
            }
            if(!source.isPlaying) {
                source.clip = soundToPlay;
                // randomize pitch for  v a r i a t i o n (TM)
                if(sounds[sound].PitchShift) {
                    source.pitch = Random.Range(.75f, 1.25f);
                } else {
                    source.pitch = 1f;
                    Debug.Log("not doing pitch shift for " + sound);
                }
                source.Play();
                return true;
            }
        }
        // all audiosources are taken; do not play the sound effect
        return false;
    }
}
