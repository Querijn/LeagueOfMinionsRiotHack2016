using UnityEngine;
using System.Collections;

public class KarthusWallPost : MonoBehaviour
{
    float m_Rotation = 0.0f;
    Vector3 m_BasePosition;

    void Start ()
    {
        m_BasePosition = transform.position;
        m_Rotation = Random.Range(0.0f, 360.0f);
    }

	void Update ()
    {
        m_Rotation += Time.deltaTime;

        if (m_Rotation > 360.0f)
            m_Rotation -= 360.0f;

        transform.localRotation = Quaternion.AngleAxis(m_Rotation * 50.0f, Vector3.up);

        transform.position = m_BasePosition + Vector3.up * Mathf.Sin(m_Rotation) * 0.133f;
	}
}
