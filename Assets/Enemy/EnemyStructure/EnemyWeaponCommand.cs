using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponCommand 
{
    public WeaponSocket _primaryWeaponSocket;
    public Weapon curWeapon;
    public WeaponCommand weaponCommand;
    public Enemy enemy;
    public AmmoProuch ammoProuch;
    public EnemyWeaponCommand(Enemy enemy)
    {
        this.enemy = enemy;
        _primaryWeaponSocket = enemy.weaponSocket;
        ammoProuch = new AmmoProuch(9999, 9999, 9999, 9999);
        ammoProuch.prochReload = new AmmoProchReload(ammoProuch);
        this.enemy.StartCoroutine(GetWeapon());
    }
    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Aiming()
    {
        AimDownSightCommand aimDownSightCommand = new AimDownSightCommand(curWeapon);
        aimDownSightCommand.Execute();
    }
    public void LowReady()
    {
        LowReadyCommand lowReadyCommand = new LowReadyCommand(curWeapon);
        lowReadyCommand.Execute();
    }
    public void Fire()
    {
        PullTriggerCommand pullTriggerCommand = new PullTriggerCommand(curWeapon);
        pullTriggerCommand.Execute();
        pullTriggerCommand.TriggerCancel();
    }
    public void Reload()
    {
        ReloadCommand reloadCommand = new ReloadCommand(curWeapon,ammoProuch);
        reloadCommand.Execute();
    }
    IEnumerator GetWeapon()
    {
        curWeapon = null;
        while (curWeapon == null)
        {
            curWeapon = _primaryWeaponSocket.CurWeapon;
            yield return null;
        }
        enemy.animator.runtimeAnimatorController = _primaryWeaponSocket.weaponSingleton.GetOverride_Enemy_Controller();
    }
}
