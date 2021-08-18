using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] int Damage;
    [SerializeField] float AttackRange;
    [SerializeField] float AttackCooldown;

    EnemyAttack AttackType;

    Transform player;
    NavMeshAgent nav;
    bool seekingPlayer = true;
    bool attackingPlayer = false;



    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        GetComponent<Health>().OnDeath += DisableNavMesh;
        AttackType = GetComponent<EnemyAttack>();
    }

    void DisableNavMesh()
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
            DisableNavMesh();
            attackingPlayer = true;
        }
        else
        {
            attackingPlayer = false;
        }
    }


    private void AttackBehaviour()
    {
        AttackType.PerformAttack(player);
        seekingPlayer = true;
    }


}
