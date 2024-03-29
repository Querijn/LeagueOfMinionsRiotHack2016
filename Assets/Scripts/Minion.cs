﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private bool m_Falling = true;

    private CustomAnimation m_Animator;

    protected float m_speedModifier;
    protected float m_hasSpeedModifier;

    // Probably there is a better way to do this
    protected float m_attackNext;
    protected float m_nextAttackDamage;
    protected float m_attackSpeed = 0.933f; // The same lenght as the attack animation
    protected GameObject m_attackTowerTarget;

    private Team m_TeamIndication = null;

    static List<Minion> m_TeamMinions = new List<Minion>();

    void Start()
    {
        m_Animator = GetComponent<CustomAnimation>();
        m_TeamIndication = GetComponent<Team>();

        if (m_TeamIndication.m_Team == Team.TeamType.Blue)
            m_TeamMinions.Add(this);
    }

    public static Minion Front
    {
        get
        {
            Minion t_Return = null;
            float t_X = -9999.0f;
            for (int i = 0; i < m_TeamMinions.Count; i++)
            {
                if (m_TeamMinions[i] == null)
                    continue;

                if (m_TeamMinions[i].transform.position.x > t_X)
                {
                    t_X = m_TeamMinions[i].transform.position.x;
                    t_Return = m_TeamMinions[i];
                }
            }
            return t_Return;
        }
    }

    public static Vector3 Average
    {
        get
        {
            Vector3 t_Average = Vector3.zero;
            for (int i = 0; i < m_TeamMinions.Count; i++)
            {
                if (m_TeamMinions[i] == null)
                    continue;

                t_Average += m_TeamMinions[i].transform.position; 
            }

            return t_Average / m_TeamMinions.Count;
        }
    }

    void Update()
    {
        m_TeamMinions.RemoveAll(m => m == null);

        transform.position += m_Velocity * Time.deltaTime;

        Vector3 walkSpeed = m_TeamIndication.WalkingDirection;
        if(walkSpeed.sqrMagnitude > 0.01f)
        {
            transform.forward = walkSpeed;

            walkSpeed *= 4;

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

        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
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
        FindObjectOfType<Camera>().GetComponent<SoundManager>().PlaySound("minion_attack_1");
        m_Action = Action.Dying;
        m_Animator.PlayAnimation("minion_melee_death3", true, true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        Team t_MyTeam = GetComponent<Team>();
        if (t_MyTeam)
        {
            if (t_MyTeam.m_Team == Team.TeamType.Blue)
                Global.blueMinionsDead++;
            else
                Global.redMinionsDead++;
        }

    }

    void OnTriggerEnter(Collider a_Collider)
    {
        if (a_Collider.tag == "Tower Projectile")
        {
            if (a_Collider.gameObject.GetComponent<TowerProjectile>().IsTarget(gameObject))
            {
                Shield has_shield = gameObject.GetComponentInChildren<Shield>();
                if (has_shield)
                { 
                    has_shield.UpdateHealth(-1);
                }
                else
                {
                    UpdateHealth(-1);
                }
                Destroy(a_Collider.gameObject);
            }
        }
        else if (a_Collider.tag == "Minion" || a_Collider.tag == "Tower")
        {
            Team t_TowerTeam = a_Collider.gameObject.GetComponent<Team>();
            Team t_MyTeam = GetComponent<Team>();
            if (t_TowerTeam == null)
                t_TowerTeam = a_Collider.transform.parent.GetComponent<Team>();

            if (t_TowerTeam && t_MyTeam && t_TowerTeam.m_Team != t_MyTeam.m_Team)
            {
                m_Action = Action.Atacking;
                m_attackNext = Time.time + m_attackSpeed;
                m_attackTowerTarget = a_Collider.gameObject;
            }
        }
    }

    void OnTriggerStay(Collider a_Collider)
    {
        if (a_Collider.tag == "Minion")
        {
            Vector3 t_Diff = a_Collider.transform.position - transform.position;
           
            if (t_Diff.sqrMagnitude < 0.5f * 0.5f)
            {
                t_Diff.y = 0.0f;
                transform.position -= t_Diff * 0.05f;
                a_Collider.transform.position += t_Diff * 0.05f;
            }
        }
    }

    IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(0.6f);

        m_Action = Action.Walking;
    }

    void OnTriggerExit(Collider a_Collider)
    {
        if ((a_Collider.tag == "Minion" || a_Collider.tag == "Tower") && m_Action == Action.Atacking)
            StartCoroutine(StopAttacking());
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
