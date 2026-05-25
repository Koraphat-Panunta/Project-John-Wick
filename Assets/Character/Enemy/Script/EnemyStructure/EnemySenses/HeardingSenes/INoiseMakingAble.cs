using UnityEngine;

public interface INoiseMakingAble 
{
    public NoiseMakingBehavior noiseMakingBehavior { get; set; }
    public Vector3 position { get; set; }
}
public class NoiseMakingBehavior
{
    private INoiseMakingAble noiseMakingAble;
    private static readonly Collider[] _heardingBuffer = new Collider[32];

    public NoiseMakingBehavior(INoiseMakingAble noiseMakingAble)
    {
        this.noiseMakingAble = noiseMakingAble;
    }
    public void VisitAllHeardingAbleInRaduis(float raduis,LayerMask layerMask)
    {
        int count = Physics.OverlapSphereNonAlloc(noiseMakingAble.position, raduis, _heardingBuffer, layerMask);

        for (int i = 0; i < count; i++)
        {
            if (_heardingBuffer[i].gameObject.TryGetComponent<IHeardingAble>(out IHeardingAble heardingAble))
            {
                heardingAble.GotHearding(this.noiseMakingAble);
            }
        }
    }
}
