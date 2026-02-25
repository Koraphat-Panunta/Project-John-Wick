using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public abstract class Character : MonoBehaviour,IInitializedAble
{
    protected float HP;
    protected float maxHp;
    public bool enableRootMotion;

    public Transform _hipBone;
    public Transform _leftUpperLegBone;
    public Transform _leftLowerLegBone;
    public Transform _leftFootBone;
    public Transform _rightUpperLegBone;
    public Transform _rightLowerLegBone;
    public Transform _rightFootBone;
    public Transform _spine_0_Bone;
    public Transform _spine_1_Bone;
    public Transform _spine_2_Bone;
    public Transform _leftShoulderBone;
    public Transform _leftArmBone;
    public Transform _leftForeArmBone;
    public Transform _leftHandBone;

    public Transform _neckBone;
    public Transform _headBone;

    public Transform _rightShoulderBone;
    public Transform _rightArmBone;
    public Transform _rightForeArmBone;
    public Transform _rightHandBone;

    [SerializeField] MovementScriptableObject movementScriptableObject;

    public float StandMoveAccelerate { get => this.movementScriptableObject.StandMoveAccelerate; }
    public float StandMoveMaxSpeed { get => this.movementScriptableObject.StandMoveMaxSpeed; }

    public float CrouchMoveAccelerate { get => this.movementScriptableObject.CrouchMoveAccelerate; }
    public float CrouchMoveMaxSpeed { get => this.movementScriptableObject.CrouchMoveMaxSpeed; }

    public float rotateSpeed { get => this.movementScriptableObject.moveRotateSpeed; }

    public float sprintAccelerate { get => this.movementScriptableObject.sprintAccelerate; }
    public float sprintMaxSpeed { get => this.movementScriptableObject.sprintMaxSpeed; }
    public float sprintRotateSpeed { get => this.movementScriptableObject.sprintRotateSpeed; }

    public float breakDecelerate { get => this.movementScriptableObject.breakDecelerate; }

    public virtual bool isDead { get 
        {
            if(HP <=0)
                return true;
            else return false;
        }
    }

    public abstract Stance stance { get; }

    public abstract MovementCompoent _movementCompoent { get; /*protected*/ set; }
    [SerializeField] public CharacterMovementController characterController;
    //public Weapon curentWeapon;
    //public Transform weaponSocket;
    public Animator animator;
    int frame;
    [SerializeField] private float SumDeltaPos;
    protected virtual void OnAnimatorMove()
    {
        if (this.enableRootMotion)
        {
            frame++;

            SumDeltaPos += animator.deltaPosition.magnitude;

            _movementCompoent.SetPosition(this.characterController.position + animator.deltaPosition);
            _movementCompoent.SetRotation(this.transform.rotation * animator.deltaRotation);

            //Debug.Log("curPos = " + this.characterController.position);
            //Debug.Log("frame "+frame+"\n"+"SumDeltaPos = "+this.SumDeltaPos);

        }
        else
        {
            SumDeltaPos = 0;
            frame = 0;
        }

    }

    public virtual void Initialized()
    {
        
    }
  
   
    public float GetHP()
    {
        return HP;
    }
    public float GetMaxHp() => maxHp;
    public void SetHP(float HP)
    {
        this.HP = Mathf.Clamp(HP,0,GetMaxHp());
    }
    public void AddHP(float HP)
    {
        this.HP = Math.Clamp(this.HP+HP, 0, this.maxHp);
        
    }

  
}
