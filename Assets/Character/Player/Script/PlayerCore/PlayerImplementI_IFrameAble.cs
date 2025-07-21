using UnityEngine;
using System.Threading.Tasks;
public partial class Player : I_IFrameAble
{
    bool I_IFrameAble._isIFrame { get => isIFrame; set => isIFrame = value; }
    public bool isIFrame { get; private set; }
    private float iFrameTime;
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
        while(this.iFrameTime > 0)
        {
            this.iFrameTime -= Time.deltaTime;
            await Task.Yield();
        }
        isIFrame = false;
        this.iFrameTime = 0;
    }
}
