using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void SendNotify(NotifyType notifyType,Enemy enemyReciever)
    {
        enemyReciever.enemyComunicate.RecievdNotify(notifyType, this._enemy);
    }
    public void SendNotify(NotifyType notifyType,float Raduis)
    {
        LayerMask targetMask = LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(this._enemy.transform.position,Raduis, targetMask);
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    if (enemy != this._enemy)
                    {
                        SendNotify(notifyType, enemy);
                    }
                }
            }
        }
    }
    public void RecievdNotify(NotifyType notifyType,Enemy enemySender)
    {
        if(notifyType == NotifyType.SendTargetLocation)
        {
            _enemy.Target.transform.position = enemySender.Target.transform.position;
            _enemy.currentTactic = new FlankingTactic(_enemy);
        }
    }
}
