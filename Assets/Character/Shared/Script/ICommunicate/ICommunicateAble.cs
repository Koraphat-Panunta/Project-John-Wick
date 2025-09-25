using System;
using UnityEngine;

public interface ICommunicateAble
{
    public GameObject communicateAble { get; }
    public Action<Communicator> NotifyCommunicate { get; set; }
    public void GetCommunicate<TypeCommunicator,T>(TypeCommunicator typeCommunicator,T var) where TypeCommunicator : Communicator;
}
