using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBodyPart : BodyPart,IHeardingAble,ICommunicateAble,I_UI_InWorldPlaceAble
{


   
    public override void OnNotify<T>(Enemy enemy, T node)
    {
      
    }
    #region ImplementCommunicate
    public GameObject communicateAble => enemy.communicateAble;
    public Action<Communicator> NotifyCommunicate { get => enemy.NotifyCommunicate; set => enemy.NotifyCommunicate = value; }
    public void GetCommunicate<TypeCommunicator,T>(TypeCommunicator typeCommunicator,T var) where TypeCommunicator : Communicator => enemy.GetCommunicate(typeCommunicator,var);
    #endregion

    #region ImplementGotHearding
    public Action<INoiseMakingAble> NotifyGotHearing { get => enemy.NotifyGotHearing; set => enemy.NotifyGotHearing = value; }

    public void GotHearding(INoiseMakingAble noiseMaker) => enemy.GotHearding(noiseMaker);
    #endregion



}
