using UnityEngine;

public partial class Player 
{
    public bool _isParkourCommand;

    [SerializeField] public ParkourScriptableObject climbLowScrp;
    [SerializeField] public ParkourScriptableObject climbHighScrp;
    [SerializeField] public VaultingParkourScriptableObject vaultingScrp;
}
