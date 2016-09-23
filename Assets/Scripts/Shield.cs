using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    public float m_shieldHealth;

    // Use this for initialization  
    void Start ()
    {
        if(transform.parent && transform.parent.GetComponentsInChildren<Shield>().Length >= 2)
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider a_Collider)
    {
        if (a_Collider.tag == "Tower Projectile")
        {
            if (a_Collider.gameObject.GetComponent<TowerProjectile>().IsTarget(gameObject.transform.parent.gameObject))
            {
                UpdateHealth(-1);
                Destroy(a_Collider);
            }
        }
    }

    public void UpdateHealth(float variation)
    {
        m_shieldHealth += variation;
        if (m_shieldHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    
}
