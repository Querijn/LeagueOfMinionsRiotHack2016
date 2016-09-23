using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Team : MonoBehaviour
{
    public enum TeamType
    {
        Red,
        Blue
    };

    public class Target
    {
        public TeamType Team;
        public GameObject Object;
    }

    static public List<Target>[] m_Targets = new List<Target>[2];

    public TeamType m_Team;

    private Target m_Target = null;

    public void Start()
    {
        Target t_Me = new Target();
        t_Me.Object = gameObject;
        t_Me.Team = m_Team;

        int t_TeamIndicator = (int)(m_Team == TeamType.Red ? TeamType.Blue : TeamType.Red);
        if(m_Targets[t_TeamIndicator] == null)
            m_Targets[t_TeamIndicator] = new List<Target>();

        m_Targets[t_TeamIndicator].Add(t_Me);
    }

    public void Update()
    {
        float t_ClosestDistance = 9999999.0f;
        m_Target = null;
        if (m_Targets[(int)m_Team] == null)
            return;

        for(int i = 0; i < m_Targets[(int)m_Team].Count; i++)
        {
            if(m_Targets[(int)m_Team][i].Object == null)
            {
                continue;
            }

            float t_Distance = (m_Targets[(int)m_Team][i].Object.transform.position - transform.position).sqrMagnitude;
            if (t_Distance < t_ClosestDistance)
            {
                t_ClosestDistance = t_Distance;
                m_Target = m_Targets[(int)m_Team][i];
            }
        }

        for(int i = 0; i < 2; i++)
            m_Targets[i].RemoveAll(x => x.Object == null);
    }
    
    public Vector3 WalkingDirection
    {
        get
        {
            if(m_Target == null || m_Target.Object == null)
                return Vector3.zero;

            return (m_Target.Object.transform.position - transform.position).normalized;
        }
    }
}
