using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Serialization;

/// <summary>
/// Singleton responsible for controlling music and sfx
/// </summary>
public class AudioManager : MonoBehaviour
{   
    public static AudioManager instance {get; private set;}

    #region Private Fields
    
    private const int FLING_INDEX = 0;
    private const int MUSIC_INDEX = 1;
    
    private Dictionary<string, AudioEvent> _sounds;

    // Specifies the music that should play in each scene.
    // No entry in this dict means no music will play.
    private Dictionary<SceneID, string> _perSceneMusic;

    private AudioSource[] _audioSources;

    private bool _soundDictInitialized;

    #endregion

    #region Serialized Fields & Properties
    
    [FormerlySerializedAs("musicPlaying")] 
    public bool _musicPlaying;
    
    [Tooltip("The number of AudioSources to instantiate. Roughly corresponds to the number of sound effects that can be played at once.")]
    [SerializeField] private int _numAudioSources = 5;
    
    [FormerlySerializedAs("soundVolume")]
    [Tooltip("Global game sound volume.")]
    [SerializeField] private float _soundVolume = .7f;
    public float soundVolume { get => _soundVolume; set => _soundVolume = value; }

    [FormerlySerializedAs("MUSIC_VOLUME_MODIFIER")]
    [Tooltip("Multiplier for the music volume.")]
    [SerializeField] private float _musicVolumeModifier = .6f;
    
    [FormerlySerializedAs("musicVolume")]
    [Tooltip("Global game music volume.")]
    [SerializeField] private float _musicVolume = .7f;
    public float musicVolume
    {
        get => _musicVolume * _musicVolumeModifier;
        set => _musicVolume = value;
    }

    #endregion

