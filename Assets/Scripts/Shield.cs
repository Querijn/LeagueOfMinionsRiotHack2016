using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    public float m_shieldHealth;


    // Use this for initialization
    
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateHealth(float variation)
    {
        m_shieldHealth += variation;
        if (m_shieldHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    
}
