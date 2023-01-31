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

    private string path = "Assets/Resources/Audio/SFX/";

    private Dictionary<string, AudioEvent> sounds = new Dictionary<string, AudioEvent>();

    private AudioSource[] audioSources = new AudioSource[5];

    void Awake() {
        // Singleton logic
        if(instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
        }

        // Create and instantiate new AudioEvents for each event specified
        // by the file structure of the SFX folder.
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*");
        foreach(FileInfo fileInfo in info) {
            string fileName = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf("."));
            string pathToThisFile = path + fileName + "/";
            if(Directory.Exists(pathToThisFile)) {
                // create an AudioEvent from this folder
                // Debug.Log("path " + pathToThisFile + " exists!");
                AudioEvent newEvent = new AudioEvent();
                // Make the key in the AudioEvent dictionary the folder name minus the .meta extension
                string key = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf("."));
                newEvent.addSoundsFromPath(pathToThisFile);
                sounds.Add(key, newEvent);
            } else {
                // Debug.Log("didn't find directory " + pathToThisFile);
            }
        }

        // Instantiate the AudioSources
        for(int i = 0; i < numAudioSources; i++) {
            audioSources[0] = gameObject.AddComponent<AudioSource>();
            audioSources[0].playOnAwake = false;
        }

        // // DEBUG: Print sounds dict to console
        // foreach(KeyValuePair<string, AudioEvent> sound in sounds) {
        //     Debug.Log("Key = " + sound.Key + ", Value = " + sound.Value);
        // }

        // Manually override the default pitch shift variable for the following sound event(s)
        // Maybe find a more elegant solution for this later?
        sounds["Defeat"].setPitchShift(false);

        DontDestroyOnLoad(this.transform.parent.gameObject);
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
                if(sounds[sound].doPitchShift()) {
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
