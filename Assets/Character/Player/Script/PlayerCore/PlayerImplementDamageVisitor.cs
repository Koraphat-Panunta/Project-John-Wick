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
        if(damageAble is I_Got_OCM_Attacked_Able gotGunFuAttackedAble)
        {
            if (gotGunFuAttackedAble._triggerEnterGotAttacked_OCM
                && 
                (gotGunFuAttackedAble.curAttackerGunFuNode is GunFuHitNodeLeaf
                || gotGunFuAttackedAble.curAttackerGunFuNode is OCM_HitDownNodeLeaf
                )
                )
            {
                this.executeGauge.AddGauge(this.playerStatsScriptableObject.addExecuteGauge);
            }
        }
    }
}
