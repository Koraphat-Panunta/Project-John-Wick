using UnityEngine;

public class BulletCapacity 
{
    public Bullet bullet { get; set; }
    public int maxCapacity { get; set; }
    public int curCount { get; set; }

    public bool Load(Bullet bullet,int amout,out int overAmout)
    {
        overAmout = 0;

        if (this.bullet.GetType() != bullet.GetType())
            return false;

        this.curCount += amout;
        if(this.curCount > this.maxCapacity)
        {
            overAmout = this.curCount - this.maxCapacity;
            this.curCount -= overAmout;
        }

        return true;

    }
    public bool GetBulletOut(out Bullet bullet)
    {
        bullet = this.bullet;
        if(curCount <= 0)
            return false;

        this.curCount -= 1;
        return true;
    }
    public int UnLoadAllBullet()
    {
        int bulletNumber = this.curCount;

        this.curCount = 0;

        return bulletNumber;
    }
    public void UnLoadAllBullet(out int amoutUnLoad)
    {
       amoutUnLoad = UnLoadAllBullet();
    }
}
