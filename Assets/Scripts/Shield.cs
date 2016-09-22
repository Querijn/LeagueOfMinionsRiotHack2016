using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    public float m_health;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateHealth(float variation)
    {
        m_health += variation;
        if (m_health <= 0)
        {
            Destroy(this);
        }
    }
}
