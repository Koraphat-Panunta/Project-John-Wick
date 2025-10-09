using UnityEngine;
using UnityEngine.UI;

public class EnemyHPInWorldUI : InWorldUI
{
    [SerializeField] Image hpBarImage;
    private float curHpEnemy;
    private float maxHpEnemy;

    public void SetValue(float curHp,float maxHp)
    {
        this.curHpEnemy = curHp;
        this.maxHpEnemy = maxHp;
    }
    protected override void FixedUpdate()
    {
        hpBarImage.fillAmount = curHpEnemy / maxHpEnemy;
        base.FixedUpdate();
    }
}
