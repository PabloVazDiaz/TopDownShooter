using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    public override void PerformAttack(Transform target)
    {
        
        if (!isReadyToAttack || isAttacking)
            return;
        TimeLastAttack = Time.time;
        StartCoroutine(AttackSequence(target.GetComponent<Health>()));
    }

    IEnumerator AttackSequence(Health target)
    {

        isAttacking = true;
        target.GetDamaged(Damage,transform.position);
        yield return new WaitForSeconds(AttackCooldown);
        isAttacking = false;
    }
}
