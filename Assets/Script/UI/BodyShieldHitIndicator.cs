using UnityEngine;

public class BodyShieldHitIndicator : HittedIndicator
{
    public override void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.HumanShieldOpponentGetShoot)
        {
            Debug.Log("Hit Indicate OnNotify");
            Vector3 hitDir = -player.playerBulletDamageAbleBehavior.damageDetail.hitDir; // Reverse direction
            base.ShowIndicator(hitDir);
        }
    }
}
