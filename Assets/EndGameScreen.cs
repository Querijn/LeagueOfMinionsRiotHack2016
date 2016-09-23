using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject statusStr = GameObject.Find("status");
        GameObject msv = GameObject.Find("msv");
        GameObject mkv = GameObject.Find("mkv");
        if (PlayerPrefs.GetFloat("Won Game") >= 1f)
            statusStr.GetComponent<Text>().text = "Victory!";
        else
            statusStr.GetComponent<Text>().text = "Defeat";

        msv.GetComponent<Text>().text = PlayerPrefs.GetFloat("Blue Minions Spawned").ToString();
        mkv.GetComponent<Text>().text = PlayerPrefs.GetFloat("Red Minions Killed").ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


}
