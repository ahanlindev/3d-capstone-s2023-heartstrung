using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

// Attach this script to the LevelManager.
// 
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set;}

    [Tooltip("The number of AudioSources to instantiate. Roughly corresponds to the number of sound effects that can be played at once.")]
    [SerializeField] public int numAudioSources = 5;

    private Dictionary<string, AudioEvent> sounds = new Dictionary<string, AudioEvent>();

    // Specifies the music that should play in each scene.
    // No entry in this dict means no music will play.

    [SerializeField] public Dictionary<SceneID, string> perSceneMusic = new Dictionary<SceneID, string>();

    private AudioSource[] audioSources;

    private float flingPower = 0f;
    private bool soundDictInitialized = false;

    public bool musicPlaying = false;

    [Tooltip("Global game sound volume.")]
    [SerializeField] public float soundVolume = .7f;

    [Tooltip("Global game music volume.")]
    [SerializeField] public float musicVolume = .7f;

    void Awake() {
        // Singleton logic
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        // Add entries to the perSceneMusic dictionary
        perSceneMusic[SceneID.TUTORIAL_1] = "OverworldMusic";
        perSceneMusic[SceneID.TUTORIAL_2_1] = "OverworldMusic";
        perSceneMusic[SceneID.TUTORIAL_2_2] = "OverworldMusic";
        perSceneMusic[SceneID.TUTORIAL_2_3] = "OverworldMusic";
        perSceneMusic[SceneID.TUTORIAL_2_4] = "OverworldMusic";
        perSceneMusic[SceneID.STRAWBERRY_1] = "OverworldMusic";
        perSceneMusic[SceneID.STRAWBERRY_2] = "OverworldMusic";
        perSceneMusic[SceneID.KINGDOM_1] = "OverworldMusic";
        perSceneMusic[SceneID.KINGDOM_2] = "OverworldMusic";

        // Instantiate the AudioSources
        // audioSources[0] is implicitly the fling audio source
        // audioSources[1] is implicitly the music audio source
        audioSources = new AudioSource[numAudioSources + 2];

        for(int i = 0; i < numAudioSources + 2; i++) {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].playOnAwake = false;
        }
        // Debug.Log("Done loading sounds.");
    }

    void Start() {
        // Get the AudioEvents pre-defined as children of this gameObject
        AudioEvent[] audioEvents = GetComponentsInChildren<AudioEvent>();

        // To make search faster and easier, make a dictionary
        foreach(AudioEvent audioEvent in audioEvents) {
            sounds.Add(audioEvent.EventName, audioEvent);
        }
        // Debug.Log("SoundDict initialized");

        audioSources[0].clip = sounds["ChargeFling"].poolSound();
        audioSources[0].loop = true;
        audioSources[1].loop = true;

        // // DEBUG: Print sounds dict to console
        // foreach(KeyValuePair<string, AudioEvent> sound in sounds) {
        //     Debug.Log("Key = " + sound.Key + ", Value = " + sound.Value);
        // }
        soundDictInitialized = true;
        LoadSceneMusic(SceneManager.GetActiveScene());
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log("OnSceneLoaded: " + scene.name);
        // Debug.Log(mode);
        if(soundDictInitialized) {
            LoadSceneMusic(scene);
        }
    }

    void LoadSceneMusic(Scene scene) {
        if(perSceneMusic.ContainsKey(scene.ToSceneID())) {
            // Debug.Log("Loading music " + scene.ToSceneID());
            startMusic(perSceneMusic[scene.ToSceneID()]);
        } else {
            stopMusic();
        }
    }

    /// <summary>Attempts to play a sound from the specified AudioEvent.</summary>
    /// <param name="sound">The name of the AudioPool to pool from.</param>
    public bool playSoundEvent(string sound) {
        AudioClip soundToPlay = sounds[sound].poolSound();
        if(soundToPlay == null) {
            // Debug.LogError(sound + " is not a sound event!");
            return false;
        }
        // find an available AudioSource to play the sound
        // we start at index one because audioSources[0] is reserved for fling
        for(int i = 2; i < audioSources.Length; i++) {
            if(!audioSources[i]) {
                // Debug.LogError("source is null bruh");
            }
            if(!audioSources[i].isPlaying) {
                audioSources[i].clip = soundToPlay;
                audioSources[i].volume = soundVolume;
                // randomize pitch for  v a r i a t i o n (TM)
                if(sounds[sound].PitchShift) {
                    audioSources[i].pitch = Random.Range(.75f, 1.25f);
                } else {
                    audioSources[i].pitch = 1f;
                    // Debug.Log("not doing pitch shift for " + sound);
                }
                audioSources[i].Play();
                return true;
            }
        }
        // all audiosources are taken; do not play the sound effect
        return false;
    }

    ///<summary>Starts playing the audio fling sound effect.</summary>
    public void startFlingSoundEffect(float power) {
        // Debug.Log("Fling Started");
        audioSources[0].pitch = convertFlingPowerToPitch(power);
        audioSources[0].volume = soundVolume / 10f;
        audioSources[0].Play();
    }

    ///<summary>Updates the pitch the audio fling sound effect.</summary>
    public void continueFlingSoundEffect(float power) {
        // Debug.Log("Fling Continuing...");
        audioSources[0].pitch = convertFlingPowerToPitch(power);
    }

    ///<summary>Disables the audio fling sound effect.</summary>
    public void finishFlingSoundEffect() {
        // Debug.Log("Fling Finished");
        audioSources[0].Stop();
    }

    ///<sumary>Internal function that converts the fling power from the player into a pitch for the audioManager to use.</summary>
    private float convertFlingPowerToPitch(float power) {
        return power * 2 + 1f;
    }

    public void startMusic(string name) {
        // Debug.Log("Music Started:" + name);
        audioSources[1].clip = sounds[name].poolSound();
        audioSources[1].pitch = 1f;
        audioSources[1].volume = musicVolume;
        audioSources[1].Play();
        musicPlaying = true;
    }

    public void updateMusicVolume() {
        Debug.Log("updating music volume");
        audioSources[1].volume = musicVolume;
    }

    public void stopMusic() {
        // Debug.Log("Music Stopped");
        audioSources[1].Stop();
        musicPlaying = false;
    }

    public void pauseMusic() {
        audioSources[1].Pause();
        musicPlaying = false;
    }

    public void unPauseMusic() {
        audioSources[1].UnPause();
        musicPlaying = true;
    }
}
