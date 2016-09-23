using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviour {

    public Team.TeamType Team;
    public int MinionLimit;
    public int CurrentMinionCount;

    public float spawnRate;
    public GameObject minionClass;
    public KeyCode keyToPress;

    // Use this for initialization
    void Start (){}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(Global.spawnKey) && CurrentMinionCount < MinionLimit)
        {
            CurrentMinionCount++;
            Spawn();
            Global.spawnKey = (KeyCode)((int)Random.Range(97.0f, 122.0f));
        }
	
	}

    public void Spawn ()
    {
        Instantiate(minionClass, transform.position, Quaternion.identity);
    }
}
