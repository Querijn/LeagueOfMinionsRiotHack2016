using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    public float m_duration;
    protected float m_deathTime;

    // Use this for initialization
    void Start()
    {
        m_deathTime = Time.time + m_duration;
        gameObject.GetComponent<TextMesh>().text = m_duration.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        TextMesh text = gameObject.GetComponent<TextMesh>();
        text.text = ((int)(m_deathTime - Time.time)).ToString();

        if (Time.time > m_deathTime && transform.parent)
        {
            transform.parent.SendMessage("Die");
            Destroy(gameObject);
        }
    }
}
