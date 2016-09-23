using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviour {

    public Team.TeamType Team;
    public float totalMinions;
    public float spawnRate;
    public float minionsLeft;
    public GameObject minionClass;

    void SpawnMinion()
    {
        Instantiate(minionClass, transform.position, Quaternion.identity);

        minionsLeft -= 1;
        if (minionsLeft <= 0)
        {
            CancelInvoke("SpawnMinion");
        }
    }

    // Use this for initialization
    void Start () {
        minionsLeft = totalMinions;
        InvokeRepeating("SpawnMinion", 5, spawnRate);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
