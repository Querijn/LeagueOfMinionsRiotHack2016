using UnityEngine;
using System.Collections;

public class SpeedModifier : MonoBehaviour
{
    public float percentage;
    public float duration;
    public int triggerLimit = int.MaxValue;

    void OnTriggerEnter(Collider a_Collider)
    {
        if (a_Collider.tag == "Minion")
        {
            Minion targetMinion = a_Collider.gameObject.GetComponent<Minion>();
            targetMinion.ChangeSpeed(percentage, duration);
        }
        triggerLimit -= 1;
    }

    void Update()
    {
        if (triggerLimit <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
