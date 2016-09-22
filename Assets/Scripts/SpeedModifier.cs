using UnityEngine;
using System.Collections;

public class SpeedModifier : MonoBehaviour
{
    public float percentage;
    public float speed_duration;
    public int triggerLimit = int.MaxValue;
    public float lifespan = float.PositiveInfinity;

    protected float endOfLife;

    void Start()
    {
        endOfLife = Time.time + lifespan;
    }

    void OnTriggerEnter(Collider a_Collider)
    {
        if (a_Collider.tag == "Minion")
        {
            Minion targetMinion = a_Collider.gameObject.GetComponent<Minion>();
            targetMinion.ChangeSpeed(percentage, speed_duration);
        }
        triggerLimit -= 1;
    }

    void Update()
    {
        if (triggerLimit <= 0 || Time.time >= endOfLife)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
