using UnityEngine;
using System.Collections;

public class KarthusWall : MonoBehaviour
{
    public float percentage;
    public float duration;

    void OnTriggerEnter(Collider a_Collider)
    {
        if (a_Collider.tag == "Minion")
        {
            Minion targetMinion = a_Collider.gameObject.GetComponent<Minion>();
            targetMinion.ChangeSpeed(percentage, duration);
        }
    }
}
