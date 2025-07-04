using UnityEngine;

public class CombatOffensiveInstinct
{
    public float offensiveIntensity;
    private FieldOfView fieldOfView;
    private GameObject target;
    private ICombatOffensiveInstinct offensiveInstinct;
    private IFindingTarget findingTarget;

    public enum CombatPhase 
    {
        Chill,
        Suspect, 
        SemiAlert, // InCombat not SpotingTarget not recive targetSignal
        Alert, // InCombat not SpotingTarget but still sense of combat 
        FullAlert //InCombat and SpotingTarget
    }
    public CombatPhase myCombatPhase;
    public CombatOffensiveInstinct(FieldOfView fieldOfView
        ,ICombatOffensiveInstinct OffensiveInstincted
        ,IFindingTarget findingTarget)
    {
        myCombatPhase = CombatPhase.Chill;
        this.findingTarget = findingTarget;
        this.fieldOfView = fieldOfView;
        offensiveInstinct = OffensiveInstincted;
    }

    public void UpdateSening()
    {
        UpdateCombatPhase();
        TargetPointingWeaponAware();
        float coolDownOffensiveIntensity = 10;

        if(offensiveIntensity >0)
        offensiveIntensity -= coolDownOffensiveIntensity*Time.deltaTime;
    }

    
    private bool IsLineOfSightCloseEnough(Vector3 bulletLine,Vector3 referencePos,Vector3 startPos)
    {
        float raduisAware = 3;

        Vector3 startPosToRefPoint = referencePos - startPos;
        float t = Vector3.Dot(startPosToRefPoint, bulletLine) / Vector3.Dot(bulletLine,bulletLine);
        if (t >1||t<0)
            return false;
       


        Vector3 closestPoint = startPos + t*bulletLine;
        Debug.DrawLine(closestPoint, referencePos,Color.yellow);
        Debug.DrawLine(startPos, startPos + bulletLine, Color.red);

        if(Vector3.Distance(closestPoint,referencePos) < raduisAware)
            return true;

        return false;
    }
    private void IncreseBulletSuppressIntensity()
    {
        float bulletsupressIntensity = 25;

        offensiveIntensity += bulletsupressIntensity;
    }
    private void TargetPointingWeaponAware()
    {
        float lineOfsightAwareIntensity = 20;

        if(fieldOfView.FindSingleObjectInView(offensiveInstinct.targetLayer,new Vector3(0, 1.23f, 0),out GameObject target) == false)
        return;

        if(target.TryGetComponent<IWeaponAdvanceUser>(out IWeaponAdvanceUser thisTarget))
        {
            //Debug.Log("Out Target");
            if ((thisTarget._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>())
                return ;
            Vector3 aimingLine = thisTarget._shootingPos - thisTarget._currentWeapon.bulletSpawnerPos.position;
            Vector3 referencePos = offensiveInstinct.objInstict.transform.position + new Vector3(0,1,0);
            Vector3 startPos = thisTarget._currentWeapon.bulletSpawnerPos.position;

            if(IsLineOfSightCloseEnough(aimingLine,referencePos,startPos))
            {
                offensiveIntensity += lineOfsightAwareIntensity*Time.deltaTime;
            }
        }
    }

    private void UpdateCombatPhase()
    {
        FindingTarget findingTarget = this.findingTarget.findingTargetComponent;

        if (findingTarget.isSpottingTarget) {
            myCombatPhase = CombatPhase.FullAlert;
            return; 
        }


        if(findingTarget.lostSightTiming < 1){
            myCombatPhase = CombatPhase.Alert;
            return;
        }

        if(findingTarget.lostSightTiming < 3.5f){
            myCombatPhase = CombatPhase.SemiAlert;
            return;
        }

        myCombatPhase = CombatPhase.Suspect;
    }
}
