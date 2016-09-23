using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text minionsText;
    protected float totalMinions;

    public Text timeText;
    public float timeLeft;

    // Use this for initialization
    void Start () {
        InvokeRepeating("UpdateTime", 1, 1);
        GameObject spawner = GameObject.Find("MinionSpawner");
        if (spawner)
            totalMinions = spawner.GetComponent<MinionSpawner>().totalMinions;
        else
            totalMinions = 99;
    }
	
	// Update is called once per frame
	void Update () {
        if (minionsText != null)
            minionsText.text = GameObject.FindGameObjectsWithTag("Minion").Length + " / " + totalMinions;

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
