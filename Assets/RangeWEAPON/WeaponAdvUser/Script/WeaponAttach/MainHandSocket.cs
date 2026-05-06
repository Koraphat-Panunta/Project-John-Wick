using UnityEngine;

public class MainHandSocket :  WeaponSocket
{
    [SerializeField] private Character character;
    public override Transform weaponAttachingAbleTransform { get { return this.transform; } }
    public override IWeaponAdvanceUser weaponAdvanceUser => character as IWeaponAdvanceUser;


    public override void Attatch(Weapon weapon, Vector3 additionalOffsetPosition, Quaternion additionalOffsetRotation, float attatchingDuration)
    {
        //Debug.Log("weaponAdvanceUser = " + weaponAdvanceUser._weaponManuverManager.restWeaponManuverLeafNode);

        this.weaponAdvanceUser._weaponManuverManager.reloadNodeAttachAbleSelector.AddtoChildNode(weapon._reloadSelecotrOverriden);
        weapon._weaponAttacherComponent.Attach(
                this.weaponAttachingAbleTransform
                , weapon._mainHandGripTransform
                , additionalOffsetPosition
                , additionalOffsetRotation
                , attatchingDuration);
        base.Attatch(weapon, additionalOffsetPosition, additionalOffsetRotation, attatchingDuration);
      

    }

    public override void Detach()
    {
        this.weaponAdvanceUser._weaponManuverManager.reloadNodeAttachAbleSelector.RemoveNode(this.curWeaponAtSocket._reloadSelecotrOverriden);
        base.Detach();
    }

    private void OnValidate()
    {
        if(character == null)
            character = GetComponentInParent<Character>();
    }
}
