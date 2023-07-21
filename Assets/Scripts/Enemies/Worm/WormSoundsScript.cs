using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSoundsScript : EnemySoundsBaseScript 
{
    protected override void PlayMyFootstepSound() {
        SoundManager.Instance.PlayWormFootstepsSound(_myLogicScript.transform.position);
    }
}
