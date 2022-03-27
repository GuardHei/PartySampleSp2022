using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TemplateEnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rigidbody;
    public Transform target;
    public Vector3 originalPos;
    public TemplateEnemyState currentState;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        originalPos = transform.position;
        //Behavior test
        currentState = TemplateEnemyState.Approaching;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount > 2) agent.enabled = true;

        if (!agent || !target || !agent.isOnNavMesh) return;

        switch (currentState) 
        {
            case TemplateEnemyState.Idle: UpdateIdle();
            break;
            case TemplateEnemyState.Approaching: UpdateApproaching();
            break;
            case TemplateEnemyState.Seeking: UpdateSeeking();
            break;
            case TemplateEnemyState.Attacking: UpdateAttacking();
            break;
        }
    }

    // void fixedUpdate()
    // {
    //     rigidbody.velocity = agent.desiredVelocity;
    //     Debug.Log(agent.desiredVelocity);
    //     agent.nextPosition = rigidbody.position;
    // }

    //Sees the player --> approaching --> attacking if within certain range & angle
    //If lost sight --> seeking --> return to idle if for too long
    //use agent.velocity & assign it to the rigidbody to move the character
    //Assign velocity to 0 if the character stops
    //Manually control the rotation of the character: force it to rotate

    void UpdateIdle()
    {
        agent.SetDestination(originalPos);
        rigidbody.velocity = agent.desiredVelocity;
        agent.nextPosition = rigidbody.position;
    }

    void UpdateApproaching()
    {
        agent.SetDestination(target.position);
        rigidbody.velocity = agent.desiredVelocity;
        agent.nextPosition = rigidbody.position;
    }

    void UpdateSeeking()
    {
        
    }
    
    void UpdateAttacking()
    {
        //Add attack behaviors
    }

    public enum TemplateEnemyState
    {
        Idle = 0,
        Approaching = 1,
        Seeking = 2,
        Attacking = 3
    }
}
