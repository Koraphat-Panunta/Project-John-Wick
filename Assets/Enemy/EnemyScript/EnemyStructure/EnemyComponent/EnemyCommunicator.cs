using UnityEngine;

public class EnemyCommunicator : Communicator
{
    public Enemy enemy { get; private set; }
    public EnemyCommunicator(Enemy enemy) 
    {
        this.enemy = enemy;
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
        Collider[] target = Physics.OverlapSphere(position, raduis,layerMask.value);

        Debug.Log("Layer = " + layerMask.value);

        if (target.Length <= 0)
            return;
        Debug.Log("SendCommunicate 2");
        foreach (Collider collider in target) 
        {
            Debug.Log("SendCommunicate 3" + collider);
            if (collider.gameObject.TryGetComponent<ICommunicateAble>(out ICommunicateAble communicateAble)
                && communicateAble.communicateAble != enemy)
            {
                Debug.Log("SendCommunicate 4" + communicateAble);
                communicateAble.GetCommunicate<EnemyCommunicator>(this);
            }
        }
       
        this.enemyCommunicateMassage = EnemyCommunicateMassage.None;
    }
}
