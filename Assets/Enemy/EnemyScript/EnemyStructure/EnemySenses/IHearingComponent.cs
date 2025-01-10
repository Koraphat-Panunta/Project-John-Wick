using UnityEngine;

public interface IHearingComponent 
{
   public GameObject userHearing { get; set; }
   public Environment environment { get; }
   public HearingSensing hearingSensing { get; set; }
    public void InitailizedHearingComponent();
   public void GotHearding(GameObject souceSound);
}
