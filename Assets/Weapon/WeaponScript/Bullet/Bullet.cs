using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    float mass = 1f;
    float velocity = 40f;
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
        rb.drag = 0.01f;
        rb.angularDrag = 0.05f;
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
            Debug.Log("BodyPartHit");
        }
        DrawBulletLine.bulletHitPos.Add(gameObject.transform.position);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        
    }
}
