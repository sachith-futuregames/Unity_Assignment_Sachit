using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageScript : MonoBehaviour
{
    public List<Player_Controller> EnemiesInRange;
    public int TeamID;
    public int Damage;


    private void OnTriggerEnter(Collider other)
    {
        Player_Controller Temp;
        if (Temp = other.gameObject.GetComponent<Player_Controller>())
        {
            if (Temp.TeamID != TeamID)
            {
                EnemiesInRange.Add(Temp);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player_Controller Temp;
        if (Temp = other.gameObject.GetComponent<Player_Controller>())
        {
            if (Temp.TeamID != TeamID)
            {
                EnemiesInRange.Remove(Temp);
            }
        }
    }

    public void SendDamage()
    {
        foreach(var Enemy in EnemiesInRange)
        {
            Enemy.TakeDamage(Damage);
        }
    }
}
