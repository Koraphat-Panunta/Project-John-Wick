using UnityEngine;

public interface ICombatOffensiveInstinct 
{
    public CombatOffensiveInstinct combatOffensiveInstinct { get; set; }
    public FieldOfView fieldOfView { get; set; }
    public LayerMask objDomainDetect { get; set; }
    public void InitailizedCombatOffensiveInstinct();
    

}
