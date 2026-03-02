using UnityEngine;

public partial class Player : IDamageVisitor
{
    public void OnNotifyFeedBackVisitor(IDamageAble damageAble)
    {
        if(damageAble is Character character)
        {
            if (character.isDead)
            {
                this.NotifyObserver(this, SubjectPlayer.NotifyEvent.OpponentKilled);
            }
            else if(character is IPostureAble postureAble
                && postureAble._posture <= 0)
            {
                this.NotifyObserver(this, SubjectPlayer.NotifyEvent.OppenentStagger);
            }
        }
        if(damageAble is IGotGunFuAttackedAble gotGunFuAttackedAble)
        {
            if (gotGunFuAttackedAble._triggerHitedGunFu
                && 
                (gotGunFuAttackedAble.curAttackerGunFuNode is GunFuHitNodeLeaf
                || gotGunFuAttackedAble.curAttackerGunFuNode is GunFuHitDownNodeLeaf
                )
                )
            {
                this.executeGauge.AddGauge(this.playerStatsScriptableObject.addExecuteGauge);
            }
        }
    }
}
