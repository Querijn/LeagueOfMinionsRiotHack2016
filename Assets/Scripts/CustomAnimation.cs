using UnityEngine;
using System.Collections;
using System;

public class CustomAnimation : MonoBehaviour
{
    private String m_AnimationPlaying;
    private bool m_AnimationBlocking = false;

    void Start ()
    {
	}

	void Update ()
    {
        if (m_AnimationBlocking)
            m_AnimationBlocking = GetComponent<Animation>().IsPlaying(m_AnimationPlaying);
    }


    public void PlayAnimation(String a_Animation, bool a_Block, bool a_Forced)
    {
        if (a_Forced || !m_AnimationBlocking)
        {
            m_AnimationPlaying = a_Animation;
            GetComponent<Animation>().CrossFade(a_Animation);
        }

        if (a_Block && (!m_AnimationBlocking || !GetComponent<Animation>().IsPlaying(m_AnimationPlaying)))
        {
            m_AnimationBlocking = true;
        }
    }
}
