using UnityEngine;
using System.Collections;

public class Snare : MonoBehaviour
{ 
    public float m_duration;

    protected float m_animationEnd;
    // Use this for initialization

    void Start () {
        if (transform.parent && transform.parent.GetComponentsInChildren<Snare>().Length >= 2)
        {
            Destroy(gameObject);
            return;
        }
        transform.parent.SendMessage("ChangeSpeed", new Vector2(0.0f, m_duration));
        m_animationEnd = Time.time + m_duration;
    }
	
	// Update is called once per frame
	void Update () {
	    if (Time.time > m_animationEnd)
        {
            Destroy(gameObject);
        }
	}
}
