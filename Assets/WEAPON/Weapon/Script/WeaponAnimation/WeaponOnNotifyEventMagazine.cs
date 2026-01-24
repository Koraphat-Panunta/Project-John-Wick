using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;

public class WeaponOnNotifyEventMagazine : MonoBehaviour,IObserverWeapon,IInitializedAble
{
    public UnityEvent pickMag_In; // หยิบMagIn
    public UnityEvent pullMag_Out; // ดึงMagOut
    public UnityEvent releaseMag_Out; //ปลดMagOut
    public UnityEvent putMag_In; //ปลดMagOut
    public UnityEvent keepMag_Out;

    [SerializeField] public Weapon weapon;

    // Start is called before the first frame update
    public void Initialized()
    {
        this.weapon.AddObserver(this);
    }
    private void OnValidate()
    {
        if (this.weapon == null)
        {
            this.weapon = GetComponent<Weapon>();
        }
       
    }
    public void OnNotify<T>(Weapon weapon, T weaponNotify)
    {
        if(weaponNotify is IReloadMagazineNode.ReloadMagazineEvent reloadMagazineEvent)
        {

            switch(reloadMagazineEvent)
            {
                case IReloadMagazineNode.ReloadMagazineEvent.PickUpMag_In:
                    this.pickMag_In.Invoke();
                    break;
                case IReloadMagazineNode.ReloadMagazineEvent.ReleaseMag:
                    {
                        try
                        {
                            if ((weapon.userWeapon._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<TacticalReloadMagazineFullStageNodeLeaf>())
                                this.pullMag_Out.Invoke();
                            else
                                this.releaseMag_Out.Invoke();
                        }
                        catch
                        {
                            Debug.LogError("OnNotify Weapon " + weapon+ "ReleaseMag ");
                        }
                        break;
                    }
                case IReloadMagazineNode.ReloadMagazineEvent.InputMag:
                    this.putMag_In.Invoke();
                    break;
                case IReloadMagazineNode.ReloadMagazineEvent.KeepMag_Out:
                    this.keepMag_Out.Invoke();
                    break;
            }    
            //if (reloadMagazineEvent == IReloadMagazineNode.ReloadMagazineEvent.PickUpMag_In)
            //    this.pickMag_In.Invoke();
        }
    }

}
