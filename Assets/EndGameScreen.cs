using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject statusStr = GameObject.Find("status");
        GameObject msv = GameObject.Find("msv");
        GameObject mkv = GameObject.Find("mkv");
        GameObject timeStr = GameObject.Find("timeStr");
        if (PlayerPrefs.GetFloat("Won Game") >= 1f)
            statusStr.GetComponent<Text>().text = "Victory!";
        else
            statusStr.GetComponent<Text>().text = "Defeat";

        msv.GetComponent<Text>().text = PlayerPrefs.GetFloat("Blue Minions Spawned").ToString();
        mkv.GetComponent<Text>().text = PlayerPrefs.GetFloat("Red Minions Killed").ToString();

        float timeTaken = PlayerPrefs.GetFloat("Total Time Taken");
        string timeTakenStr = "";

        int minutesLeft = (int)(timeTaken / 60.0f);
        int secondsLeft = (int)(timeTaken % 60);

        if (minutesLeft < 10)
            timeTakenStr += "0";
        timeTakenStr += minutesLeft + ":";

        if (secondsLeft < 10)
            timeTakenStr += "0";
        timeTakenStr += secondsLeft;

        timeStr.GetComponent<Text>().text = timeTakenStr;



    }
	
	// Update is called once per frame
	void Update () {
	
	}


}
