using UnityEngine;

public class CombatOffensiveInstinct 
{
    public float offensiveIntensity;
    private FieldOfView fieldOfView;
    private GameObject target;
    public LayerMask objDomainDetect { get; set; }

    public CombatOffensiveInstinct(FieldOfView fieldOfView, LayerMask objDomainDetect)
    {
        this.fieldOfView = fieldOfView;
        this.objDomainDetect = objDomainDetect;
    }
   

    public void DomainSensingNotify()
    {
        float bulletsupressIntensity = 25;

        offensiveIntensity += bulletsupressIntensity;
    }

    public void UpdateSening()
    {
        float coolDownOffensiveIntensity = 10;
        //if(this.fieldOfView.)
        offensiveIntensity -= coolDownOffensiveIntensity*Time.deltaTime;
    }
}
