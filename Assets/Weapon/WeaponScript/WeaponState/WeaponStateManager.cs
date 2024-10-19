using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStateManager 
{
    public WeaponState _currentState { get; private set; }
    public Fire fireState { get; protected set; }
    public Reload reloadState { get; protected set; }
    public None none { get; protected set; }
    public Weapon _weapon;
    public WeaponStateManager(Weapon weapon)
    {
        this._weapon = weapon;
        fireState = new Fire(this._weapon);
        reloadState = new Reload(this._weapon,weapon.reloadSpeed);
        none = new None(this._weapon);
        _currentState = none;
        _currentState.EnterState();
    }
    public void ChangeState(WeaponState Nextstate)
    {
        if(_currentState != Nextstate) 
        {
            _currentState.ExitState();
            _currentState = Nextstate;
            _currentState.EnterState();
        }
    }

    public void FixedUpdate()
    {
        _currentState.WeaponStateFixedUpdate(this);
    }
    public void Update()
    {
        _currentState.WeaponStateUpdate(this);
    }
    

}
