using UnityEngine;

public class BodyShieldHitIndicator : HittedIndicator
{
    public override void OnNotify<T>(Player player, T node)
    {
        if(node is SubjectPlayer.NotifyEvent playerEvent)
        //if (playerEvent == SubjectPlayer.NotifyEvent.HumanShieldOpponentGetShoot)
        //{
        //    Debug.Log("Hit Indicate OnNotify");
        //    Vector3 hitDir = -player.playerBulletDamageAbleBehavior.damageDetail.hitDir; // Reverse direction
        //    base.ShowIndicator(hitDir);
        //}

        base.OnNotify(player, node);
    }
   

}
