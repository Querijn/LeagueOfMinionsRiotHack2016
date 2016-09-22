using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour
{
    public enum Action
    {
        Unknown,
        Walking,
        Digging
    };

    public float m_health;
    public float m_maximum_falling_distance;
    private bool m_marked_to_die;

    private Action m_Action;

    private Vector3 m_Velocity = Vector3.zero;
    private Vector3 m_WalkDirection = Vector3.right;

    private GameObject m_DiggingBlock = null;
    private bool m_Digging = false;

    private CustomAnimation m_Animator;

	void Start ()
    {
        m_marked_to_die = false;
        m_Animator = GetComponent<CustomAnimation>();
        transform.localRotation = Quaternion.AngleAxis(m_WalkDirection.x > 0.0f ? 90.0f : 270.0f, Vector3.up);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_Action = Action.Digging;
        }

        Ray t_Ray = new Ray(transform.position + Vector3.up, Vector3.down);
        RaycastHit t_Hit;
        bool m_Falling = true;
        if (Physics.Raycast(t_Ray, out t_Hit))
        {
            float t_Distance = (t_Hit.transform.position - transform.position).magnitude;

            if (t_Distance <= 0.66)
            {
                m_Falling = false;
            }
            else if (t_Distance >= m_maximum_falling_distance)
            { 
                m_marked_to_die = true;
            }
            //else Debug.Log("Falling " + t_Distance);
        }

        if (m_Falling)
            m_Velocity -= Vector3.up * 9.8f * Time.deltaTime;
        else
        {
            m_Velocity = Vector3.zero;
            if (m_marked_to_die)
                Die();
        }

        transform.position += m_Velocity * Time.deltaTime;

        switch (m_Action)
        {
            case Action.Unknown:
                // Determine what to do next
                m_Action = Action.Walking;
                break;

            case Action.Walking:

                transform.position += m_WalkDirection * Time.deltaTime;
                m_Animator.PlayAnimation("minion_run", false, false);

                break;

            case Action.Digging:
                m_Animator.PlayAnimation("minion_attack1", false, false);

                if (m_Digging == false)
                {
                    m_Digging = true;
                    StartCoroutine(Dig());
                }
                break;
        }
	}

    void Die ()
    {
        Destroy(gameObject);
    }

    IEnumerator Dig()
    {
        yield return new WaitForSeconds(0.5f);

        if (m_DiggingBlock == null)
        {
            Ray t_Ray = new Ray(transform.position + Vector3.up, Vector3.down);
            RaycastHit t_Hit;
            if (Physics.Raycast(t_Ray, out t_Hit))
            {
                m_DiggingBlock = t_Hit.collider.gameObject;
            }
        }
        
        if (m_DiggingBlock != null && m_DiggingBlock.transform.localScale.y > 0.1f)
        {
            m_DiggingBlock.transform.localScale -= new Vector3(0, 0.1f, 0);
            m_DiggingBlock.transform.localPosition -= new Vector3(0, 0.05f, 0);
        }
        else
        {
            Destroy(m_DiggingBlock);
            m_DiggingBlock = null;
        }
        m_Digging = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Collider collider = other.GetComponent<Collider>();
        if (collider == null)
            return;

        if (collider.tag == "Tower Projectile")
        {
            if (collider.gameObject.GetComponent<TowerProjectile>().IsTarget(gameObject))
            {
                Destroy(collider.gameObject);
                m_health -= 1;
                if (m_health <= 0)
                    Die();
            }
        }
        else if (collider.tag == "Tower" || collider.tag == "Minion")
        {}
        else
        {
            m_WalkDirection *= -1;
            transform.localRotation = Quaternion.AngleAxis(m_WalkDirection.x > 0.0f ? 90.0f : 270.0f, Vector3.up);
        }
    }

    public float GetHeath()
    {
        return m_health;
    }

    public bool IsDead()
    {
        return (GetHeath() <= 0.0f);
    }

}
