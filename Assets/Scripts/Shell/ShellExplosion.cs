﻿using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
        //Loop through all the colliders
        for (int i = 0; i < colliders.Length; i++)
        {
            //Find the rigidbody
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            //If they do not have a rigidbody, ignore this collision
            if (!targetRigidbody) continue;

            //Add the explosion force
            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            //Find the health script on the object
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            //if there is no health, ignore the collision
            if (!targetHealth) continue;

            //Calculate the amount of damage the target should take
            float damage = CalculateDamage(targetRigidbody.position);

            //Deal the damage to the tank
            targetHealth.TakeDamage(damage);

        }

        //unparent the the particles from the shell
        m_ExplosionParticles.transform.parent = null;

        //Play the particle system
        m_ExplosionParticles.Play();

        //Play the explosion Audio
        m_ExplosionAudio.Play();

        //Once the particles have finished, set them to be destroyed
        ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
        Destroy(m_ExplosionParticles.gameObject, mainModule.duration);

        //Destroy the shell
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        // Create a Vector from the shell to the target
        Vector3 explosionToTarget = targetPosition - transform.position;

        //Calculate the distance from the shell to the target
        float explosionDistance = explosionToTarget.magnitude;

        //Calculate the proportion of the maximum distant the target is away
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        //Calculate the damage as this proportion of the maximum possible damage.
        float damage = relativeDistance * m_MaxDamage;

        damage = Mathf.Max(0.0f, damage);
        return damage;
            
    }
}