using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviour {

    public Team.TeamType Team;
    public float totalMinions;
    public float spawnRate;
    public float minionsLeft;
    public GameObject minionClass;
    public KeyCode keyToPress;

    void SpawnMinion()
    {
        Spawn();

        minionsLeft -= 1;
        if (minionsLeft <= 0)
        {
            CancelInvoke("SpawnMinion");
        }
    }

    // Use this for initialization
    void Start ()
    {
        minionsLeft = totalMinions;
        //InvokeRepeating("SpawnMinion", 1, spawnRate);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(Global.spawnKey))
        {
            Instantiate(minionClass, transform.position, Quaternion.identity);
            Global.spawnKey = (KeyCode)((int)Random.Range(97.0f, 122.0f));
        }
	
	}

    public void Spawn ()
    {
        Instantiate(minionClass, transform.position, Quaternion.identity);
    }
}
