using UnityEngine;

public class EnemyCommunicator : Communicator
{
    public EnemyCommunicator()
    {

    }
   public enum EnemyCommunicateMassage
   {
        None,
        SendTargetPosition
   }
    public EnemyCommunicateMassage enemyCommunicateMassage { get; set; }
    private static readonly Collider[] _communicateBuffer = new Collider[32];
    public void SendCommunicate<T>(Vector3 position, float raduis, LayerMask layerMask,EnemyCommunicateMassage enemyCommunicateMassage, T var)
    {
        this.enemyCommunicateMassage = enemyCommunicateMassage;
        int count = Physics.OverlapSphereNonAlloc(position, raduis, _communicateBuffer, layerMask.value);

        if (count <= 0)
            return;

        for (int i = 0; i < count; i++)
        {
            if (_communicateBuffer[i].gameObject.TryGetComponent<ICommunicateAble>(out ICommunicateAble communicateAble))
            {
                communicateAble.GetCommunicate<EnemyCommunicator,T>(this,var);
            }
        }
       
        this.enemyCommunicateMassage = EnemyCommunicateMassage.None;
    }
}
