using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingSensing : IEnvironmentAware
{
    private IHearingComponent userHearing;
    private float hearingDistance;
    public HearingSensing(IHearingComponent userHearing,Environment environment,float hearingDistance) 
    {
        this.userHearing = userHearing;
        environment.Add_Listener(this);
        this.hearingDistance = hearingDistance;
    }
    public void OnAware(GameObject sourceFrom, EnvironmentType environmentType)
    {
        if(sourceFrom.TryGetComponent<IWeaponAdvanceUser>(out IWeaponAdvanceUser target) && environmentType == EnvironmentType.Sound){

            if (Vector3.Distance(sourceFrom.transform.position, userHearing.userHearing.transform.position) <= hearingDistance)
                userHearing.GotHearding(sourceFrom);
        }
    }
}
