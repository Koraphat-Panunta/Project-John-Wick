using UnityEngine;

public interface INoiseMakingAble 
{
    public NoiseMakingBehavior noiseMakingBehavior { get; set; }
    public Vector3 position { get; set; }
}
public class NoiseMakingBehavior
{
    private INoiseMakingAble noiseMakingAble;
    public NoiseMakingBehavior(INoiseMakingAble noiseMakingAble) 
    {
        this.noiseMakingAble = noiseMakingAble;
    }
    public void VisitAllHeardingAbleInRaduis(float raduis,LayerMask layerMask)
    {
        Collider[] target = Physics.OverlapSphere(noiseMakingAble.position, raduis, layerMask);

        Debug.Log("VisitAllHeardingAbleInRaduis target = " + target.Length);

        if(target.Length <=0)
            return;

        for (int i = 0; i < target.Length; i++) 
        {
            Debug.Log("Notify Got hearding Overlap " + target[i]);
            if (target[i].gameObject.TryGetComponent<IHeardingAble>(out IHeardingAble heardingAble))
            {
                Debug.Log("Notify Got hearding " + heardingAble);
                heardingAble.GotHearding(this.noiseMakingAble);
            }
        }
    }
}
