using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] int MaxHealthPoints;
    [SerializeField] int HealthPoints;
    float sinkSpeed = 0.5f;

    AudioSource Audio;
    ParticleSystem hitParticles;
    Animator animator;

    private bool isSinking= false;

    public delegate void DeathHandler();
    public event DeathHandler OnDeath;


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
            transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime);
        }

    }

    public void GetDamaged(int damage, Vector3 hitPoint)
    {
        Audio.Play();
        HealthPoints -= damage;

        hitParticles.transform.position = hitPoint;
        hitParticles.Play();


        if (HealthPoints <= 0)
        {
            Die();
        }
    }


    public void StartSinking()
    {
        isSinking = true;
        Destroy(gameObject, 2f);
    }

    private void Die()
    {
        animator.SetTrigger("Dead");
        GetComponent<CapsuleCollider>().enabled = false;
        if(OnDeath != null)
            OnDeath();

    }
}
