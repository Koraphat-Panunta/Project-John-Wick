using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 originalPos;
    float mass = 1f;
    float velocity = 1f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        // Set Rigidbody properties
        rb.mass = mass;
        rb.drag = 0.01f;
        rb.angularDrag = 0.05f;
        originalPos = gameObject.transform.position;
        // Calculate and apply impulse force
        Vector3 force = transform.forward * mass * velocity;
        rb.AddForce(force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet Hit :" + collision.gameObject.name);
        //DrawBulletLine.DrawLine(originalPos,gameObject.transform.position);
        Destroy(gameObject);
   
    }
   
    private void OnDrawGizmos()
    {
        
    }
}
