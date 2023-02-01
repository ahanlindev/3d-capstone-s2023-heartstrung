using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

/// <summary>A class that stores a pool of AudioClips to play from when activated.<\summary>
public class AudioEvent : MonoBehaviour
{
    [Tooltip("The name of this sound event. Used to reference in scripts.")]
    public string EventName {get; private set;}

    [Tooltip("The list of sound effects that this AudioEvent will choose from.")]
    [SerializeField] public List<AudioClip> SoundPool = new List<AudioClip>();

    [Tooltip("Should randomized pitch shifting occur on this sound event?")]
    [SerializeField] public bool PitchShift = true;

    void Awake() {
        Debug.Log("setting name to " + this.gameObject.name);
        EventName = this.gameObject.name;
        Debug.Log("name has been set to " + EventName);
    }

    /// <summary>Returns a randomly selected AudioClip from SoundPool.</summary>
    public AudioClip poolSound() {
        return SoundPool[Random.Range(0, SoundPool.Count)];
    }
}
