using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {

    //Record which game object is currently selected.
    GameObject selected_object;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //Detect a mousedown on a minion object.
        //TODO: Filter to only work on minion gameObjects?
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform.gameObject.name == "Minion")
            {
                selected_object = hit.transform.gameObject;           
            }
        }

        //Right-Click to deselect
        if (Input.GetMouseButtonDown(1))
        {
            selected_object = null;
            GameObject.Find("Sphere").transform.position = new Vector3(500, 500, 500); //Offscreen
        }

        //Draw a selection indicator (red dot?) above the game object.
        if(selected_object != null)
        {
            Vector3 position = selected_object.transform.position;
            position.y += 1; //Move up above minion.
            GameObject.Find("Sphere").transform.position = position;
        }

	}
}
