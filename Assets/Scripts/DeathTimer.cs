using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    public float m_countdown;
    public float m_bombRadius;
    protected float m_deathTime;

    // Use this for initialization
    void Start()
    {
        m_deathTime = Time.time + m_countdown;
    }

    // Update is called once per frame
    void Update()
    {
        TextMesh text = gameObject.GetComponent<TextMesh>();
        if (text)
            text.text = ((int)(m_deathTime - Time.time)).ToString();

        if (Time.time > m_deathTime && transform.parent)
        {
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, m_bombRadius);
            foreach (Collider thing in hitColliders)
            {
                if (thing.tag == "Minion")
                    thing.SendMessage("Die");
            }
            Destroy(gameObject);
        }
    }
}
