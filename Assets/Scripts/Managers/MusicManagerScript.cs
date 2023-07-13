using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{
    public static MusicManagerScript Instance { get; private set; }

    private AudioSource _musicAudioSource;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);

        }
        Instance = this;

        _musicAudioSource = GetComponent<AudioSource>();
        _musicAudioSource.Stop();
    }

    public void PlayMusic() {
        _musicAudioSource.Play();
    }

    public void PauseMusic() {
        _musicAudioSource.Pause();
    }

    public void UnPauseMusic() {
        _musicAudioSource.UnPause();
    }

    public void StopMusic() {
        _musicAudioSource.Stop();
    }
}
