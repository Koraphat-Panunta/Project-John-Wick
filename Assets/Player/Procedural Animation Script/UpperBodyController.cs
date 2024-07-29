using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class UpperBodyController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] MultiAimConstraint AimConstraint;
    void Start()
    {
        AimConstraint = GetComponent<MultiAimConstraint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        WeaponActionEvent.Scubscibtion(WeaponActionEvent.WeaponEvent.Aim, UpperBodyTurn);
        WeaponActionEvent.Scubscibtion(WeaponActionEvent.WeaponEvent.LowReady, UpperBodyTurn);
    }
    private void OnDisable()
    {
        WeaponActionEvent.UnSubscirbe(WeaponActionEvent.WeaponEvent.Aim, UpperBodyTurn);
        WeaponActionEvent.UnSubscirbe(WeaponActionEvent.WeaponEvent.LowReady, UpperBodyTurn);
    }
    private void UpperBodyTurn(Weapon weapon)
    {
        if(weapon.weapon_StanceManager.Current_state == weapon.weapon_StanceManager.aimDownSight)
        {
            AimConstraint.weight = 1;
        }
        else
        {
            AimConstraint.weight = 0;
        }
       
    }
}
