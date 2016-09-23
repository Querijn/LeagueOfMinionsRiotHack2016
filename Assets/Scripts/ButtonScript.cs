using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject action;

    protected bool isOver;
    protected OpacityFilter opacityFilter;

    // Use this for initialization
    void Start ()
    {
        isOver = false;
        opacityFilter = gameObject.GetComponentInChildren<OpacityFilter>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isOver)
        {
            if (Input.GetMouseButton(0))
            {
                opacityFilter.GetComponent<RawImage>().color = new Color(0, 0, 0, 0.4f);
            }
            else
            {
                opacityFilter.GetComponent<RawImage>().color = new Color(0, 0, 0, 0f);
            }
        }
        else
        {
            opacityFilter.GetComponent<RawImage>().color = new Color(0, 0, 0, 0.2f);
        }

        if (isOver && Input.GetMouseButtonDown(0) && Global.selected_object)
        {
            Instantiate(action, Global.selected_object.transform.position, Quaternion.identity, Global.selected_object.transform);
        }

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
