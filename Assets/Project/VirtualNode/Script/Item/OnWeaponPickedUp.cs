using UnityEngine;

public class OnWeaponPickedUp : VirtualEventNode,IObserverWeapon
{
    [SerializeField] Weapon weapon;
    [SerializeField] bool triggerOnce;
    private bool isAlreadyTrigger;
    private void Awake()
    {
        weapon.AddObserver(this);
    }
    public override void Execute()
    {
        base.Execute();
        isAlreadyTrigger = true;
    }

    public void OnNotify(Weapon weapon, WeaponSubject.WeaponNotifyType weaponNotify)
    {
        if(weaponNotify == WeaponSubject.WeaponNotifyType.BeenAttatch )
        {
            if(this.triggerOnce
                && this.isAlreadyTrigger == false
                )
                this.Execute();
            else if(this.triggerOnce == false
                )
                this.Execute();
        }
    }

    protected override void OnDrawGizmos()
    {
        if (isEnableGizmos 
            && weapon != null)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(this.transform.position, this.weapon.transform.position);
        }
        base.OnDrawGizmos();
    }
}
