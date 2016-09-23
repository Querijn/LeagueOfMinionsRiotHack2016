using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public struct Clip
    {
        public string name;
        public AudioClip clip;
    }
    public Clip[] AudioClips;


    public void PlaySound(string a_Name)
    {
        GameObject t_GameObject = new GameObject();
        t_GameObject.transform.parent = transform;
        AudioSource t_AudioSource = t_GameObject.AddComponent<AudioSource>();

        for (int i = 0; i < AudioClips.Length; i++)
        {
            if (AudioClips[i].name == a_Name)
            {
                t_AudioSource.clip = AudioClips[i].clip;
            }
        }

        //Destroy the sound-playing gameObject once it's finished playing.
        if (t_AudioSource)
            Destroy(t_GameObject, t_AudioSource.clip.length);

        t_AudioSource.Play();
    }



	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}
}
