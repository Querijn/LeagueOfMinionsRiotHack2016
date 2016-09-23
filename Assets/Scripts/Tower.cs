using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

    public float m_firerate;
    public float m_health;
    public TowerProjectile m_projectile;

    protected bool m_isShooting = false;
    protected float m_nextFire = 0;

    protected GameObject m_latestTarget;

    // Use this for initialization
    void Start () {
	
	}
	
    void OnTriggerEnter(Collider other)
    {
        Collider collider = other.GetComponent<Collider>();
        
        if (collider && collider.tag == "Minion" && other.GetComponent<Team>().m_Team != GetComponent<Team>().m_Team && m_isShooting == false)
        {
            Shoot(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        Collider collider = other.GetComponent<Collider>();
        Team my_team = GetComponent<Team>();

        if (collider == null)
        {
            return;
        }

        // Making it separated, so towers may shoot anything
        if (m_latestTarget != null)
        {
            Minion targetMinion = m_latestTarget.GetComponent<Minion>();
            if (targetMinion && !targetMinion.IsDead())
                Shoot(m_latestTarget.gameObject);
        }

        if (collider.tag == "Minion" && collider.GetComponent<Team>().m_Team != my_team.m_Team && m_isShooting == false)
        {
            Shoot(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Collider collider = other.GetComponent<Collider>();
        if (collider == null)
        {
            return;
        }
        if (collider.gameObject == m_latestTarget)
        {
            m_latestTarget = null;
        }
    }

    // Update is called once per frame
    void Update ()
    {
	    if (Time.time >= m_nextFire)
        {
            m_isShooting = false;
        }
	}

    void Shoot(GameObject target)
    {
        if (target == null || target.GetComponent<Minion>().IsDead())
            return;

        if (m_isShooting == false)
        {
            m_isShooting = true;
            m_nextFire = Time.time + m_firerate;

            Vector3 projectile_position = transform.FindChild("SRUAP_OrderTurret1_Idle1").FindChild("Buffbone_Glb_Channel_Loc").position;

            TowerProjectile projectile = (TowerProjectile)Instantiate(m_projectile, projectile_position, Quaternion.identity);
            projectile.transform.forward = target.transform.position - projectile.transform.position;
            projectile.SetTarget(target);
            m_latestTarget = target;

            FindObjectOfType<Camera>().GetComponent<SoundManager>().PlaySound("tower_shot_1");

            //Debug.Log("Spawning a projectile, target: '" + target.name + "'");
        }
    }

    public void UpdateHealth(float variation)
    {
        m_health += variation;
        if (m_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool IsDead()
    {
        return (m_health <= 0.0f);
    }
}
