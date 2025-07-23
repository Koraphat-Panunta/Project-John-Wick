using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPbarDisplay : MonoBehaviour,IObserverEnemy,IGotPointingAble
{
    [SerializeField] Canvas hpBarCanvas;
    [SerializeField] Image hpBarImage;
    [SerializeField] Image hpBarMinusImage;
    [SerializeField] Enemy enemy;

    [Range(0,10)]
    [SerializeField] private float displayTime;

    [SerializeField] private float displayElapseTime;

    [Range(0, 10)]
    [SerializeField] private float minusVelocity;

    Task updateHpBar;
    Task showHpBarFading;
    private bool isDead => enemy.isDead;

    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
        Debug.Log("Enemy Notify ");
        if (enemyEvent == SubjectEnemy.EnemyEvent.GotBulletHit)
        {
            Debug.Log("Enemy Notify GotBulletHit");
            hpBarImage.rectTransform.localScale = new Vector3(enemy.GetHP()/enemy.GetMaxHp(), hpBarImage.rectTransform.localScale.y, hpBarImage.rectTransform.localScale.z);

            if(updateHpBar == null || updateHpBar.IsCompleted)
            updateHpBar = UpdateHpBar();
        }
    }

    public void Notify<T>(Enemy enemy, T node) where T : INode
    {
        if(node is EnemyStateLeafNode enemyStateLeafNode)
            switch (enemyStateLeafNode)
            {
                case IGotGunFuAttackNode gotGunFuAttackAbleNode:
                    {
                        ShowingUp();
                        break;
                    }
                case EnemyDeadStateNode gotDeadStateNode: 
                    {
                        hpBarImage.rectTransform.localScale = new Vector3(0, hpBarImage.rectTransform.localScale.y, hpBarImage.rectTransform.localScale.z);

                        if (updateHpBar == null || updateHpBar.IsCompleted)
                            updateHpBar = UpdateHpBar();
                        break;
                    }
            }
    }
    public void NotifyPointingAble(IPointerAble pointter) => ShowingUp();
   
    private void ShowingUp()
    {
        if (isDead)
            return;

        displayElapseTime = displayTime;

        if (showHpBarFading == null || showHpBarFading.IsCompleted)
            showHpBarFading = ShowBarFading();
    }
    private async Task UpdateHpBar()
    {
        while (hpBarMinusImage.rectTransform.localScale.x >= hpBarImage.rectTransform.localScale.x)
        {
            hpBarMinusImage.rectTransform.localScale = Vector3.MoveTowards(hpBarMinusImage.rectTransform.localScale, hpBarImage.rectTransform.localScale, minusVelocity * Time.deltaTime);
            await Task.Yield();
        }

    }
    private async Task ShowBarFading()
    {
       
        hpBarImage.color = new Color(hpBarImage.color.r,hpBarImage.color.g,hpBarImage.color.b,1);
        hpBarMinusImage.color = new Color(hpBarMinusImage.color.r, hpBarMinusImage.color.g, hpBarMinusImage.color.b, 1);

        while (displayElapseTime > 0)
        {
            displayElapseTime -= Time.deltaTime;

            hpBarCanvas.transform.LookAt(Camera.main.transform.position);


            float alpha = displayElapseTime / displayTime;

            hpBarImage.color = new Color(hpBarImage.color.r, hpBarImage.color.g, hpBarImage.color.b, alpha);
            hpBarMinusImage.color = new Color(hpBarMinusImage.color.r, hpBarMinusImage.color.g, hpBarMinusImage.color.b, alpha);

            await Task.Yield();
        }

       
            
    }

    private void Awake()
    {
        enemy.AddObserver(this);
    }

}
