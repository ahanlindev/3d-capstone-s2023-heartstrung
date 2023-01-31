using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// A class that stores a pool of AudioClips to play from when activated.
public class AudioEvent
{
    private List<AudioClip> soundPool = new List<AudioClip>();

    [Tooltip("Should randomized pitch shifting occur on this sound event?")]
    [SerializeField] private bool pitchShift = true;

    public bool doPitchShift() {
        return pitchShift;
    }

    public void setPitchShift(bool var) {
        pitchShift = var;
    }

    /// <summary>Add all AudioClip files from the specified file path to this AudioEvent pool.</summary>
    /// <param name="path">The path from the root of the project to the sound effect folder.</param>
    public void addSoundsFromPath(string path) {
        // Debug.Log("adding sounds from path " + path);
        // Grab each existing sound effect from path
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach(FileInfo fileInfo in info) {
            if(fileInfo.Extension == ".ogg" || fileInfo.Extension == ".mp3" || fileInfo.Extension == ".wav") {
                string fileName = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf("."));
                string resourcesPath = path.Substring(path.IndexOf("Resources") + "Resources".Length + 1);
                // Debug.Log("Found Sound Effect " + fileName);
                // convert the file into an AudioClip for the AudioSource

                AudioClip sound = Resources.Load<AudioClip>(resourcesPath + fileName);
                if(sound != null) {
                    // Debug.Log("Sound Effect Loaded!");
                } else {
                    // Debug.LogError("could not load sound effect " + resourcesPath + fileName);
                }

                soundPool.Add(sound);
            }
        }
    }

    // Add the specified AudioClip to this AudioEvent pool.
    public void addSound(AudioClip clip) {
        soundPool.Add(clip);
    }

    // Returns a randomly selected AudioClip from soundPool.
    public AudioClip poolSound() {
        return soundPool[Random.Range(0, soundPool.Count)];
    }
}
