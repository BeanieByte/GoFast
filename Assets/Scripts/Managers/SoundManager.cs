using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipReferencesSO _audioClipReferencesSO;
    [SerializeField] private Camera _gameCamera;

    private float _gameCameraCurrentWidth;
    private float _gameCameraCurrentWidthOffset = 3f;
    private float _gameCameraCurrentHeight;
    private float _gameCameraCurrentHeightOffset = 1.5f;
    private float _gameCameraWidthAndHeightDivider = 2f;

    private float _screenSizeToWorldPositionMultiplier = 0.012f;
    private float _soundSourceBorderMinXValue;
    private float _soundSourceBorderMaxXValue;
    private float _soundSourceBorderMinYValue;
    private float _soundSourceBorderMaxYValue;

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

        _gameCameraCurrentWidth = UnityEngine.Screen.width;
        _soundSourceBorderMinXValue = (_gameCamera.transform.position.x - ((_gameCameraCurrentWidth / _gameCameraWidthAndHeightDivider) * _screenSizeToWorldPositionMultiplier)) - _gameCameraCurrentWidthOffset;
        _soundSourceBorderMaxXValue = (_gameCamera.transform.position.x + ((_gameCameraCurrentWidth / _gameCameraWidthAndHeightDivider) * _screenSizeToWorldPositionMultiplier)) + _gameCameraCurrentWidthOffset;

        _gameCameraCurrentHeight = UnityEngine.Screen.height;
        _soundSourceBorderMinYValue = (_gameCamera.transform.position.y - ((_gameCameraCurrentHeight / _gameCameraWidthAndHeightDivider) * _screenSizeToWorldPositionMultiplier)) - _gameCameraCurrentHeightOffset;
        _soundSourceBorderMaxYValue = (_gameCamera.transform.position.y + ((_gameCameraCurrentHeight / _gameCameraWidthAndHeightDivider) * _screenSizeToWorldPositionMultiplier)) + _gameCameraCurrentHeightOffset;
    }

    private void FixedUpdate() {

        if (_gameCameraCurrentWidth != UnityEngine.Screen.width && _gameCameraCurrentHeight != UnityEngine.Screen.height) {

            _gameCameraCurrentWidth = UnityEngine.Screen.width;
            _gameCameraCurrentHeight = UnityEngine.Screen.height;

        }

        _soundSourceBorderMinXValue = (_gameCamera.transform.position.x - ((_gameCameraCurrentWidth / _gameCameraWidthAndHeightDivider) * _screenSizeToWorldPositionMultiplier)) - _gameCameraCurrentWidthOffset;
        _soundSourceBorderMaxXValue = (_gameCamera.transform.position.x + ((_gameCameraCurrentWidth / _gameCameraWidthAndHeightDivider) * _screenSizeToWorldPositionMultiplier)) + _gameCameraCurrentWidthOffset;

        _soundSourceBorderMinYValue = (_gameCamera.transform.position.y - ((_gameCameraCurrentHeight / _gameCameraWidthAndHeightDivider) * _screenSizeToWorldPositionMultiplier)) - _gameCameraCurrentHeightOffset;
        _soundSourceBorderMaxYValue = (_gameCamera.transform.position.y + ((_gameCameraCurrentHeight / _gameCameraWidthAndHeightDivider) * _screenSizeToWorldPositionMultiplier)) + _gameCameraCurrentHeightOffset;

    }

    //private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
    //    PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    //}

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(audioClip, position, volume * AdjustVolumeBasedOnDistance(position));
    }

    #region PlayerOnlySounds

    public void PlayPlayerWallHitSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.playerWallHit, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPlayerBurningStart(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.playerBurningStart, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPlayerBurningStop(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.playerBurningStop, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPlayerParalyzedStart(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.playerParalyzedStart, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPlayerParalyzedStop(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.playerParalyzedStop, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPlayerFreezeStart(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.playerFreezeStart, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPlayerFreezeStop(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.playerFreezeStop, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPlayerPoisonedStart(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.playerPoisonedStart, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPlayerPoisonedStop(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.playerPoisonedStop, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPlayerSlimedStart(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.playerSlimedStart, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPlayerSlimedStop(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.playerSlimedStop, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    #endregion


    #region EnemiesOnlySounds

    public void PlayFlyFootstepsSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.flyFootsteps, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayWormFootstepsSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.wormFootsteps, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayMushroomBounceSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.mushroomBounce, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayBomberGoblinThrowSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.bomberGoblinThrow, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    #endregion


    #region Player&EnemiesSharedSounds

    public void PlayFootstepSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.footstep, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayAttackSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.attacking, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayJumpSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.jumping, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayLandingSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.landing, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayHitSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.hit, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayDeathSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.death, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayBurningSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.burning, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayParalyzedSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.paralyzed, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayFreezingSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.freezing, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPoisonSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.poison, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlaySlimedSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.slimed, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    #endregion

    #region ObjectsSounds

    public void PlayCoinPickUpSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.coinPickUp, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayPowerUpPickUpSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.powerUpPickup, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayBombTickingDownSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.bombTickingDown, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    public void PlayBombExplosionSound(Vector3 position) {
        if (!CheckIfSoundCanBePlayed(position)) {
            return;
        }
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.bombExplosion, position, _defaultSoundVolume * AdjustVolumeBasedOnDistance(position));
    }

    #endregion

    #region GameAndUIElements

    public void PlayCountdownToStartSound() {
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.countdownToStart, _gameCamera.transform.position, _defaultSoundVolume - 0.2f);
    }

    public void PlayGoTextBeforeStartSound() {
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.goTextBeforeStart, _gameCamera.transform.position, _defaultSoundVolume - 0.2f);
    }

    public void PlayGameWonSound() {
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.gameWon, _gameCamera.transform.position, _defaultSoundVolume - 0.2f);
    }

    public void PlayGameLostSound() {
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.gameLost, _gameCamera.transform.position, _defaultSoundVolume - 0.2f);
    }

    public void PlayButtonHoverSound() {
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.buttonHover, _gameCamera.transform.position, _defaultSoundVolume - 0.2f);
    }

    public void PlayButtonClickSound() {
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.buttonClick, _gameCamera.transform.position, _defaultSoundVolume - 0.2f);
    }

    public void PlayMenuGoBackSound() {
        AudioSource.PlayClipAtPoint(_audioClipReferencesSO.menuGoBack, _gameCamera.transform.position, _defaultSoundVolume - 0.2f);
    }

    #endregion

    private bool CheckIfSoundCanBePlayed(Vector3 position) {

        bool isSoundSourceOnScreen = position.x > _soundSourceBorderMinXValue
            && position.x < _soundSourceBorderMaxXValue
            && position.y > _soundSourceBorderMinYValue
            && position.y < _soundSourceBorderMaxYValue;

        if (isSoundSourceOnScreen) {
            return true;
        } else {
            return false;
        }
    }

    private float AdjustVolumeBasedOnDistance(Vector3 position) {

        float currentGameObjectXPosition = position.x;
        float currentGameObjectYPosition = position.y;

        float minXPosition = 0.3f;
        float maxXPosition = 1f;

        if (currentGameObjectXPosition >= _soundSourceBorderMinXValue && currentGameObjectXPosition <= _gameCamera.transform.position.x) {

            minXPosition = _soundSourceBorderMinXValue;
            maxXPosition = _gameCamera.transform.position.x;

        } else if (currentGameObjectXPosition > _gameCamera.transform.position.x && currentGameObjectXPosition <= _soundSourceBorderMaxXValue) {

            minXPosition = _gameCamera.transform.position.x;
            maxXPosition = _soundSourceBorderMaxXValue;

        }

        currentGameObjectXPosition = Mathf.Clamp(currentGameObjectXPosition, minXPosition, maxXPosition);

        float tX = Mathf.InverseLerp(minXPosition, maxXPosition, currentGameObjectXPosition);

        float xVolumeValue = Mathf.Max(0.3f, tX);



        float minYPosition = 0.3f;
        float maxYPosition = 1f;

        if (currentGameObjectYPosition >= _soundSourceBorderMinYValue && currentGameObjectYPosition <= _gameCamera.transform.position.y) {

            minYPosition = _soundSourceBorderMinYValue;
            maxYPosition = _gameCamera.transform.position.y;

        } else if (currentGameObjectYPosition > _gameCamera.transform.position.y && currentGameObjectYPosition <= _soundSourceBorderMaxYValue) {

            minYPosition = _gameCamera.transform.position.y;
            maxYPosition = _soundSourceBorderMaxYValue;

        }

        currentGameObjectYPosition = Mathf.Clamp(currentGameObjectYPosition, minYPosition, maxYPosition);

        float tY = Mathf.InverseLerp(minYPosition, maxYPosition, currentGameObjectYPosition);

        float yVolumeValue = Mathf.Max(0.3f, tY);



        float finalVolumeValue = 0;

        if (xVolumeValue > yVolumeValue) {
            finalVolumeValue = yVolumeValue;
        }
        else if (yVolumeValue > xVolumeValue) {
            finalVolumeValue = xVolumeValue;
        }
        else if (xVolumeValue == yVolumeValue) {
            finalVolumeValue = yVolumeValue;
        }

        return finalVolumeValue;

    }
}
