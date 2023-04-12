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
    private const int FLING_INDEX = 0;
    private const int MUSIC_INDEX = 1;
    
    [Tooltip("Modifies the music volume.")]
    [SerializeField] private const float MUSIC_VOLUME_MODIFIER = .6f;

    public static AudioManager instance {get; private set;}

    [Tooltip("The number of AudioSources to instantiate. Roughly corresponds to the number of sound effects that can be played at once.")]
    [SerializeField] private int _numAudioSources = 5;

    private Dictionary<string, AudioEvent> _sounds;

    // Specifies the music that should play in each scene.
    // No entry in this dict means no music will play.

    [SerializeField] private Dictionary<SceneID, string> _perSceneMusic;

    private AudioSource[] _audioSources;

    private float _flingPower = 0f;
    private bool _soundDictInitialized = false;

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
        _perSceneMusic = new Dictionary<SceneID, string>();
        _perSceneMusic[SceneID.TUTORIAL_1] = "OverworldMusic";
        _perSceneMusic[SceneID.TUTORIAL_2_1] = "OverworldMusic";
        _perSceneMusic[SceneID.TUTORIAL_2_2] = "OverworldMusic";
        _perSceneMusic[SceneID.TUTORIAL_2_3] = "OverworldMusic";
        _perSceneMusic[SceneID.TUTORIAL_2_4] = "OverworldMusic";
        _perSceneMusic[SceneID.STRAWBERRY_1] = "OverworldMusic";
        _perSceneMusic[SceneID.STRAWBERRY_2] = "OverworldMusic";
        _perSceneMusic[SceneID.KINGDOM_1] = "OverworldMusicDistorted";
        _perSceneMusic[SceneID.KINGDOM_2] = "OverworldMusicDistorted";

        // Instantiate the AudioSources
        // audioSources[FLING_INDEX] is implicitly the fling audio source
        // audioSources[MUSIC_INDEX] is implicitly the music audio source
        _audioSources = new AudioSource[_numAudioSources + 2];

        for(int i = 0; i < _numAudioSources + 2; i++) {
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
            _audioSources[i].playOnAwake = false;
        }
        // Debug.Log("Done loading sounds.");
    }

    void Start() {
        // Get the AudioEvents pre-defined as children of this gameObject
        AudioEvent[] audioEvents = GetComponentsInChildren<AudioEvent>();

        // To make search faster and easier, make a dictionary
        _sounds = new Dictionary<string, AudioEvent>();
        foreach(AudioEvent audioEvent in audioEvents) {
            _sounds.Add(audioEvent.EventName, audioEvent);
        }
        // Debug.Log("SoundDict initialized");

        _audioSources[FLING_INDEX].clip = _sounds["ChargeFling"].poolSound();
        _audioSources[FLING_INDEX].loop = true;
        _audioSources[MUSIC_INDEX].loop = true;

        // // DEBUG: Print sounds dict to console
        // foreach(KeyValuePair<string, AudioEvent> sound in sounds) {
        //     Debug.Log("Key = " + sound.Key + ", Value = " + sound.Value);
        // }
        _soundDictInitialized = true;
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
        if(_soundDictInitialized) {
            LoadSceneMusic(scene);
        }
    }

    void LoadSceneMusic(Scene scene) {
        if(_perSceneMusic.ContainsKey(scene.ToSceneID())) {
            // Debug.Log("Loading music " + scene.ToSceneID());
            string music = _perSceneMusic[scene.ToSceneID()];
            if (!musicAlreadyPlaying(music)) {
                startMusic(music);
            }
        } else {
            stopMusic();
        }
    }

    /// <summary>Attempts to play a sound from the specified AudioEvent.</summary>
    /// <param name="sound">The name of the AudioPool to pool from.</param>
    public bool playSoundEvent(string sound) {
        AudioClip soundToPlay = _sounds[sound].poolSound();
        if(soundToPlay == null) {
            // Debug.LogError(sound + " is not a sound event!");
            return false;
        }
        // find an available AudioSource to play the sound
        for(int i = 0; i < _audioSources.Length; i++) {
            // skip reserved indices
            if (i == FLING_INDEX || i == MUSIC_INDEX) { continue; }

            if(!_audioSources[i].isPlaying) {
                _audioSources[i].clip = soundToPlay;
                _audioSources[i].volume = soundVolume;
                // randomize pitch for  v a r i a t i o n (TM)
                if(_sounds[sound].PitchShift) {
                    _audioSources[i].pitch = Random.Range(.75f, 1.25f);
                } else {
                    _audioSources[i].pitch = 1f;
                    // Debug.Log("not doing pitch shift for " + sound);
                }
                _audioSources[i].Play();
                return true;
            }
        }
        // all audiosources are taken; do not play the sound effect
        return false;
    }

    ///<summary>Starts playing the audio fling sound effect.</summary>
    public void startFlingSoundEffect(float power) {
        // Debug.Log("Fling Started");
        _audioSources[FLING_INDEX].pitch = convertFlingPowerToPitch(power);
        _audioSources[FLING_INDEX].volume = soundVolume / 10f;
        _audioSources[FLING_INDEX].Play();
    }

    ///<summary>Updates the pitch the audio fling sound effect.</summary>
    public void continueFlingSoundEffect(float power) {
        // Debug.Log("Fling Continuing...");
        _audioSources[FLING_INDEX].pitch = convertFlingPowerToPitch(power);
    }

    ///<summary>Disables the audio fling sound effect.</summary>
    public void finishFlingSoundEffect() {
        // Debug.Log("Fling Finished");
        _audioSources[FLING_INDEX].Stop();
    }

    ///<summary>Internal function that converts the fling power from the player into a pitch for the audioManager to use.</summary>
    private float convertFlingPowerToPitch(float power) {
        return power * 2 + 1f;
    }

    /// <summary>Return true if the music file with the given name is currently playing</summary>
    private bool musicAlreadyPlaying(string name) {
        // TODO fragile if we change names of clip or audio. Make an enum?
        bool sameName = _audioSources[MUSIC_INDEX]?.clip?.name == name;
        bool currentlyPlaying = _audioSources[MUSIC_INDEX]?.isPlaying ?? false;
        return (sameName && currentlyPlaying);
    }

    public void startMusic(string name) {
        // Debug.Log("Music Started:" + name);
        _audioSources[MUSIC_INDEX].clip = _sounds[name].poolSound();
        _audioSources[MUSIC_INDEX].pitch = 1f;
        _audioSources[MUSIC_INDEX].volume = musicVolume * MUSIC_VOLUME_MODIFIER;
        _audioSources[MUSIC_INDEX].Play();
        musicPlaying = true;
    }

    public void updateMusicVolume() {
        Debug.Log("updating music volume");
        _audioSources[MUSIC_INDEX].volume = musicVolume * MUSIC_VOLUME_MODIFIER;
    }

    public void stopMusic() {
        // Debug.Log("Music Stopped");
        _audioSources[MUSIC_INDEX].Stop();
        musicPlaying = false;
    }

    public void pauseMusic() {
        _audioSources[MUSIC_INDEX].Pause();
        musicPlaying = false;
    }

    public void unPauseMusic() {
        _audioSources[MUSIC_INDEX].UnPause();
        musicPlaying = true;
    }
}
