using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlashThing : MonoBehaviour {
    Text textObj;

	// Use this for initialization
	void Start () {
        textObj = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        textObj.color = new Color(255, 255, 255, (Mathf.Sin(Time.time * 2.0f) + 1.3f) / 2.0f);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("____GAME");
        }
    }
}
