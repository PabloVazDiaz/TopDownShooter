using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] string TargetTag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(TargetTag))
        {
            other.GetComponent<Health>().GetDamaged(damage, transform.position);
            Destroy(gameObject);
        }
    }
}
