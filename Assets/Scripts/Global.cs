using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {

    //Record which game object is currently selected.
    GameObject selected_object;

    public GameObject snare;
    public GameObject shield;

    public float maxMana;
    public float manaPerSec;
    public float manaRegenRate;
    protected float currentMana;

    void AddMana ()
    {
        currentMana += manaPerSec / 5.0f;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }

    // Use this for initialization
    void Start () {
        InvokeRepeating("AddMana", 1 / 5.0f, 1 / 5.0f);
    }

	// Update is called once per frame
	void Update ()
    {
        
        
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

        if (selected_object)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Snare has_snare = selected_object.GetComponent<Snare>();
                if (!has_snare)
                    GameObject.Instantiate(snare, selected_object.transform.position, Quaternion.identity, selected_object.transform);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                Shield has_shield = selected_object.GetComponent<Shield>();
                if (!has_shield)
                    GameObject.Instantiate(shield, selected_object.transform.position, Quaternion.identity, selected_object.transform);
            }
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                Destroy(selected_object);
            }
        }




        // Moving the Camera
        Vector2 mouseEdge = MouseScreenEdge(20);

        if (!(Mathf.Approximately(mouseEdge.x, 0f))) 
        {
            //Move your camera depending on the sign of mouse.Edge.x

            if (mouseEdge.x < 0)
            {
                transform.Translate(new Vector3(-10 * Time.deltaTime, 0, 0));
            }
            else
            {
                transform.Translate(new Vector3(10 * Time.deltaTime, 0, 0));    
            }
        }

    }

    // http://answers.unity3d.com/questions/425712/how-can-i-move-the-camera-when-the-mouse-reaches-t.html
    Vector2 MouseScreenEdge(int margin)
    {
        //Margin is calculated in px from the edge of the screen

        Vector2 half = new Vector2(Screen.width / 2, Screen.height / 2);

        //If mouse is dead center, (x,y) would be (0,0)
        float x = Input.mousePosition.x - half.x;
        float y = Input.mousePosition.y - half.y;

        //If x is not within the edge margin, then x is 0;
        //In another word, not close to the edge
        if (Mathf.Abs(x) > half.x - margin)
        {
            if (x <= 0)
                x += margin - half.x;
            else
                x += half.x - margin;
        }
        else
        {
            x = 0f;
        }

        if (Mathf.Abs(y) > half.y - margin)
        {
            if (y <= 0)
                y += margin - half.y;
            else
                y += half.y - margin;
        }
        else
        {
            y = 0f;
        }

        return new Vector2(x, y);
    }
}
