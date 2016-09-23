using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndMatch : MonoBehaviour {

    public float win;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider a_Collider)
    {
        PlayerPrefs.SetFloat("Red Minions Killed", Global.redMinionsDead);
        PlayerPrefs.SetFloat("Blue Minions Killed", Global.blueMinionsDead);
        PlayerPrefs.SetFloat("Blue Minions Spawned", Global.blueMinionsSpawnedTotal);
        PlayerPrefs.SetFloat("Won Game", win);

        SceneManager.LoadScene("____ENDGAME");
    }
}
