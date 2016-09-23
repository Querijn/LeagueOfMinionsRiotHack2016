using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlashThing : MonoBehaviour {
    Text textObj;
    protected float m_animationTargetAlpha;
    public float m_animationDuration;
    protected float m_endAnimation;
    protected bool m_isAnimating;

	// Use this for initialization
	void Start () {
        textObj = GetComponent<Text>();
        m_animationTargetAlpha = 1.0f;
        m_isAnimating = false;


    }
	
	// Update is called once per frame
	void Update () {
       
        textObj.CrossFadeColor(new Color(255, 255, 255, m_animationTargetAlpha), m_animationDuration, false, true);

        if (!m_isAnimating)
        {
            m_isAnimating = true;
            m_endAnimation = Time.time + m_animationDuration;
        }

        if (Time.time > m_endAnimation) 
        {
            m_isAnimating = false;
        }

        if (!m_isAnimating && textObj.color.a <= 0.01)
        {
            m_animationTargetAlpha = 1.0f;
        }
        else if (!m_isAnimating && textObj.color.a >= 0.99)
        {
            m_animationTargetAlpha = 0.0f;
        }

    }
}
