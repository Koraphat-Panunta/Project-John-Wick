using UnityEngine;

public partial class Player : IThrowObjectAble
{
    [SerializeField] public AnimationTriggerEventSCRP throwObjectAnimationTriggerEventSCRP;
    public bool _isTriggerThrowCommand;
    private IThrowAbleObject throwableObj;
    public IThrowAbleObject curThrowAbleObject { get 
        {
            if(_currentWeapon != null )
                    return _currentWeapon;
            else return this.throwableObj;
        } set => throwableObj = value;
    }
    public IBeenThrewObjectAt curBeenThrowObjectAt { get; set; }

    public void ObjectBeenHit(IThrowAbleObject threwObject, Collider hited)
    {

    }
}
