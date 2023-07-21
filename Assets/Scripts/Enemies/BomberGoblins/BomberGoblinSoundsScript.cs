using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberGoblinSoundsScript : EnemySoundsBaseScript 
{
    public override void PlayAttackSound() {
        SoundManager.Instance.PlayBomberGoblinThrowSound(_myLogicScript.transform.position);
    }
}
