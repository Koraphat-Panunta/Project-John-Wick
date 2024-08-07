using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MyEnemyController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    public int frame = 0;
    private void OnValidate()
    {
        if(agent!) agent = GetComponent<NavMeshAgent>();
        if (animator!)animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (agent.hasPath)
        {
            Debug.Log("Has path frame = " + frame);
            Vector3 dir = agent.steeringTarget - transform.position;
            Vector3 animDir = transform.InverseTransformDirection(dir);
            float dot = Vector3.Dot(transform.position, dir);
            bool isFacingDir;
            if(dot > 0.65f)
            {
                isFacingDir = true;
            }
            else
            {
                isFacingDir= false;
            }

            animator.SetFloat("Vertical",animDir.z,0.5f,Time.deltaTime);
            animator.SetFloat("Horizontal", animDir.x,0.1f,Time.deltaTime);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);
            if (Vector3.Distance(transform.position, agent.destination) <= agent.radius)
            {
                agent.ResetPath();
            }
        }
        else
        {
            animator.SetFloat("Vertical",math.lerp(animator.GetFloat("Vertical"),0,2*Time.deltaTime));
            animator.SetFloat("Horizontal",math.lerp(animator.GetFloat("Horizontal"),0,2*Time.deltaTime));
            Debug.Log(agent.destination);
        }
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var Hitray = Physics.Raycast(ray, out RaycastHit hit, 100);
            if (Hitray == true)
            {
                agent.destination = hit.point;
                Debug.Log("Click frame = "+frame);
                if(agent.hasPath)
                {
                    Debug.Log("has path");
                }
            }

        }
        if (agent.hasPath)
        {
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
            {
                Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.green);
            }
        }
    }
    private void OnDrawGizmos()
    {
       
    }
}
