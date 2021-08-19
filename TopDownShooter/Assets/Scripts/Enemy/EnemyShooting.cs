using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShooting : EnemyAttack
{
    [SerializeField] float CastingTime;
    [SerializeField] float ProjectileSpeed;
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject ShootingPoint;
    [SerializeField] Slider CastingBar;
    public override void PerformAttack(Transform target)
    {

        if (!isReadyToAttack || isAttacking)
            return;
        TimeLastAttack = Time.time;
        StartCoroutine(AttackSequence(target.GetComponent<Health>()));
    }

    IEnumerator AttackSequence(Health target)
    {
        //Show progress
        isAttacking = true;
        while(Time.time - TimeLastAttack < CastingTime)
        {
            CastingBar.value = Time.time - TimeLastAttack / CastingTime;
            yield return new WaitForEndOfFrame();
            
            //yield return new WaitForSeconds(CastingTime);
        }
        Vector3 castingPosition = Vector3.MoveTowards(ShootingPoint.transform.position, target.transform.position, 0.5f);
        Quaternion castingRotation = Quaternion.LookRotation(target.transform.position - transform.position, transform.up);
        GameObject bullet = Instantiate(Projectile, castingPosition, castingRotation);
        bullet.GetComponent<Rigidbody>().AddForce(ProjectileSpeed * bullet.transform.forward, ForceMode.Impulse);
        CastingBar.value = 0;
        yield return new WaitForSeconds(AttackCooldown);
        isAttacking = false;
    }
}
