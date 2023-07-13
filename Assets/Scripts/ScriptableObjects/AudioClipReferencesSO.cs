using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AudioClipReferencesSO")]
public class AudioClipReferencesSO : ScriptableObject
{
    [Header("Player and Enemies")]
    public AudioClip footstep;
    public AudioClip jumping;
    public AudioClip landing;
    public AudioClip attacking;
    public AudioClip hit;
    public AudioClip playerWallHit;
    public AudioClip death;
    public AudioClip burning;
    public AudioClip playerBurningStart;
    public AudioClip playerBurningStop;
    public AudioClip paralyzed;
    public AudioClip playerParalyzedStart;
    public AudioClip playerParalyzedStop;
    public AudioClip freezing;
    public AudioClip playerFreezeStart;
    public AudioClip playerFreezeStop;
    public AudioClip poison;
    public AudioClip playerPoisonedStart;
    public AudioClip playerPoisonedStop;
    public AudioClip slimed;
    public AudioClip playerSlimedStart;
    public AudioClip playerSlimedStop;
    public AudioClip wormFootsteps;
    public AudioClip mushroomBounce;
    public AudioClip bomberGoblinThrow;

    [Header("Objects")]
    public AudioClip coinPickUp;
    public AudioClip powerUpPickup;
    public AudioClip bombTickingDown;
    public AudioClip bombExplosion;

    [Header("Game and UI elements")]
    public AudioClip countdownToStart;
    public AudioClip goTextBeforeStart;
    public AudioClip gameWon;
    public AudioClip gameLost;
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public AudioClip menuGoBack;

}
