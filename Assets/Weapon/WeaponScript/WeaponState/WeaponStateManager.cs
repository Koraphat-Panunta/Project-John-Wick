using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStateManager : MonoBehaviour
{
    public WeaponState _currentState { get; private set; }
    public Fire fireState { get; protected set; }
    public Reload reloadState { get; protected set; }
    public None none { get; protected set; }
    [SerializeField] public WeaponSingleton _weaponSingleton;
    protected void Start()
    {
        fireState = new Fire(_weaponSingleton);
        reloadState = new Reload(_weaponSingleton);
        none = new None();
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

    protected void FixedUpdate()
    {
        _currentState.WeaponStateFixedUpdate(this);
    }
    protected void Update()
    {
        _currentState.WeaponStateUpdate(this);
    }
    

}
