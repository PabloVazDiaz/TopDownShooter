using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{

    [SerializeField] float meleeCooldown;
    [SerializeField] float meleePushForce;
    [SerializeField] GameObject meleeObject;

    [Powerable]
    public float meleeDamage;
    public float meleeAttackAngle = 60f;

    private TrailRenderer meleeTrail;
    private float timeLastAttack;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        meleeTrail = meleeObject.GetComponentInChildren<TrailRenderer>();
    }

    public void MeleeAttack()
    {
        //do melee animation
        if (Time.time - timeLastAttack < meleeCooldown)
            return;
        timeLastAttack = Time.time;
        MeleeVisual();
        //StartCoroutine(MeleeAnimation());
        //meleeObject.transform.RotateAround(transform.position, 20f);
        //physics cone
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, 1.5f);
        //damage and push
        foreach (Collider collider in hitEnemies)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                collider.GetComponent<Health>().GetDamaged((int)meleeDamage, collider.ClosestPoint(transform.position));
                collider.GetComponent<Rigidbody>().AddForce(transform.forward * meleePushForce, ForceMode.Impulse);
            }
        }
    }

    private void MeleeVisual()
    {
        meleeTrail.enabled = true;
        LeanTween.rotateAround(meleeObject, Vector3.up, meleeAttackAngle, 0.05f).setOnComplete(x => meleeTrail.enabled = false);
        LeanTween.rotateAround(meleeObject, Vector3.up, -meleeAttackAngle, 0f);


    }

    private IEnumerator MeleeAnimation()
    {
        meleeTrail.enabled = true;
        yield return new WaitForEndOfFrame();
        meleeObject.transform.Rotate(Vector3.up, meleeAttackAngle);
        yield return new WaitForEndOfFrame();
        meleeTrail.enabled = false;
        //yield return new WaitForSeconds(0.5f);

        meleeObject.transform.Rotate(Vector3.up, -45);

    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position + transform.forward, 1);
    }
}
