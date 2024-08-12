using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource dialogueSource;

    [Header("Volume Settings")]
    [Range(0, 1)] public float musicVolume = 0.5f;
    [Range(0, 1)] public float sfxVolume = 0.7f;
    [Range(0, 1)] public float dialogueVolume = 0.5f;

    private void Awake() {
        // Singleton pattern to ensure only one instance of SoundManager exists
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        UpdateVolumes();
    }

    // Play a music clip
    public void PlayMusic(AudioClip clip, bool loop = true) {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    // Stop the currently playing music
    public void StopMusic() {
        musicSource.Stop();
    }

    // Play a sound effect
    public void PlaySFX(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }

    // Play a dialogue clip
    public void PlayDialogue(AudioClip clip) {
        dialogueSource.clip = clip;
        dialogueSource.Play();
    }

    // Stop the currently playing dialogue
    public void StopDialogue() {
        dialogueSource.Stop();
    }

    // Update the volumes of the audio sources
    public void UpdateVolumes() {
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
        dialogueSource.volume = dialogueVolume;
    }

    // Update is called once per frame
    void Update() {
        // Example: dynamically adjust the volume in runtime (if needed)
        UpdateVolumes();
    }
}
