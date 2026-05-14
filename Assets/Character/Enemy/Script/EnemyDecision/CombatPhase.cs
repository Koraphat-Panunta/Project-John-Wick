using UnityEngine;

public enum CombatPhase 
{
    Suspect,//None in combat loop
    Aware,//In Combat loop but did not facing the targer directly
    Alert//In Combat loop and facing the targer directly
}
