using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioClip _defaultLevelMusic;

    [Header("Player's SFX")]
    [SerializeField] private AudioClip _playerRunningSFX;
    [SerializeField] private AudioClip _playerJumpingSFX;
    [SerializeField] private AudioClip _playerAttackingSFX;
    [SerializeField] private AudioClip _playerHitSFX;
    [SerializeField] private AudioClip _playerDeathSFX;
    [SerializeField] private AudioClip _playerBurningSFX;
    [SerializeField] private AudioClip _playerParalyzedSFX;
    [SerializeField] private AudioClip _playerFreezeSFX;
    [SerializeField] private AudioClip _playerPoisonSFX;
    [SerializeField] private AudioClip _playerSlimeSFX;

    [Header("Enemies SFX")]
    [SerializeField] private AudioClip _enemyRegularWalkingSFX;
    [SerializeField] private AudioClip _enemySloshyWalingSFX;
    [SerializeField] private AudioClip _enemySlideWalkingSFX;
    [SerializeField] private AudioClip _enemyAttackSFX;
    [SerializeField] private AudioClip _enemyHitSFX;
    [SerializeField] private AudioClip _enemyDeathSFX;
    [SerializeField] private AudioClip _enemyBounceableSFX;
    [SerializeField] private AudioClip _bombTickingDownSFX;
    [SerializeField] private AudioClip _bombExplosionSFX;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);

        }
        Instance = this;
    }

    public void PlayLevelMusic() { 
        
    }
}
