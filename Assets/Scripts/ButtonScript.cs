using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject action;
    public KeyCode actionKey;

    protected bool isOver;
    protected OpacityFilter opacityFilter;
    protected Tooltip tooltipHolder;

    //Colors.
    private Color hover_color = new Color(0, 0, 0, 0.2f);
    private Color press_down_color = new Color(0, 0, 0, 0.4f);
    private Color normal_color = new Color(0, 0, 0, 0f);
    private Color pending_color = new Color(0, 100, 0, .5f);

    // Use this for initialization
    void Start ()
    {
        isOver = false;
        opacityFilter = gameObject.GetComponentInChildren<OpacityFilter>();
        tooltipHolder = gameObject.GetComponentInChildren<Tooltip>();
        if (tooltipHolder)
            tooltipHolder.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isOver)
        {
            if (Input.GetMouseButton(0))
            {
                opacityFilter.GetComponent<RawImage>().color = press_down_color;
            }
            else
            {
                opacityFilter.GetComponent<RawImage>().color = hover_color;
                if (tooltipHolder)
                    tooltipHolder.gameObject.SetActive(true);
            }
        }
        else
        {
            opacityFilter.GetComponent<RawImage>().color = normal_color;
            if (tooltipHolder)
                tooltipHolder.gameObject.SetActive(false);
        }

        //Apply the spell if:
        //A minion is selected, and we either pressed a button or we pressed the corresponding (1-5) key
        if (Global.selected_object)
        {
            if ((isOver && Input.GetMouseButtonDown(0)) || Input.GetKeyDown(actionKey))
            {
                Instantiate(action, Global.selected_object.transform.position, Quaternion.identity, Global.selected_object.transform);
            }
        }
        
        //Else, if an object is not selected and we pressed a button or pressed the corresponding key.
        else if((isOver && Input.GetMouseButtonDown(0)) || Input.GetKeyDown(actionKey))
        {
            Global.PendingAction = action;
        }

        //Highlight button if a pending action is set, and it's for this button
        if (Global.PendingAction == action && opacityFilter.GetComponent<RawImage>().color != pending_color)
           opacityFilter.GetComponent<RawImage>().color = pending_color;

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
    }
}
