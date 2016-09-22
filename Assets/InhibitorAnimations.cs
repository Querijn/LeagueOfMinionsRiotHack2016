using UnityEngine;
using System.Collections;

public class InhibitorAnimations : MonoBehaviour
{
    private float m_Angle = 0.0f;
    private GameObject m_Tower = null;
    private Vector3 m_BasePosition;


    void Start ()
    {
        m_Tower = transform.FindChild("Root/Tower").gameObject;
        m_BasePosition = m_Tower.transform.position;
	}

	void Update ()
    {
        m_Angle += Time.deltaTime * 35.0f;
        m_Tower.transform.localRotation = Quaternion.AngleAxis(m_Angle, Vector3.up);

        m_Tower.transform.position = m_BasePosition + Vector3.up * Mathf.Sin(m_Angle * 0.03f) * 0.133f;
    }
}
