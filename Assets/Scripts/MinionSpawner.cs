using UnityEngine;
using System.Collections;

public class MinionSpawner : MonoBehaviour {

    public Team.TeamType Team;
    public int MinionLimit;

    public float spawnRate;
    public GameObject minionClass;
    public KeyCode keyToPress;

    // Use this for initialization
    void Start (){}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Global.spawnKey) && Global.blueMinionsCount < MinionLimit)
        {
            Spawn();
            Global.spawnKey = (KeyCode)((int)Random.Range(97.0f, 122.0f));
            Global.blueMinionsSpawnedTotal++;
        }
    }

    public void Spawn ()
    {
        Instantiate(minionClass, transform.position, Quaternion.identity);
    }
}
