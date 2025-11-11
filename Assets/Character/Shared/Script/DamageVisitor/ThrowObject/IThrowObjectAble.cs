using UnityEngine;

public interface IThrowObjectAble 
{
   
    public IThrowAbleObject curThrowAbleObject { get; set; }
    public IBeenThrewObjectAt curBeenThrowObjectAt { get; set; }
    public void ObjectBeenHit(IThrowAbleObject threwObject,Collider hited);
}
