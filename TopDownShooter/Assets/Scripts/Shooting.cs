using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject gunPoint;
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] float bulletSpeed;
    
    [Powerable]
    public float shootCooldown;
    [Powerable]
    public float bulletRange;
    
    private float LastShootTime = 0;
    private bool isFiring = false;

    private void Update()
    {
        if (isFiring)
            Shoot();
    }

    public void Shoot()
    {
        
        if (Time.time - LastShootTime > shootCooldown)
        {
            //GameObject bullet = Instantiate(BulletPrefab, gunPoint.transform.position, gunPoint.transform.rotation);
            GameObject bullet = ObjectPooler.instance.SpawnFromPool(PoolObject.PlayerBullet, gunPoint.transform.position, gunPoint.transform.rotation);
            
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
            LastShootTime = Time.time;
            StartCoroutine(DisableBullet(bullet, bulletRange / bulletSpeed));
            //Destroy(bullet, bulletRange / bulletSpeed);
        }


    }

    public void ToogleShooting(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;
        Debug.Log("pum");
        isFiring = !isFiring;
    }


    private IEnumerator DisableBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(false);
    }

}
