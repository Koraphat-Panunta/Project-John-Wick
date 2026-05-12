using UnityEngine;
using static EnemyBodyBulletDamageAbleBehavior;

public class ChestBodyPart : BodyPart
{
    public override void OnNotify<T>(Enemy enemy, T node)
    {
        base.OnNotify(enemy, node);
    }
}
