using UnityEngine;
using System.Collections;

public class RedTeamAI : MonoBehaviour {

    static MinionSpawner m_spawner;
    protected float m_nextSpawn;
    public float m_maximumMinions;

    protected bool hasUpdatedRate = false;
    protected float defaultUpdateRate = 0.2f;
	// Use this for initialization
	void Start ()
    {
        m_spawner = gameObject.GetComponentInChildren<MinionSpawner>();
        m_nextSpawn = Time.time + Random.Range(2f, 5f);

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!hasUpdatedRate)
        {
            float turrentCount = 0;
            foreach (GameObject tw in GameObject.FindGameObjectsWithTag("Tower"))
            {
                Team t_MyTeam = tw.transform.parent.GetComponent<Team>();
                if (t_MyTeam && t_MyTeam.m_Team == Team.TeamType.Red)
                    turrentCount++;
            }

            if (turrentCount <= 1)
            {
                defaultUpdateRate = 0.15f;
                m_maximumMinions += 3;
                hasUpdatedRate = true;
            }
        }



	    if (Time.time >= m_nextSpawn && Global.redMinionsCount < m_maximumMinions)
        {
            SpawnMinion();
            m_nextSpawn = Time.time + .2f;
        }
	}

    public static void SpawnMinion()
    {
        m_spawner.Spawn();
    }
}
