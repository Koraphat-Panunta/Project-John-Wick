using UnityEngine;

public class EnemyCommunicator : Communicator
{
    private Enemy enemy;
    public EnemyCommunicator(Enemy enemy) 
    { 
    }
   public enum EnemyCommunicateMassage
   {
        None,
        SendTargetPosition
   }
    public EnemyCommunicateMassage enemyCommunicateMassage { get;private set; }
    public void SendCommunicate(Vector3 position, float raduis, LayerMask layerMask,EnemyCommunicateMassage enemyCommunicateMassage)
    {
        this.enemyCommunicateMassage = enemyCommunicateMassage;
        Collider[] target = Physics.OverlapSphere(position, raduis, layerMask);

        if (target.Length <= 0)
            return;

        for (int i = 0; i < target.Length; i++)
        {
            if (target[i].gameObject.TryGetComponent<ICommunicateAble>(out ICommunicateAble communicateAble)
                &&communicateAble.communicateAble != enemy)
            {
                
                communicateAble.GetCommunicate<EnemyCommunicator>(this);
            }
        }
        this.enemyCommunicateMassage = EnemyCommunicateMassage.None;
    }
}
