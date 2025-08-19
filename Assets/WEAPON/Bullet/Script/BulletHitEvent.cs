using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class BulletHitEvent : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    private Bullet bullet;
    [SerializeField] ParticleSystem spark;
    private ObjectPooling<ParticleSystem> particleSpark;
    private void Awake()
    {
        StartCoroutine(Initialized());
    }

    private IEnumerator Initialized()
    {
        yield return new WaitUntil(() => weapon.didAwake);
        this.particleSpark = new ObjectPooling<ParticleSystem>(spark, 12, 3, Vector3.zero);
        weapon.bullet.bulletHitNotify += OnBulletHit;
    }
    private void OnBulletHit(Collider hitedBullet,Vector3 hitPos,Vector3 bulletDir)
    {
        if (hitedBullet.TryGetComponent<IBulletDamageAble>(out IBulletDamageAble bulletDamageAble) && bulletDamageAble is Armored_Protection)
        {
            ParticleSystem particle = particleSpark.Get();
            particle.transform.position = hitPos;
            particle.transform.forward = bulletDir*-1;
            _ = ParticleUpdate(particle);
        }
    }
    private async Task ParticleUpdate(ParticleSystem particle)
    {
        particle.Play();
        while (particle.isPlaying)
        {
            await Task.Yield();
        }
        particleSpark.ReturnToPool(particle);
    }
}
