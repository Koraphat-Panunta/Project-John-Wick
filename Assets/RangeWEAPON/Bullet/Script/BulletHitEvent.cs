using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class BulletHitEvent : MonoBehaviour,IInitializedAble
{
    [SerializeField] Weapon weapon;
    private Bullet bullet;
    [SerializeField] ParticleSystem spark;
    [SerializeField] ParticleSystem bloodSplit;
    private ObjectPooling<ParticleSystem> particleSpark;
    private ObjectPooling<ParticleSystem> bloodSplits;

    public SoundData bulletHitFresh;
    public SoundData bulletHitArmored;
    public SoundData bulletHitDefault;
    
    
    private void OnBulletHit(Collider hitedBullet,Vector3 hitPos,Vector3 bulletDir)
    {
        if (hitedBullet.TryGetComponent<IBulletDamageAble>(out IBulletDamageAble bulletDamageAble) )
        {
            switch (bulletDamageAble)
            {

                case Armored_Protection armored_Protection:
                    {
                        ParticleSystem particle = particleSpark.Get();
                        particle.transform.position = hitPos;
                        particle.transform.forward = bulletDir * -1;
                        _ = ParticleUpdate(particle,particleSpark);

                        SoundBuilder soundBuilder = SoundEmitterManager.Instance.CreateSoundBuilder(hitPos,this.bulletHitArmored);
                        soundBuilder.Play();

                        break;
                    }
                case BodyPart bodyPart:
                    {
                        ParticleSystem particle = bloodSplits.Get();
                        particle.transform.position = hitPos;
                        particle.transform.forward = bulletDir * -1;
                        _ = ParticleUpdate(particle,bloodSplits);

                        SoundBuilder soundBuilder = SoundEmitterManager.Instance.CreateSoundBuilder(hitPos, this.bulletHitFresh);
                        soundBuilder.Play();
                        break;
                    }
            }
           
        }

    }
    private async Task ParticleUpdate(ParticleSystem particle,ObjectPooling<ParticleSystem> objectPoolingReturn)
    {
        particle.Play();
        while (particle.isPlaying)
        {
            await Task.Yield();
        }
        objectPoolingReturn.ReturnToPool(particle);
    }

    public void Initialized()
    {
        this.particleSpark = new ObjectPooling<ParticleSystem>(spark, 12, 3, Vector3.zero);
        this.bloodSplits = new ObjectPooling<ParticleSystem>(bloodSplit, 12, 3, Vector3.zero);
        weapon.bullet.bulletHitNotify += OnBulletHit;
    }
}
