using UnityEngine;
using System.Collections;

public class FixedBackgroundImage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 t_CameraPosition = Camera.main.transform.position;
        t_CameraPosition.Scale(new Vector3(1, 1, 0));

        transform.position = t_CameraPosition + Vector3.forward * 40;
	}
}
