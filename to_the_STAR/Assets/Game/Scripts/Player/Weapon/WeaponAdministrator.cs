using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAdministrator : MonoBehaviour
{
    enum Weapon
    {
        hammer,
        flameThrower
    }

    Weapon_Hammer hammer;
    Weapon_Flamethrower flamethrower;

    Weapon weapon = Weapon.flameThrower;

    void Start()
    {
        hammer = gameObject.GetComponent<Weapon_Hammer>();
        flamethrower = gameObject.GetComponent<Weapon_Flamethrower>();
    }
    void Update()
    {
        if (Check())
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                weapon = Weapon.hammer;
                hammer.enable();
                flamethrower.disable();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                weapon = Weapon.flameThrower;
                hammer.disable();
                flamethrower.enable();
            }
        }
    }

    bool Check()
    {
        if (weapon == Weapon.hammer) return !hammer.isCharging();
        else if (weapon == Weapon.flameThrower) return !flamethrower.isTurnOn();
        else return true;
    }
}
