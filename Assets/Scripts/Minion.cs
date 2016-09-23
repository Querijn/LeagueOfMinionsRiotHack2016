﻿using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour
{
    public enum Action
    {
        Unknown,
        Walking,
        Atacking,
        Dying
    };

    public float m_health = 5.0f;
    public float m_GroundPlaneHeight = 0.5f;

    private Action m_Action;

    private Vector3 m_Velocity = Vector3.zero;
    private Vector3 m_WalkDirection = Vector3.right;

    private bool m_Falling = true;

    private CustomAnimation m_Animator;

    protected float m_speedModifier;
    protected float m_hasSpeedModifier;
    

    // Probably there is a better way to do this
    protected float m_attackNext;
    protected float m_nextAttackDamage;
    protected float m_attackSpeed = 0.933f; // The same lenght as the attack animation
    protected GameObject m_attackTowerTarget;

    void Start()
    {
        m_Animator = GetComponent<CustomAnimation>();
        transform.localRotation = Quaternion.AngleAxis(m_WalkDirection.x > 0.0f ? 90.0f : 270.0f, Vector3.up);
    }

    void Update()
    {
        m_Falling = (transform.position.y > m_GroundPlaneHeight);
        if (m_Falling)
            m_Velocity -= Vector3.up * 9.8f * Time.deltaTime;
        else
        {
            m_Velocity = Vector3.zero;
            Vector3 t_Position = transform.position;
            if (t_Position.y < m_GroundPlaneHeight)
                t_Position.y = m_GroundPlaneHeight;
            transform.position = t_Position;
        }

        transform.position += m_Velocity * Time.deltaTime;

        Vector3 walkSpeed = m_WalkDirection;
        if (m_hasSpeedModifier >= Time.time)
        {
            walkSpeed *= m_speedModifier;
        }

        switch (m_Action)
        {
            case Action.Unknown:
                // Determine what to do next
                m_Action = Action.Walking;
                break;

            case Action.Walking:
                m_Animator.PlayAnimation("minion_melee_run", false, false);

                transform.position += walkSpeed * Time.deltaTime;
                break;

            case Action.Atacking:
                m_Animator.PlayAnimation("minion_melee_attack5", false, false);
                break;

            case Action.Dying:
                break;
        }
    }

    void FixedUpdate()
    {
        if (m_attackTowerTarget)
        {
            if (Time.time > m_attackNext)
            {
                AttackTarget(m_attackTowerTarget);
            }

            TurretBody turret = m_attackTowerTarget.GetComponent<TurretBody>();

            if (turret && turret.IsDead())
            {
                m_attackTowerTarget = null;
                m_Action = Action.Walking;
            }
        }
        else
        {
            m_Action = Action.Walking;
        }
    }

    IEnumerator Die()
    {
        m_Action = Action.Dying;
        m_Animator.PlayAnimation("minion_melee_death3", true, true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider a_Collider)
    {
        if (a_Collider.tag == "Tower Projectile")
        {
            if (a_Collider.gameObject.GetComponent<TowerProjectile>().IsTarget(gameObject))
            {
                Destroy(a_Collider.gameObject);
                UpdateHealth(-1);
            }
        }
        else if (a_Collider.tag == "Tower")
        {
            m_Action = Action.Atacking;
            m_attackNext = Time.time + m_attackSpeed;
            m_attackTowerTarget = a_Collider.gameObject;
        }
        else if (a_Collider.tag == "Minion" || a_Collider.tag == "Minion Ignorable")
        {
        }
        else if (m_Falling)
        {
            m_Falling = false;
            m_Velocity = Vector3.zero;
        }
        else
        {
            m_WalkDirection *= -1;
            transform.localRotation = Quaternion.AngleAxis(m_WalkDirection.x > 0.0f ? 90.0f : 270.0f, Vector3.up);
        }


    }
    
    void AttackTarget(GameObject target)
    {
        m_attackNext = Time.time + m_attackSpeed;
        target.SendMessage("UpdateHealth", -1.0f);
    }

    public void UpdateHealth(float variation)
    {
        m_health += variation;
        if (m_health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public bool IsDead()
    {
        return (m_Action != Action.Atacking && m_health <= 0.0f);
    }

    public void ChangeSpeed(Vector2 packed)
    {
        ChangeSpeed(packed.x, packed.y);
    }

    public void ChangeSpeed(float percentage, float duration)
    {
        m_hasSpeedModifier = Time.time + duration;
        m_speedModifier = percentage;
    }
}
