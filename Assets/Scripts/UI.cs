using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text minionsText;
    protected float totalMinions;

    public Text timeText;
    public float timeLeft;

    public Text spawnKey;

    // Use this for initialization
    void Start () {
        InvokeRepeating("UpdateTime", 1, 1);
        GameObject spawner = GameObject.Find("Blue Spawner");
        if (spawner)
            totalMinions = spawner.GetComponent<MinionSpawner>().MinionLimit;
        else
            totalMinions = 20;
    }
	
	// Update is called once per frame
	void Update () {
        if (spawnKey)
            spawnKey.text = Global.spawnKey.ToString();

        if (minionsText != null)
            minionsText.text = Global.blueMinionsCount+ " / " + totalMinions;

        int minutesLeft = (int)(timeLeft / 60.0f);
        int secondsLeft = (int)(timeLeft % 60);

        timeText.text = "";

        if (minutesLeft < 10)
            timeText.text += "0";
        timeText.text += minutesLeft + ":";

        if (secondsLeft < 10)
            timeText.text += "0";
        timeText.text += secondsLeft;
        
        if (timeLeft > 0)
        {
            
        }
    }

    void UpdateTime ()
    {
        timeLeft -= 1.0f;
        timeLeft = Mathf.Clamp(timeLeft, 0, float.PositiveInfinity);
    }
}
