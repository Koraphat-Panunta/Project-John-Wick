using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class BulletHitEvent : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    private Bullet bullet;
    [SerializeField] ParticleSystem spark;
    [SerializeField] ParticleSystem bloodSplit;
    private ObjectPooling<ParticleSystem> particleSpark;
    private ObjectPooling<ParticleSystem> bloodSplits;
    private ObjectPooling<AudioSource> bulletHitSound;
    [SerializeField] private AudioSource bulletHitPrefab;
    [SerializeField] private AudioClip hitArmor;
    [SerializeField] private AudioClip hitFresh;
    private void Awake()
    {
        StartCoroutine(Initialized());
    }

    private IEnumerator Initialized()
    {
        yield return new WaitUntil(() => weapon.didAwake);
        this.particleSpark = new ObjectPooling<ParticleSystem>(spark, 12, 3, Vector3.zero);
        this.bloodSplits = new ObjectPooling<ParticleSystem>(bloodSplit, 12, 3, Vector3.zero);
        this.bulletHitSound = new ObjectPooling<AudioSource>(bulletHitPrefab, 10, 3, Vector3.zero);
        weapon.bullet.bulletHitNotify += OnBulletHit;
    }
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

                        AudioSource hitSound = bulletHitSound.Get();
                        hitSound.transform.position = hitPos;
                        hitSound.PlayOneShot(hitArmor);
                        _ = AudioHitUpdate(hitSound);
                        break;
                    }
                case BodyPart bodyPart:
                    {
                        ParticleSystem particle = bloodSplits.Get();
                        particle.transform.position = hitPos;
                        particle.transform.forward = bulletDir * -1;
                        _ = ParticleUpdate(particle,bloodSplits);

                        AudioSource hitSound = bulletHitSound.Get();
                        hitSound.transform.position = hitPos;
                        hitSound.PlayOneShot(hitFresh);
                        _ = AudioHitUpdate(hitSound);
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
    private async Task AudioHitUpdate(AudioSource audioSource)
    {
        await Task.Delay((int)(audioSource.clip.length * 1000));
        bulletHitSound.ReturnToPool(audioSource);
    }
}
