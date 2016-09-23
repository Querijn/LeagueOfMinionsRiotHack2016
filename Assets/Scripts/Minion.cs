using UnityEngine;
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

    void Start()
    {
        m_Animator = GetComponent<CustomAnimation>();
        m_TeamIndication = GetComponent<Team>();
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

        Vector3 walkSpeed = m_TeamIndication.WalkingDirection;
        if(walkSpeed.sqrMagnitude != 0)
            transform.forward = walkSpeed;

        walkSpeed *= 5;

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
        FindObjectOfType<Camera>().GetComponent<SoundManager>().PlaySound("minion_attack_1");
        m_Action = Action.Dying;
        m_Animator.PlayAnimation("minion_melee_death3", true, true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

        //Get the corresponding spawner
        if (name.ToLower().Contains("blue"))
            GameObject.Find("Blue Spawner").GetComponent<MinionSpawner>().CurrentMinionCount--;
        else if (name.ToLower().Contains("red"))
            GameObject.Find("Red Spawner").GetComponent<MinionSpawner>().CurrentMinionCount--;

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
            if(a_Collider.tag == "Minion")
            {
                Vector3 t_Diff = a_Collider.transform.position - transform.position;
                t_Diff.y = 0.0f;
                if (t_Diff.sqrMagnitude < 0.5f * 0.5f)
                {
                    t_Diff.Normalize();
                    transform.position -= t_Diff * 0.5f;
                    a_Collider.transform.position += t_Diff * 0.5f;
                }
            }

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
            t_Diff.y = 0.0f;
            if (t_Diff.sqrMagnitude < 0.5f * 0.5f)
            {
                t_Diff.Normalize();
                transform.position -= t_Diff * 0.1f;
                a_Collider.transform.position += t_Diff * 0.1f;
            }
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
