using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

    public float m_firerate;

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
        
        if (collider && collider.tag == "Minion" && m_isShooting == false)
        {
            Shoot(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        Collider collider = other.GetComponent<Collider>();
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

        if (collider.tag == "Minion" && m_isShooting == false)
        {
            Shoot(other.gameObject);
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
        if (m_isShooting == false)
        {
            m_isShooting = true;
            m_nextFire = Time.time + m_firerate;
            TowerProjectile projectile = (TowerProjectile)Instantiate(m_projectile, transform.position, Quaternion.identity);
            projectile.SetTarget(target);
            m_latestTarget = target;

            Debug.Log("Spawning a projectile, target: '" + target.name + "'");
        }
    }
}
