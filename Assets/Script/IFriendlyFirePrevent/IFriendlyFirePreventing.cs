using UnityEngine;

public interface IFriendlyFirePreventing 
{
   public enum FriendlyFirePreventingMode
    {
        Disable,
        Enable,
    }
    public FriendlyFirePreventingMode curFriendlyFireMode { get; set; }
    public int allieID { get; set; }
    public FriendlyFirePreventingBehavior friendlyFirePreventingBehavior { get; set; }
    public bool IsFriendlyCheck(IFriendlyFirePreventing friendlyFirePreventing) => friendlyFirePreventingBehavior.IsFriendlyCheck(friendlyFirePreventing);
    public void DisableFriendlyFirePreventing() => friendlyFirePreventingBehavior.DisableFriendlyFirePreventing();
    public void EnableFriendlyFirePreventing() => friendlyFirePreventingBehavior.EnableFriendlyFirePreventing();
}
public class FriendlyFirePreventingBehavior
{
    private IFriendlyFirePreventing friendlyFirePreventing { get; set; }
    public FriendlyFirePreventingBehavior(IFriendlyFirePreventing friendlyFirePreventing)
    {
        this.friendlyFirePreventing = friendlyFirePreventing;
        this.friendlyFirePreventing.curFriendlyFireMode = IFriendlyFirePreventing.FriendlyFirePreventingMode.Enable;
    }
    public bool IsFriendlyCheck(IFriendlyFirePreventing friendlyFirePreventing)
    {
        if(this.friendlyFirePreventing.curFriendlyFireMode == IFriendlyFirePreventing.FriendlyFirePreventingMode.Disable)
            return false;

        if(this.friendlyFirePreventing.allieID == friendlyFirePreventing.allieID)
            return true;

        return false;
    }
    public void DisableFriendlyFirePreventing()
    {
        this.friendlyFirePreventing.curFriendlyFireMode = IFriendlyFirePreventing.FriendlyFirePreventingMode.Disable;
    }
    public void EnableFriendlyFirePreventing()
    {
        this.friendlyFirePreventing.curFriendlyFireMode = IFriendlyFirePreventing.FriendlyFirePreventingMode.Enable;
    }
}
