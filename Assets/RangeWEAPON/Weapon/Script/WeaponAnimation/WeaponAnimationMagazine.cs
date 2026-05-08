using UnityEngine;

public class WeaponAnimationMagazine : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Range(0, 1)]
    [SerializeField] public float sliceBarrelNormalized;

    [SerializeField] RangeWeapon weapon;
    

    public void Update()
    {
        if (weapon.chamber.isLoad
            && weapon.curBulletCapacity > 0)
        {
            this.sliceBarrelNormalized = Mathf.Clamp01(this.sliceBarrelNormalized - Time.deltaTime * 5);
        }
            
        this.animator.SetFloat("BarrelOpen", this.sliceBarrelNormalized);
    }

    public void TriggerBarrel()
    {
        this.sliceBarrelNormalized = 1;
    }


}
