using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyComunicate;

public class EnemyComunicate
{
    // Start is called before the first frame update
    private Enemy _enemy;
    public EnemyComunicate(Enemy enemy)
    {
        _enemy = enemy;
    }
    public enum NotifyType
    {
        SendTargetLocation,
    }
    public void SendNotify(NotifyType notifyType, Enemy enemyReciever)
    {
        enemyReciever.enemyComunicate.RecievdNotify(notifyType, this._enemy);
    }
    public void SendNotify(NotifyType notifyType, float Raduis)
    {
        LayerMask targetMask = LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(this._enemy.transform.position, Raduis, targetMask);
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<ChestBodyPart>(out ChestBodyPart enemy))
                {
                    if (enemy.enemy != this._enemy && enemy.enemy.GetHP() > 0)
                    {
                        SendNotify(notifyType, enemy.enemy);
                    }
                }
            }
        }
    }
    public void RecievdNotify(NotifyType notifyType, Enemy enemySender)
    {
        if (notifyType == NotifyType.SendTargetLocation)
        {
            _enemy.Target.transform.position = enemySender.Target.transform.position;
        }
    }
}
