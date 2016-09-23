using UnityEngine;
using System.Collections;

public class RedTeamAI : MonoBehaviour {

    static MinionSpawner m_spawner;
    protected float m_nextSpawn;
    public float m_maximumMinions;

	// Use this for initialization
	void Start ()
    {
        m_spawner = gameObject.GetComponentInChildren<MinionSpawner>();
        m_nextSpawn = Time.time + Random.Range(2f, 5f);

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Time.time >= m_nextSpawn && Global.redMinionsCount < m_maximumMinions)
        {
            SpawnMinion();
            m_nextSpawn = Time.time + .1f;
        }
	}

    public static void SpawnMinion()
    {
        m_spawner.Spawn();
    }
}
