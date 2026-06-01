using UnityEngine;

[CreateAssetMenu(fileName = "WeaponHandIK_ConstraintSCRP", menuName = "ScriptableObjects/ConstrainObject/TwoBoneIK_ConstraintSCRP/WeaponHandIK_ConstraintSCRP")]
public class WeaponHandIK_ConstraintSCRP : TwoBoneIK_ConstraintSCRP
{
    public WeaponHandRecoilSCRP recoilData;
    public WeaponHandBlockSCRP blockData;
}
