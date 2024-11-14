using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    float mass = 1f;
    float velocity = 240;
    public BulletType type = BulletType._9mm;
    public float damage = 40;

    void Start()
    {

    }
    public void ShootDirection(Vector3 Dir)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        // Set Rigidbody properties
        rb.mass = mass;
        rb.linearDamping = 0.01f;
        rb.angularDamping = 0.05f;
        // Calculate and apply impulse force
        Vector3 force = Dir * mass * velocity;
        rb.AddForce(force, ForceMode.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<BodyPart>(out BodyPart bodyPart))
        {
            bodyPart.GotHit(damage);
        }
        if(collision.collider.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(damage);
        }
        DrawBulletLine.bulletHitPos.Add(gameObject.transform.position);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        
    }
}
