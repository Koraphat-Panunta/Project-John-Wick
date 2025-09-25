using UnityEngine;

public class Communicator
{
   public Communicator()
   {

   }
    
    public void SendCommunicate<TypeCommunicator,T>(Vector3 position, float raduis, LayerMask layerMask, TypeCommunicator communicator,T var) where TypeCommunicator : Communicator
    {
        Collider[] target = Physics.OverlapSphere(position, raduis, layerMask);

        if (target.Length <= 0)
            return;

        for (int i = 0; i < target.Length; i++)
        {
            if (target[i].gameObject.TryGetComponent<ICommunicateAble>(out ICommunicateAble communicateAble))
            {
                communicateAble.GetCommunicate<TypeCommunicator,T>(communicator,var);
            }
        }
    }

}
