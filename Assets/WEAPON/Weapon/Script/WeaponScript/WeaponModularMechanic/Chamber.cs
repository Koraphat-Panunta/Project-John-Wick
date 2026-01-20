using UnityEngine;

public class Chamber
{
    public bool isLoad { get => this.bullet != null?true:false ; }
    public Bullet bullet { get; protected set; }
    public Weapon weapon { get; protected set; }
    public BulletSpawner bulletSpawner { get; protected set; }

    public Chamber(Bullet bulletType,BulletSpawner bulletSpawner,Weapon weapon) 
    {
        this.bullet = bulletType;
        this.weapon = weapon;
        this.bulletSpawner = bulletSpawner;
    }
    public void Load(Bullet bullet)
    {
        if (this.bullet != null)
        {
            Debug.LogError("Load Chamber " + weapon + " Failed");
            return;
        }
        this.bullet = bullet;
    }
    public void FireTrigger() 
    {
        if(this.isLoad == false)
            return;

        this.bulletSpawner.SpawnBullet(this.bullet,this.weapon.shootingPosition);
        this.bullet = null;


    }

}
