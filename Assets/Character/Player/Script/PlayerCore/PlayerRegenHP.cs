using UnityEngine;

public partial class Player
{
    private float regenHPDisableTime = 5;
    private float regenHPDisableTimer;
    [Range(0.1f,100)]
    [SerializeField] private float regenSpeed;
    private void RegenHPUpdate()
    {
        if (regenHPDisableTimer > 0)
            regenHPDisableTimer -= Time.deltaTime;

        if(GetHP() < (maxHp*0.5f) && regenHPDisableTimer <= 0)
        {
            AddHP(Mathf.Abs((maxHp*0.5f) - GetHP()));
            NotifyObserver(this, SubjectPlayer.NotifyEvent.HealthRegen);
        }
    }
}
