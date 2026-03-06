using UnityEngine;

public class UpdateAnimatorIK : MonoBehaviour
    , IInitializedAble
{
    [SerializeField] private MonoBehaviour[] updateOnAnimatorIK;
    private IUpdateOnAnimator[] ikUpdate;
    public void Initialized()
    {
        if(this.updateOnAnimatorIK == null
            || this.updateOnAnimatorIK.Length <= 0)
            return;

        this.ikUpdate = new IUpdateOnAnimator[this.updateOnAnimatorIK.Length];

        for(int i = 0; i < this.updateOnAnimatorIK.Length; i++)
        {
            if (this.updateOnAnimatorIK[i] is IUpdateOnAnimator updateOnAnimator)
                this.ikUpdate[i] = updateOnAnimator;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {

        if(layerIndex == 0)
        {
            for(int i = 0;i < this.ikUpdate.Length; i++)
            {
                this.ikUpdate[i].UpdateAnimatorIK(Time.deltaTime);
            }
        }
    }
}
