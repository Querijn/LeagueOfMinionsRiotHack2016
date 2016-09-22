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
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f) && hit.transform.gameObject.tag == "Minion")
            {

                //Set this object as the selected object
                selected_object = hit.transform.gameObject;

                //Set the shader
                Renderer renderer = selected_object.GetComponentInChildren<Renderer>();
                renderer.material.shader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
                renderer.material.SetColor("_Color", Color.white);
                renderer.material.SetColor("_OutlineColor", Color.green);
                renderer.material.SetFloat("_Outline", 0.2f);
              
            }
        }

        //Right-Click to deselect
        if (Input.GetMouseButtonDown(1))
        {
            if (selected_object)
            {
                //Set normal shader on gameObject's renderer
                selected_object.GetComponentInChildren<Renderer>().material.shader = Shader.Find("Unlit/Texture");
                selected_object = null;
            }
        }

	}
}
