using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject gunPoint;
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] float bulletSpeed;
    [SerializeField] float shootCooldown;
    [SerializeField] float bulletRange;

    private float LastShootTime = 0;


    public void Shoot()
    { 
        if (Time.time - LastShootTime > shootCooldown)
        {
            GameObject bullet = Instantiate(BulletPrefab, gunPoint.transform.position, gunPoint.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
            LastShootTime = Time.time;
            Destroy(bullet, bulletRange / bulletSpeed);
        }


    }


}
