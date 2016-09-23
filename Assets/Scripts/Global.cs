using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {

    //Record which game object is currently selected.
    static public GameObject selected_object;
    static public KeyCode spawnKey = KeyCode.A;

    static public float redMinionsCount;
    static public float blueMinionsCount;
    static public float blueMinionsDead;
    static public float redMinionsDead;
    static public float blueMinionsSpawnedTotal;

    public GameObject snare;
    public GameObject shield;

    public float maxMana;
    public float manaPerSec;
    public float manaRegenRate;
    protected float currentMana;

    public float minimunLeftScroll;
    public float maximumLeftScroll;

    static public GameObject PendingAction;

    void AddMana ()
    {
        currentMana += manaPerSec / 5.0f;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }

    // Use this for initialization
    void Start () {
        spawnKey = (KeyCode)((int)Random.Range(97.0f, 122.0f));
        InvokeRepeating("AddMana", 1 / 5.0f, 1 / 5.0f);

        //Start playing global sound
        FindObjectOfType<Camera>().GetComponent<SoundManager>().PlaySound("game_music");

        blueMinionsDead = 0;
        redMinionsDead = 0;
        blueMinionsSpawnedTotal = 0;
    }

	// Update is called once per frame
	void Update ()
    {
        redMinionsCount = 0;
        blueMinionsCount = 0;

        foreach (GameObject element in GameObject.FindGameObjectsWithTag("Minion"))
        {
            Team t_MyTeam = element.GetComponent<Team>();
            if (t_MyTeam.m_Team == Team.TeamType.Red)
                redMinionsCount++;
            else
                blueMinionsCount++;
        }


        //Detect a mousedown on a minion object.
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask(new[] { "Selectables" })))
            {

                //Remove outline shader from previously selected minion.
                if (selected_object)
                    selected_object.GetComponentInChildren<Renderer>().material.shader = Shader.Find("Unlit/Texture");

                //Set this object as the selected object
                selected_object = hit.transform.gameObject;

                //Set the shader
                Renderer renderer = selected_object.GetComponentInChildren<Renderer>();
                renderer.material.shader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
                renderer.material.SetColor("_Color", Color.white);
                renderer.material.SetColor("_OutlineColor", Color.green);
                renderer.material.SetFloat("_Outline", 0.005f);

                //Apply a spell action if one is present.
                if (PendingAction)
                {
                    Instantiate(PendingAction, selected_object.transform.position, Quaternion.identity, selected_object.transform);
                    PendingAction = null;
                }

            }
            else if (selected_object)
            {
                //Set normal shader on gameObject's renderer
                selected_object.GetComponentInChildren<Renderer>().material.shader = Shader.Find("Unlit/Texture");
                selected_object = null;
            }
        }

        Minion t_FrontMinion = Minion.Front;
        if(t_FrontMinion != null)
        {
            Vector3 t_Average = Minion.Average;
            t_Average += t_FrontMinion.transform.position;
            t_Average /= 2.0f;

            float t_X = Mathf.Clamp(t_FrontMinion.transform.position.x, minimunLeftScroll, maximumLeftScroll);

            Vector3 t_TargetPosition = new Vector3(t_X, transform.position.y, transform.position.z);
            Vector3 t_Diff = t_TargetPosition - transform.position;

            transform.position += t_Diff * Time.deltaTime;
        }

        // Moving the Camera
        //Vector2 mouseEdge = MouseScreenEdge(20);

        //if (!(Mathf.Approximately(mouseEdge.x, 0f))) 
        //{
        //    //Move your camera depending on the sign of mouse.Edge.x
        //    Vector3 position = new Vector3(10 * Time.deltaTime, 0, 0);
        //    if (mouseEdge.x < 0)
        //        position.x *= -1;

        //    if (mouseEdge.x < 0 && transform.position.x >= minimunLeftScroll || mouseEdge.x > 0 && transform.position.x <= maximumLeftScroll)
        //    {
        //        transform.Translate(position);

        //        GameObject bg2 = GameObject.Find("NotThatFar");
        //        if (bg2)
        //        {
        //            position.x *= -0.1f;
        //            bg2.transform.Translate(position);
        //        }

        //        GameObject bg = GameObject.Find("TooFarBg");
        //        if (bg)
        //        {
        //            position.x *= 0.1f;
        //            bg.transform.Translate(position);
        //        }
        //    }
        //}

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
