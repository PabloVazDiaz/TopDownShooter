using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{


    public int Damage;
    public float AttackRange;
    public float AttackCooldown;
    public bool isReadyToAttack = true;
    public bool isAttacking = false;

    public float TimeLastAttack = 0;

    public virtual void PerformAttack(Transform target)
    {
        if (isAttacking || Time.time - TimeLastAttack <= AttackCooldown)
            return;

        TimeLastAttack = Time.time;
    }


    public bool IsTargetInRange(Transform target) 
    {
        return Vector3.Distance(target.position, transform.position) < AttackRange;
    }

    public bool IsAttackReady()
    {
        return Time.time - TimeLastAttack <= AttackCooldown;
    }
   

}
