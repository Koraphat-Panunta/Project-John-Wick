using UnityEngine;

public partial class Player 
{
    public bool _isParkourCommand;

    [SerializeField] public ClimbParkourScriptableObject climbLowScrp;
    [SerializeField] public ClimbParkourScriptableObject climbHighScrp;
    [SerializeField] public VaultingParkourScriptableObject vaultingScrp;
}
