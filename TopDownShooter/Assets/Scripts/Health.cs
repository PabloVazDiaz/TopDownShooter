using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int MaxHealthPoints;
    public int HealthPoints;
    float sinkSpeed = 0.5f;

    AudioSource Audio;
    ParticleSystem hitParticles;
    Animator animator;

    private bool isSinking= false;
    public bool isDead = false;
    public delegate void DeathHandler(Vector3 position);
    public event DeathHandler OnDeath;

    public event Action<int> onHPChanged;
    

    private void Awake()
    {
        Audio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        animator = GetComponent<Animator>();
         
    }

    void Start()
    {
        HealthPoints = MaxHealthPoints;
    }

    private void Update()
    {
        if (isSinking)
        {
            //transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime);
        }

    }

    public void GetDamaged(int damage, Vector3 hitPoint)
    {
        if (isDead)
            return;
        Audio.Play();
        HealthPoints -= damage;

        if (hitParticles != null)
        {
            hitParticles.transform.position = hitPoint;
            hitParticles.Play();
        }

        if (onHPChanged != null)
        {
            onHPChanged(HealthPoints);
        }


        if (HealthPoints <= 0)
        {
            Die();
        }
    }


    public void StartSinking()
    {
        isSinking = true;
        LeanTween.moveY(gameObject, -3f,1/ sinkSpeed).setOnComplete(x => gameObject.SetActive(false));
        //Destroy(gameObject, 2f);
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<IPooledObject>()?.OnObjectDisable();
        if(OnDeath != null)
            OnDeath(transform.position);
        

    }
}
