using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgentMovementOverride 
{
    private NavMeshAgent agent;
    private bool isOverride = false;
    public EnemyAgentMovementOverride(NavMeshAgent agent)
    {
        this.agent = agent;
        this.agent.speed = 0;
        this.agent.acceleration = 0;
    }
    public void Update()
    {
        isOverride = false;
        this.agent.speed = 0;
        this.agent.acceleration = 0;
    }
    public void LateUpdate()
    {
        if(isOverride == false)
        {
            this.agent.speed = 0;
            this.agent.acceleration = 0;
        }
    }
    public void OverrideAgentInFrame(float speed,float acceleration)
    {
        isOverride = true;
        this.agent.speed = speed;
        this.agent.acceleration = acceleration;
    }
}
