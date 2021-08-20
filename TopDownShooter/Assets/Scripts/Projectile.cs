using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [SerializeField] int damage = 10;
    [SerializeField] string TargetTag;

    public void OnObjectDisable()
    {
        gameObject.SetActive(false);
    }

    public void OnObjectSpawn()
    {
        gameObject.SetActive(false);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(TargetTag))
        {
            other.GetComponent<Health>().GetDamaged(damage, transform.position);
            gameObject.SetActive(false);
        }
    }
}
