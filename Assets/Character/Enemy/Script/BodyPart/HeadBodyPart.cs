using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBodyPart : BodyPart,IHeardingAble,ICommunicateAble,I_UI_InWorldPlaceAble
{
    public override float hpReciverMultiplyRate { get; set; }
    public override float postureReciverRate { get; set; }
    public override float staggerReciverRate { get; set; }


    protected override void Start()
    {
        base.Start();

        hpReciverMultiplyRate = 2.0f;
        postureReciverRate = 3.0f;
        staggerReciverRate = 3.0f;
    }
   
    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        enemy._painPart = IPainStateAble.PainPart.Head;
        base.TakeDamage(damageVisitor);
    }
    public override void TakeDamage(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {
        TakeDamage(damageVisitor);
        base.TakeDamage(damageVisitor, hitPart, hitDir, hitforce);
    }
    public override void Notify<T>(Enemy enemy, T node)
    {
       
        base.Notify(enemy, node);
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
