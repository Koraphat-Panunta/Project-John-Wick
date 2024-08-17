using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    float mass = 1f;
    float velocity = 60f;
    public BulletType type = BulletType._9mm;

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
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Bullet Hit :" + collision.gameObject.name);
        DrawBulletLine.bulletHitPos.Add(gameObject.transform.position);

        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        
    }
}
