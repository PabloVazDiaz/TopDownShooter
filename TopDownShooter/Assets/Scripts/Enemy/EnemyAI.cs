using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IPooledObject
{
    [SerializeField] int Damage;
    [SerializeField] float AttackRange;
    [SerializeField] float AttackCooldown;
    [SerializeField] float rotationSpeed;

    EnemyAttack AttackType;

    Transform player;
    NavMeshAgent nav;
    bool seekingPlayer = true;
    bool attackingPlayer = false;
    Rigidbody rb;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        GetComponent<Health>().OnDeath += DisableNavMesh;
        AttackType = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody>();
    }

    void DisableNavMesh(Vector3 position)
    {
        seekingPlayer = false;
        nav.enabled = false;
    }

    private void Update()
    {
        if (seekingPlayer)
        {
            SeekingBehaviour();
            
        }else if (attackingPlayer)
        {
            AttackBehaviour();
        }
    }


    private void SeekingBehaviour()
    {
        nav.enabled = true;
        nav.SetDestination(player.position);
        if (AttackType.IsTargetInRange(player))
        {
            DisableNavMesh(transform.position);
            attackingPlayer = true;
        }
        else
        {
            attackingPlayer = false;
        }
    }


    private void AttackBehaviour()
    {
        FaceTarget();
        AttackType.PerformAttack(player);
        seekingPlayer = true;
    }

    private void FaceTarget()
    {
        Quaternion RotationToPlayer = Quaternion.LookRotation(player.position - transform.position);
        Quaternion newRotation = Quaternion.Lerp(transform.rotation, RotationToPlayer, rotationSpeed * Time.deltaTime);
        rb.MoveRotation(newRotation);
    }

    public void OnObjectSpawn()
    {
        Health health = GetComponent<Health>();
        health.HealthPoints = health.MaxHealthPoints;
        health.isDead = false;
        nav.enabled = true;
        AttackType.enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void OnObjectDisable()
    {
        nav.enabled = false;
        AttackType.enabled = false;
        AttackType.StopAllCoroutines();
    }
}
