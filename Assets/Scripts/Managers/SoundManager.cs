using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipReferencesSO _audioClipReferencesSO;

    private float _defaultSoundVolume = 0.8f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

        }
        Instance = this;
    }

    private void Start() {
        
    }

    //private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
    //    PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    //}

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    public void PlayPlayerFootstepsSound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.footstep, position, _defaultSoundVolume);
    }

    private void OnDestroy() {
        
    }
}
