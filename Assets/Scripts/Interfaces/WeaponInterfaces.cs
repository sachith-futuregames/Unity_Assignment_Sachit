using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    //Function to toggle the ADS mode of the Shootable Weapon
    void ToggleZoom();

    //Function to move into the ADS Mode of the Shootable Weapon
    void Zoom();

    //Function to move out of the ADS Mode of the Shootable Weapon
    void UnZoom();

    //Function to Call to Execute the actual Shoot Function
    void Shoot();
}

public interface IAirDrop
{  
    //Function to Go into the Target Selection Mode
    void SetTarget();

    //Function to Call the External Force to Shoot
    void CallShoot();

}

public interface IHealthSystem
{
    //Function to Receive Damage
    void TakeDamage(int InDamage);

    //Function to Receive Health
    void GetHealth(int InHealth);

    //Function to Simulate Death of Object
    void DeathState();
}
