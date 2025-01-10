using UnityEngine;

public interface ICoverUseable 
{
    public Vector3 peekPos { get; set; }
    public Vector3 coverPos { get; set; }
    public CoverPoint coverPoint { get; set; }
    public Character userCover { get; set; }
    public FindingCover findingCover { get; set; }
    public bool isInCover { get; set; }
    public void InitailizedCoverUsable();
}
