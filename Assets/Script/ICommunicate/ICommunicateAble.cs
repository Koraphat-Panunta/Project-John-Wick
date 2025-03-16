using System;
using UnityEngine;

public interface ICommunicateAble
{
    public GameObject communicateAble { get; }
    public Action<Communicator> NotifyCommunicate { get; set; }
    public void GetCommunicate<TypeCommunicator>(TypeCommunicator typeCommunicator) where TypeCommunicator : Communicator;
}
