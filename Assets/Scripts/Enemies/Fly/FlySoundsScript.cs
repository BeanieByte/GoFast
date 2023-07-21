using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySoundsScript : EnemySoundsBaseScript 
{
    protected override void PlayMyFootstepSound() {
        SoundManager.Instance.PlayFlyFootstepsSound(_myLogicScript.transform.position);
    }
}
