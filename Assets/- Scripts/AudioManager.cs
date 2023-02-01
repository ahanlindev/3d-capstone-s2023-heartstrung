using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Attach this script to the LevelManager.
// 
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set;}

    [Tooltip("The number of AudioSources to instantiate. Roughly corresponds to the number of sound effects that can be played at once.")]
    [SerializeField] public int numAudioSources = 5;

    private string path = "Assets/Resources/SFX/";
    private string resourcesPath = "SFX/";

    private Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

    private AudioSource[] audioSources = new AudioSource[5];

    void Awake() {
        // Singleton logic
        if(instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
        }

        // Grab each existing sound effect from path
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach(FileInfo fileInfo in info) {
            if(fileInfo.Extension == ".ogg") {
                // Make the key in the AudioSources dictionary the file name minus the extension
                string key = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf("."));
                Debug.Log("Found Sound Effect " + key);
                // convert the file into an AudioClip for the AudioSource

                AudioClip sound = Resources.Load<AudioClip>(resourcesPath + key);
                if(sound != null) {
                    Debug.Log("Sound Effect Loaded!");
                }
                sounds.Add(key, sound);
            }
        }

        // Instantiate the AudioSources
        for(int i = 0; i < numAudioSources; i++) {
            audioSources[0] = gameObject.AddComponent<AudioSource>();
            audioSources[0].playOnAwake = false;
        }

        // DEBUG: Print sounds dict to console
        foreach(KeyValuePair<string, AudioClip> sound in sounds) {
            Debug.Log("Key = " + sound.Key + ", Value = " + sound.Value);
        }
    }

    /// <summary>Attempts to play the sound effect identified by the name passed in.</summary>
    /// <param name="sound">The name of the sound effect to play.</param>
    public bool playSound(string sound) {
        AudioClip soundToPlay = sounds[sound];
        if(soundToPlay == null) {
            Debug.LogError(sound + " is not a sound effect!");
            return false;
        }
        // find an available AudioSource to play the sound
        foreach(AudioSource source in gameObject.GetComponents<AudioSource>()) {
            if(!source) {
                Debug.LogError("source is null bruh");
            }
            if(!source.isPlaying) {
                source.clip = soundToPlay;
                source.Play();
                return true;
            }
        }
        // all audiosources are taken; do not play the sound effect
        return false;
    }
}
