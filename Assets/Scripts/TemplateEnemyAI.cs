using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class TemplateEnemyAI : MonoBehaviour {
    
    public float sight = 5f;
    public float trackingSight = 20f;
    public LayerMask cannotSeeThrough;
    public float maxLostTime = 5f;
    public float minAttackDistance = 2f;
    public float maxAttackDistance = 3f;
    [Range(0f, 1f)]
    public float aggressiveness = .5f;
    public float minAttackInterval = 1.5f;
    public float maxAttackInterval = 3f;

    public NavMeshAgent agent;
    public Rigidbody rigidbody;
    public Transform target;
    public Vector3 originalPos;
    public TemplateEnemyState currentState;
    public float playerLastSeenTime = -1000;
    public Vector3 playerLastSeenPosition;
    public bool canAttack = true;

    public CoroutineTask attackCooldown;

    // Start is called before the first frame update
    void Awake() {
        if (target == null && PlayerStats.player) target = PlayerStats.player.transform;
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        originalPos = transform.position;
        playerLastSeenPosition = originalPos;
        // Behavior test
        currentState = TemplateEnemyState.Approaching;
        attackCooldown = new CoroutineTask(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount > 2) agent.enabled = true;

        if (!agent || !target || !agent.isOnNavMesh) {
            rigidbody.velocity = Vector3.zero;
            if (agent) agent.nextPosition = rigidbody.position;
            return;
        }

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
        MoveTowards(originalPos);

        if (!CanSeePlayer(sight)) return;
        
        currentState = TemplateEnemyState.Approaching;
        return;
    }

    void UpdateApproaching() {
        float dist;
        if (!CanSeePlayer(trackingSight, out dist)) {
            currentState = TemplateEnemyState.Seeking;
            return;
        }

        if (dist <= minAttackDistance) {
            currentState = TemplateEnemyState.Attacking;
            return;
        }
        
        MoveTowards(target.position);
    }

    void UpdateSeeking()
    {
        if (Time.time - playerLastSeenTime > maxLostTime) {
            currentState = TemplateEnemyState.Idle;
            return;
        }

        MoveTowards(playerLastSeenPosition);
        if (CanSeePlayer(trackingSight)) currentState = TemplateEnemyState.Approaching;
    }
    
    void UpdateAttacking()
    {
        //Add attack behaviors
        float dist;
        if (!CanSeePlayer(trackingSight, out dist)) {
            currentState = TemplateEnemyState.Seeking;
            return;
        }

        if (dist > maxAttackDistance) {
            currentState = TemplateEnemyState.Approaching;
            return;
        }
        
        StopMovement();
        
        TryAttack();
    }

    void MoveTowards(Vector3 pos) {
        agent.SetDestination(pos);
        rigidbody.velocity = agent.desiredVelocity;
        agent.nextPosition = rigidbody.position;
    }

    void StopMovement() {
        agent.ResetPath();
        rigidbody.velocity = Vector3.zero;
        agent.nextPosition = rigidbody.position;
    }
    
    bool CanSeePlayer(float s, out float dist) {
        var currPos = transform.position;
        var tarPos = target.position;

        var direction = tarPos - currPos;
        dist = direction.magnitude;

        if (dist > s) return false;

        var canSee = !Physics.Linecast(currPos, tarPos, cannotSeeThrough.value, QueryTriggerInteraction.Ignore);
        
        Debug.DrawLine(currPos, tarPos, canSee ? Color.green : Color.red);

        if (!canSee) return false;
        
        playerLastSeenTime = Time.time;
        playerLastSeenPosition = tarPos;

        return true;
    }

    bool CanSeePlayer(float s) => CanSeePlayer(s, out var dist);

    void TryAttack() {
        if (!canAttack) return;
        if (Random.value > aggressiveness) return;
        
        Attack();

        attackCooldown.StartCoroutine(AttackCooldown(Random.Range(minAttackInterval, maxAttackInterval)));
    }

    void Attack() {
        print("Attack");
    }

    IEnumerator AttackCooldown(float cooldown) {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    [Conditional("UNITY_EDITOR")]
    private void OnDrawGizmos() {
        if (currentState == TemplateEnemyState.Seeking) {
            Gizmos.color = new Color(1.0f, .0f, .0f, 1.0f - ((Time.time - playerLastSeenTime) / maxLostTime));
            Gizmos.DrawCube(playerLastSeenPosition, Vector3.one);
        }
    }

    public enum TemplateEnemyState
    {
        Idle = 0,
        Approaching = 1,
        Seeking = 2,
        Attacking = 3
    }
}
