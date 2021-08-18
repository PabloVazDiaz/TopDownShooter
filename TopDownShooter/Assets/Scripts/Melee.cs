using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{

    [SerializeField] float meleeCooldown;
    [SerializeField] int meleeDamage;
    [SerializeField] float meleePushForce;
    [SerializeField] GameObject meleeObject;

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
        StartCoroutine(MeleeAnimation());
        //meleeObject.transform.RotateAround(transform.position, 20f);
        //physics cone
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, 1);
        //damage and push
        foreach (Collider collider in hitEnemies)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                collider.GetComponent<Health>().GetDamaged(meleeDamage, collider.ClosestPoint(transform.position));
                collider.GetComponent<Rigidbody>().AddForce(transform.forward * meleePushForce, ForceMode.Impulse);
            }
        }
    }

    private IEnumerator MeleeAnimation()
    {
        meleeTrail.enabled = true;
        yield return new WaitForEndOfFrame();
        meleeObject.transform.Rotate(Vector3.up, 45);
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
