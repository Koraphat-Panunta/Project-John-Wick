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
    public void SendCommunicate<T>(Vector3 position, float raduis, LayerMask layerMask,EnemyCommunicateMassage enemyCommunicateMassage, T var)
    {
        this.enemyCommunicateMassage = enemyCommunicateMassage;
        Collider[] target = Physics.OverlapSphere(position, raduis,layerMask.value);

        //Debug.Log("Layer = " + layerMask.value);

        if (target.Length <= 0)
            return;
        //Debug.Log("SendCommunicate 2");
        foreach (Collider collider in target) 
        {
            //Debug.Log("SendCommunicate 3" + collider);
            if (collider.gameObject.TryGetComponent<ICommunicateAble>(out ICommunicateAble communicateAble))
            {
                //Debug.Log("SendCommunicate 4" + communicateAble);
                communicateAble.GetCommunicate<EnemyCommunicator,T>(this,var);
            }
        }
       
        this.enemyCommunicateMassage = EnemyCommunicateMassage.None;
    }
}
