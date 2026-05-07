using UnityEngine;

public partial class Enemy : IMeleeWeaponUserAble
{
   

    public IMeleeAttackNodeLeaf _curMeleeNodeLeaf
    {
        get 
        {
            if(this.stateManagerNode.TryGetCurNodeLeaf<IMeleeAttackNodeLeaf>(out IMeleeAttackNodeLeaf meleeAttackNodeLeaf) == false)
                return null;

            return meleeAttackNodeLeaf;
        }
    }

    public MeleeAttackingPhase _curMeleeAttackPhase 
    {
        get
        {
            if (this._curMeleeNodeLeaf == null)
                return MeleeAttackingPhase.None;

            return this._curMeleeNodeLeaf._attackingPhase;
        }
    }

    public MeleeWeapon _curMeleeWeapon => this.MainHandSocket.curMeleeWeapon;

    public Transform _meleeWeaponUserTransform => this.transform;

    public Transform _targetTransform => this.target; 

    public bool _isPerformAttackAble 
    {
        get 
        {
            if(
                this.isDead
                || this._isInPain
                || this._isFallDown
                || this._posture <= 0
                )
                return false;

            return true;
        }
    }

    public void OnNotifyMeleeAttack<T>(T var)
    {
        this.NotifyObserver<T>(this, var);
    }
}
