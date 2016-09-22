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

    public float m_maximum_falling_distance;
    private bool m_marked_to_die;

    private Action m_Action;

    private Vector3 m_Velocity = Vector3.zero;
    private Vector3 m_WalkDirection = Vector3.right;

    private GameObject m_DiggingBlock = null;
    private bool m_Digging = false;
    private bool m_Falling = true;

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
        if (Physics.Raycast(t_Ray, out t_Hit, LayerMask.NameToLayer("Walls")))
        {
            float t_Distance = Mathf.Abs((t_Hit.transform.position - transform.position).y);

            if (t_Distance >= 0.1)
            {
                if (t_Distance >= m_maximum_falling_distance)
                {
                    m_marked_to_die = true;
                }
                m_Falling = true;
            }
        }

        if (m_Falling)
            m_Velocity -= Vector3.up * 9.8f * Time.deltaTime;
        else
        {
            m_Velocity = Vector3.zero;
            if (m_marked_to_die)
                StartCoroutine(Die());
        }

        transform.position += m_Velocity * Time.deltaTime;

        switch (m_Action)
        {
            case Action.Unknown:
                // Determine what to do next
                m_Action = Action.Walking;
                break;

            case Action.Walking:
                m_Animator.PlayAnimation("minion_melee_run", false, false);
                if (m_Falling)
                    break;

                transform.position += m_WalkDirection * Time.deltaTime;
                break;

            case Action.Digging:
                if (m_Falling)
                {
                    m_Animator.PlayAnimation("minion_melee_run", false, false);
                    break;
                }

                m_Animator.PlayAnimation("minion_melee_attack5", false, false);

                if (m_Digging == false)
                {
                    m_Digging = true;
                    StartCoroutine(Dig());
                }
                break;
        }
	}

    IEnumerator Die ()
    {
        // m_Animator.PlayAnimation("minion_death", false, false);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    IEnumerator Dig()
    {
        yield return new WaitForSeconds(0.5f);

        if (m_DiggingBlock == null)
        {
            Ray t_Ray = new Ray(transform.position + Vector3.up, Vector3.down);
            RaycastHit t_Hit;
            if (Physics.Raycast(t_Ray, out t_Hit, LayerMask.NameToLayer("Walls")))
            {
                m_DiggingBlock = t_Hit.collider.gameObject;

                Vector3 t_Position = transform.position;
                t_Position.x = t_Hit.transform.position.x;
                transform.position = t_Position;
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
        if (m_Falling)
        {
            m_Falling = false;
            m_Velocity = Vector3.zero;

            // Hack to get character out of floor
            Bounds t_WallBounds = other.gameObject.GetComponent<BoxCollider>().bounds;
            Bounds t_MinionBounds = GetComponent<BoxCollider>().bounds;
            while(t_WallBounds.Intersects(t_MinionBounds))
            {
                transform.position += Vector3.up * 0.01f;
                t_MinionBounds = GetComponent<BoxCollider>().bounds;
            }
        }
        else
        {
            m_WalkDirection *= -1;
            transform.localRotation = Quaternion.AngleAxis(m_WalkDirection.x > 0.0f ? 90.0f : 270.0f, Vector3.up);
        }
    }
}
