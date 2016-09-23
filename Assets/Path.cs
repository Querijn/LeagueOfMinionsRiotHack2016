using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour
{
    public GameObject[] m_Targets;
    public int CurrentTarget {  get; private set; }

    static private Path m_Singleton;

    public void Start()
    {
        m_Singleton = this;
        if(m_Targets.Length == 0)
        {
            CurrentTarget = -1;
            Debug.LogError("No path nodes in the path!");
            return;
        }

        CurrentTarget = 0;
    }

    public void Update()
    {
        
    }

    static public Vector3 GetWalkDirection(Transform a_Minion)
    {
        if (m_Singleton == null || m_Singleton.CurrentTarget < 0 || m_Singleton.CurrentTarget >= m_Singleton.m_Targets.Length)
            return Vector3.zero;

        if (m_Singleton.m_Targets[m_Singleton.CurrentTarget] == null)
            m_Singleton.CurrentTarget++;

        return (m_Singleton.m_Targets[m_Singleton.CurrentTarget].transform.position - a_Minion.position).normalized;
    }
}
