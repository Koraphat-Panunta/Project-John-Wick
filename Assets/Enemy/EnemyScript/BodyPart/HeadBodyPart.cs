using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBodyPart : BodyPart,IHeardingAble,ICommunicateAble
{
    public override float hpReciverRate { get; set; }
    public override float postureReciverRate { get; set; }
  

    protected override void Start()
    {
        base.Start();

        hpReciverRate = 8.0f;
        postureReciverRate = 3.0f;

    }
   
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        Bullet bulletObj = damageVisitor as Bullet;

        float damage = bulletObj.hpDamage * hpReciverRate;
        float pressureDamage = bulletObj.impactDamage * postureReciverRate;

        enemy._isPainTrigger = true;
        enemy._painPart = IPainStateAble.PainPart.Head;

        if (enemy._posture > 0)
            enemy._posture -= pressureDamage;

        enemy.TakeDamage(damage);
    }
    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {

        TakeDamage(damageVisitor);

        base.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);
    }

    #region ImplementCommunicate
    public GameObject communicateAble => enemy.communicateAble;
    public Action<Communicator> NotifyCommunicate { get => enemy.NotifyCommunicate; set => enemy.NotifyCommunicate = value; }
    public void GetCommunicate<TypeCommunicator>(TypeCommunicator typeCommunicator) where TypeCommunicator : Communicator => enemy.GetCommunicate(typeCommunicator);
    #endregion

    #region ImplementGotHearding
    public Action<INoiseMakingAble> NotifyGotHearing { get => enemy.NotifyGotHearing; set => enemy.NotifyGotHearing = value; }
    public void GotHearding(INoiseMakingAble noiseMaker) => enemy.GotHearding(noiseMaker);
    #endregion



}
