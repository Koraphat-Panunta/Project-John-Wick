using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSingleton : MonoBehaviour
{
    public Animator animator { get; private set; }
    public GameObject UserWeapon { get; private set; }
    public WeaponSocket weaponSocket { get; private set; }
    public State CurStance { get; private set; }
    public State CurState { get; private set; }
    [SerializeField] private WeaponStateManager stateManager;
    [SerializeField] private WeaponStanceManager stanceManager;
    private void Start()
    {
        weaponSocket = GetComponentInParent<WeaponSocket>();
        UserWeapon = weaponSocket.GetWeaponUser();
        animator = weaponSocket.GetAnimator();
    

    }
    private void Update()
    {
        CurStance = stanceManager.Current_state;
        CurState = stateManager.Current_state;
    }
}
