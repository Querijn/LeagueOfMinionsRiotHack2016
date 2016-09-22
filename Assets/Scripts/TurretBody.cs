using UnityEngine;
using System.Collections;

public class TurretBody : MonoBehaviour {

    public void UpdateHealth(float variation)
    {
        transform.parent.gameObject.SendMessage("UpdateHealth", variation);
    }

    public bool IsDead()
    {
        Tower turret = gameObject.GetComponent<Tower>();
        if (turret)
            return turret.IsDead();
        return false;
    }
}