    #region Monobehaviour Methods
    private void Awake() {
        // Singleton logic
        if(instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }

        // Add entries to the perSceneMusic dictionary
        _perSceneMusic = new Dictionary<SceneID, string>
        {
            [SceneID.TUTORIAL_1] = "OverworldMusic",
            [SceneID.TUTORIAL_2_1] = "OverworldMusic",
            [SceneID.TUTORIAL_2_2] = "OverworldMusic",
            [SceneID.TUTORIAL_2_3] = "OverworldMusic",
            [SceneID.TUTORIAL_2_4] = "OverworldMusic",
            [SceneID.STRAWBERRY_1] = "OverworldMusic",
            [SceneID.STRAWBERRY_2] = "OverworldMusic",
            [SceneID.KINGDOM_1] = "OverworldMusicDistorted",
            [SceneID.KINGDOM_2] = "OverworldMusicDistorted"
        };

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

    private void Start() {
        // Get the AudioEvents pre-defined as children of this gameObject
        AudioEvent[] audioEvents = GetComponentsInChildren<AudioEvent>();

        // To make search faster and easier, make a dictionary
        _sounds = new Dictionary<string, AudioEvent>();
        foreach(AudioEvent audioEvent in audioEvents) {
            _sounds.Add(audioEvent.EventName, audioEvent);
        }

        // Initialize fling charge clip
        _audioSources[FLING_INDEX].clip = _sounds["ChargeFling"].poolSound();
        _audioSources[FLING_INDEX].loop = true;
        
        // Initialize music
        _audioSources[MUSIC_INDEX].loop = true;
        _soundDictInitialized = true;
        StartSceneMusic(SceneManager.GetActiveScene());
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    #endregion
    
    #region Public Methods
    public void UpdateMusicVolume() {
        Debug.Log("updating music volume");
        _audioSources[MUSIC_INDEX].volume = _musicVolume * _musicVolumeModifier;
    }

    public void PauseMusic()
    {
        Tween fadeOut = FadeMusicTween(musicVolume, 0f);
        fadeOut.OnComplete(
            () =>
            {
                _audioSources[MUSIC_INDEX].Pause();
                _musicPlaying = false;
            }
        );
    }

    public void UnPauseMusic()
    {
        _audioSources[MUSIC_INDEX].UnPause();
        FadeMusicTween(0.0f, musicVolume);
        _musicPlaying = true;
    }
    
    /// <summary>Attempts to play a sound from the specified AudioEvent.</summary>
    /// <param name="sound">The name of the AudioPool to pool from.</param>
    public bool PlaySoundEvent(string sound) {
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
                _audioSources[i].volume = _soundVolume;
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
        // all AudioSources are taken; do not play the sound effect
        return false;
    }
    
    ///<summary>Starts playing the audio fling sound effect.</summary>
    public void StartFlingSoundEffect(float power) {
        // Debug.Log("Fling Started");
        _audioSources[FLING_INDEX].pitch = ConvertFlingPowerToPitch(power);
        _audioSources[FLING_INDEX].volume = _soundVolume / 10f;
        _audioSources[FLING_INDEX].Play();
    }

    ///<summary>Updates the pitch the audio fling sound effect.</summary>
    public void ContinueFlingSoundEffect(float power) {
        // Debug.Log("Fling Continuing...");
        _audioSources[FLING_INDEX].pitch = ConvertFlingPowerToPitch(power);
    }

    ///<summary>Disables the audio fling sound effect.</summary>
    public void FinishFlingSoundEffect() {
        // Debug.Log("Fling Finished");
        _audioSources[FLING_INDEX].Stop();
    }
    
    #endregion
    
    #region Private Methods
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log("OnSceneLoaded: " + scene.name);
        // Debug.Log(mode);
        if(_soundDictInitialized) {
            StartSceneMusic(scene);
        }
    }

    private void StartSceneMusic(Scene scene) {
        if(_perSceneMusic.ContainsKey(scene.ToSceneID())) {
            // Debug.Log("Loading music " + scene.ToSceneID());
            string music = _perSceneMusic[scene.ToSceneID()];
            if (!MusicAlreadyPlaying(music)) {
                StartMusic(music);
            }
        } else {
            StopMusic();
        }
    }

    ///<summary>Internal function that converts the fling power from the player into a pitch for the audioManager to use.</summary>
    private static float ConvertFlingPowerToPitch(float power) {
        return power * 2 + 1f;
    }

    /// <summary>Return true if the music file with the given name is currently playing</summary>
    private bool MusicAlreadyPlaying(string songName) {
        // fragile if we change names of clip or audio. Consider an enum?
        bool sameName = _audioSources[MUSIC_INDEX]?.clip?.name == songName;
        bool currentlyPlaying = _audioSources[MUSIC_INDEX]?.isPlaying ?? false;
        return (sameName && currentlyPlaying);
    }

    private void StartMusic(string songName) {
        // Debug.Log("Music Started:" + name);
        _audioSources[MUSIC_INDEX].clip = _sounds[songName].poolSound();
        _audioSources[MUSIC_INDEX].pitch = 1f;
        _audioSources[MUSIC_INDEX].Play();
        _musicPlaying = true;

        FadeMusicTween(0.0f, musicVolume);
    }

    private void StopMusic() {
        // Debug.Log("Music Stopped");
        Tween fadeOut = FadeMusicTween(musicVolume, 0f);
        fadeOut.OnComplete(
            () =>
            {
                _audioSources[MUSIC_INDEX].Stop();
                _musicPlaying = false;
            }
        );
    }
    
    /// <summary>
    /// Gradually fades the music in or out depending on parameter value
    /// </summary>
    /// <param name="startVolume">Volume to immediately set the music to.</param>
    /// <param name="endVolume">Volume that the music will be at by the end of the tween.</param>
    /// <returns>The tween that will fade the music in or out, to be modified as necessary</returns>
    private Tween FadeMusicTween(float startVolume, float endVolume)
    {
        _audioSources[MUSIC_INDEX].DOKill(); // kill any existing fade tween
        _audioSources[MUSIC_INDEX].volume = startVolume;
        return _audioSources[MUSIC_INDEX].DOFade(endVolume, 2f);
    }
    
    #endregion
}
