using UnityEngine;
using System.Collections;

public class TowerProjectile : MonoBehaviour {

    public float speed;
    protected GameObject m_currentTarget;

	void Start () {
    }

    public void SetTarget (GameObject target)
    {
        m_currentTarget = target;
    }


    public bool IsTarget (GameObject thing)
    {
        return (thing == m_currentTarget);
    }

    void Update ()
    {
	    if (m_currentTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_currentTarget.transform.position, speed * Time.deltaTime);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
