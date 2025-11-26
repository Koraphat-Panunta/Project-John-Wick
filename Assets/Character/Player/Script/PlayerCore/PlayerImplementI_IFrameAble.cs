using UnityEngine;
using System.Threading.Tasks;
public partial class Player : I_IFrameAble
{
    [SerializeField] public float humanShiedlIFrame;
    [SerializeField] public float restrictShieldIFrame;
    public bool _isIFrame { get 
        { 
            if(isIFrame)
                return true;
            
            if((playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<IGunFuExecuteNodeLeaf>())
                return true;

            if ((playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<RestrainGunFuStateNodeLeaf>(out RestrainGunFuStateNodeLeaf restrictGunFuStateNodeLeaf)
                && restrictGunFuStateNodeLeaf._timer < restrictShieldIFrame)
                return true;

            if ((playerStateNodeManager as INodeManager).TryGetCurNodeLeaf<HumanShield_GunFu_NodeLeaf>(out HumanShield_GunFu_NodeLeaf humanShield_GunFuInteraction)
               && humanShield_GunFuInteraction.subject_GunFuAble.animationTriggerEventPlayer.timer < humanShiedlIFrame)
                return true;

            return false;
            } set => isIFrame = value; }
    private bool isIFrame { get;  set; }
    public float iFrameTime { get; private set; }
    private async void TriggerIFrame(float iFrameTime)
    {
        if (this.iFrameTime > 0)
        {
            this.iFrameTime = iFrameTime;
            return;
        }
        else
            this.iFrameTime = iFrameTime;
        await this.TaskIFrame();
    } 
    private async Task TaskIFrame()
    {
        isIFrame = true;

        while (this.iFrameTime > 0)
        {
            this.iFrameTime -= Time.deltaTime;
            await Task.Yield();
        }
        isIFrame = false;
        this.iFrameTime = 0;
    }
}
