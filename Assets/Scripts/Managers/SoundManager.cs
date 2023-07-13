using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioClipReferencesSO _audioClipReferencesSO;

    private void Start() {
        
    }

    //private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
    //    PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    //}

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void OnDestroy() {
        
    }
}
